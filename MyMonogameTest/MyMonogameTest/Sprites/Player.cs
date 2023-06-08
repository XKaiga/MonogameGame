using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyMonogameTest.Sprites;
using MyMonogameTest.Models;
using System.Reflection.Metadata;
using MyMonogameTest.Sprites.World;
using System.Diagnostics.SymbolStore;
using System.Threading;
using MyMonogameTest.Levels;
using MonoGame.Extended.Timers;
using Microsoft.Xna.Framework.Content;

enum AnimationState
{
    Parado, Movimento, Lutar, Portal
}

enum MouseMoveState
{
    Moving, Staying, breaking
}

namespace MyMonogameTest.Sprites
{
    class Player : Sprite
    {
        public ContentManager Content;
        private SpriteFont spriteFont;

        #region Movement

        private Vector2 previousPosition;

        public new KeyboardState _currentKey;
        public new KeyboardState _previousKey;

        private Vector2 direction;
        private Vector2 movement;// Move the player based on input
        private bool movingRight = true;

        #region Gravity&Jump

        public float gravity = 300f; //the power of gravity
        public bool gravityOn = false;
        public bool isOnGround = true;

        private float jumpLevel = 0; //the higher y when jumping, used to know when gravity takes effect
        private float jumpForce = 175f; //how heigh jumps
        private float jumpSpeed = 10f; //how fast jumps

        #endregion

        #region Mouse

        private bool mouseClicked = false;
        public Vector2 mousePosition = Vector2.Zero;
        public float distanceToTarget;
        private MouseMoveState mouseMoveState = MouseMoveState.Staying;

        public float breakSpeed;//speed in which player walks when breaking
        public float breakSpd = 8f;//removes from breakSpeed until player stops
        private float distanceToTargetBreak;

        // Define a TimeSpan representing the minimum time between moves
        TimeSpan timeBetweenMoves = TimeSpan.FromSeconds(0.5);
        // Define a TimeSpan variable to keep track of the time since the last move was done
        public TimeSpan timeSinceLastMove = TimeSpan.Zero;

        #endregion

        #endregion

        private Game1 game;

        public Texture2D playerTexStart;

        #region Weapon

        private Texture2D weaponTex;
        private int weaponDamage = 3;

        // Define a TimeSpan representing the minimum time between shots
        TimeSpan timeBetweenShots = TimeSpan.FromSeconds(0.13);
        // Define a TimeSpan variable to keep track of the time since the last shot was fired
        public TimeSpan timeSinceLastShot = TimeSpan.Zero;

        #endregion

        #region Animation

        private AnimationState currentState = AnimationState.Parado;
        private int currentFrame = 0;

        //time passed between animations
        private float timer;

        //fps, frames per second
        private float interval = 0;

        #endregion

        // Before changing animation, waits to finish current static animation
        private bool inStaticAnimation = false;

        public Player(Texture2D texture, Game1 game) : base(texture)
        {

            this.game = game;
            Content = game.Content;

            playerTexStart = texture;
            Position = new Vector2(100, game.ScreenHeight - this.Rectangle.Height);

            Health = 2;

            Speed = 450f;
            breakSpeed = Speed;
            distanceToTargetBreak = game.level == 1 ? 100 : 58;
        }

        public override void LoadContent()
        {
            spriteFont = Content.Load<SpriteFont>("File");

            spritesAnimation = new Texture2D[4][];
            spritesAnimation[(int)AnimationState.Parado] = new[]
            {
                     playerTexStart,
                     game.Content.Load<Texture2D>("parado_2")
            };
            spritesAnimation[(int)AnimationState.Movimento] = new[]
            {
                    game.Content.Load<Texture2D>("movimento_1"),
                    game.Content.Load<Texture2D>("movimento_2"),
                    game.Content.Load<Texture2D>("movimento_3"),
                    game.Content.Load<Texture2D>("movimento_4"),
                    game.Content.Load<Texture2D>("movimento_5"),
                    game.Content.Load<Texture2D>("movimento_6")
            };
            spritesAnimation[(int)AnimationState.Lutar] = new[]
            {
                    game.Content.Load<Texture2D>("lutar_1"),
                    game.Content.Load<Texture2D>("lutar_2"),
                    game.Content.Load<Texture2D>("lutar_3"),
                    game.Content.Load<Texture2D>("lutar_4"),
                    game.Content.Load<Texture2D>("lutar_5"),
                    game.Content.Load<Texture2D>("lutar_6")
            };
            spritesAnimation[(int)AnimationState.Portal] = new[]
            {
                    game.Content.Load<Texture2D>("portal_1"),
                    game.Content.Load<Texture2D>("portal_2"),
                    game.Content.Load<Texture2D>("portal_3"),
                    game.Content.Load<Texture2D>("portal_4"),
                    game.Content.Load<Texture2D>("portal_5"),
                    game.Content.Load<Texture2D>("portal_6"),
                    game.Content.Load<Texture2D>("portal_7")
            };

            weaponTex = game.Content.Load<Texture2D>("yoyo");
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites, List<Sprite> spritesToAdd)
        {
            // Keep the sprite on the screen
            Position = Vector2.Clamp(Position, new Vector2(0, 0), new Vector2(game.ScreenWidth - this.Rectangle.Width, game.ScreenHeight - this.Rectangle.Height));
            if (Position.Y == game.ScreenHeight - this.Rectangle.Height)
                gravityOn = false;

            //animation
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timer > interval)
            {
                currentFrame++;
                if (currentFrame >= spritesAnimation[(int)currentState].Length)
                    currentFrame = 0;
                timer = 0f;
            }

            //static animation finished
            if (currentFrame == 0)
                inStaticAnimation = false;

            if (IsRemoved)
                return;

            foreach (var sprite in sprites)
            {
                if (sprite is Player)
                    continue;

                // Colission
                if (sprite.Rectangle.Intersects(this.Rectangle))
                {
                    // Behaviours
                    if (sprite is Plataform)
                    {
                        // Calculate the amount of overlap between the two rectangles
                        Rectangle overlap = Rectangle.Intersect(this.Rectangle, sprite.Rectangle);

                        // Determine which direction to move the player to resolve the collision
                        if (overlap.Width > overlap.Height)
                            // Move the player up or down, depending on which side of the rectangle the collision occurred
                            if (this.Position.Y < sprite.Position.Y)
                            {
                                this.Position.Y -= overlap.Height;
                                gravityOn = false;
                                this.isOnGround = true;
                            }
                            else
                                this.Position.Y += overlap.Height;
                        else
                        {
                            // Move the player left or right, depending on which side of the rectangle the collision occurred
                            if (this.Position.X < sprite.Position.X)
                                this.Position.X -= overlap.Width;
                            else
                                this.Position.X += overlap.Width;
                        }
                    }
                }
            }

        }

        public void Move(GameTime gameTime, List<Sprite> spritesToAdd)
        {
            _previousKey = _currentKey;
            _currentKey = Keyboard.GetState();

            // Move the player based on input
            movement = new Vector2(0, 0);

            //Left Movement
            if (Input.KeyPressed(Keys.Left, _previousKey, _currentKey))
            {
                movingRight = false;
                movement.X -= 1.0f;
                if (!inStaticAnimation)
                    ChangeAnimationState(AnimationState.Movimento);
            }

            //Right Movement
            if (Input.KeyPressed(Keys.Right, _previousKey, _currentKey))
            {
                movingRight = true;
                movement.X += 1.0f;
                if (!inStaticAnimation)
                    ChangeAnimationState(AnimationState.Movimento);
            }

            //Moving?
            if (movement != Vector2.Zero)
            {
                //ensure that the movement vector has a length of 1
                movement.Normalize();
                Position += movement * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (!inStaticAnimation)
                ChangeAnimationState(AnimationState.Parado);

            //gravity
            if (gravityOn)
            {
                Position.Y += gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (!inStaticAnimation)
                    ChangeAnimationState(AnimationState.Movimento);
            }

            //Jump
            if ((Input.KeyPressed(Keys.Up, _previousKey, _currentKey) && isOnGround && !gravityOn) || !isOnGround)
            {
                isOnGround = false;
                if (jumpLevel == 0)
                    jumpLevel = Position.Y - jumpForce;

                Position.Y -= jumpSpeed;

                if (!inStaticAnimation)
                    ChangeAnimationState(AnimationState.Movimento);

                if (Position.Y <= jumpLevel)
                {
                    isOnGround = true;
                    jumpLevel = 0;
                    gravityOn = true;
                }
            }

            Fight(gameTime, spritesToAdd, false);
        }

        public void MouseMove(GameTime gameTime, List<Sprite> spritesToAdd)
        {
            _previousKey = _currentKey;
            _currentKey = Keyboard.GetState();

            MouseState mouseState = Mouse.GetState();

            //Time the movement, to see if player can or not make another move
            if (mouseState.LeftButton == ButtonState.Pressed && timeSinceLastMove >= timeBetweenMoves)
            {
                // Reset the time since the last move was done
                timeSinceLastMove = TimeSpan.Zero;
                mouseClicked = true;
            }
            else if (timeSinceLastMove < timeBetweenMoves)
                timeSinceLastMove += gameTime.ElapsedGameTime; //Increment the time since the last move was done

            //define position / direction / moveState
            if (mouseState.LeftButton == ButtonState.Released && mouseClicked)
            {
                mouseClicked = false;
                ResetMouseMove();
                mousePosition = new Vector2(mouseState.X, (game.level == 1 && mouseState.Y < 100) ? 100 : mouseState.Y);
                direction = mousePosition - Position;
                mouseMoveState = MouseMoveState.Moving;
            }

            //move the player
            if (mouseMoveState == MouseMoveState.Moving && direction != Vector2.Zero)
            {
                direction.Normalize(); // Normaliza o vetor para ter comprimento 1
                distanceToTarget = Vector2.Distance(Position, mousePosition);
                Position += direction * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (!inStaticAnimation)
                    ChangeAnimationState(AnimationState.Movimento);
            }

            //starts to use the breaks
            if ((distanceToTarget != 0 && distanceToTarget < distanceToTargetBreak && mouseMoveState != MouseMoveState.Staying) || mouseMoveState == MouseMoveState.breaking)
            {
                mouseMoveState = MouseMoveState.breaking;
                Position += direction * breakSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                breakSpeed -= breakSpd;
                if (!inStaticAnimation)
                    ChangeAnimationState(AnimationState.Movimento);
                if (breakSpeed < 1)
                    mouseMoveState = MouseMoveState.Staying;
            }

            //stops the player
            if (mouseMoveState == MouseMoveState.Staying && !inStaticAnimation)
                ChangeAnimationState(AnimationState.Parado);


            Fight(gameTime, spritesToAdd, true);
        }

        private void ResetMouseMove()
        {
            breakSpeed = Speed;
            direction = Vector2.Zero;
        }

        private void Fight(GameTime gameTime, List<Sprite> spritesToAdd, bool mouse)
        {
            //Fight Animation
            if (Input.KeyPressed(Keys.F, _previousKey, _currentKey) && timeSinceLastShot >= timeBetweenShots)
            {
                spritesToAdd.Add(new Weapon(weaponTex, this, weaponDamage, GetFacingDirection(mouse)));
                ChangeAnimationState(AnimationState.Lutar);

                // Reset the time since the last shot was fired
                timeSinceLastShot = TimeSpan.Zero;

                //is in a static animation
                inStaticAnimation = true;
            }
            else if (timeSinceLastShot < timeBetweenShots)
                timeSinceLastShot += gameTime.ElapsedGameTime; //Increment the time since the last shot was fired
        }

        private Vector2 GetFacingDirection(bool withCursor)
        {
            // In the player's update method
            Vector2 currentPosition = Position;
            Vector2 direction = Vector2.Zero;

            if (withCursor)
                direction = new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y) - currentPosition;
            else
            {
                direction = currentPosition - previousPosition;
                // Store the current player position as the previous position
                previousPosition = currentPosition;
            }

            if (direction == Vector2.Zero)
                return new Vector2(1, 0);

            direction.Normalize();
            return direction;
        }

        private void ChangeAnimationState(AnimationState animationState)
        {
            //Update Frames Per Second
            switch (animationState)
            {
                case AnimationState.Parado:
                    interval = 500f;
                    break;
                case AnimationState.Movimento:
                    interval = 167f;
                    break;
                case AnimationState.Lutar:
                    interval = 41.75f;
                    break;
                case AnimationState.Portal:
                    interval = 143f;
                    break;
            }

            //Starts Animation from start
            if (currentState != animationState)
                currentFrame = 0;

            //change animation state
            currentState = animationState;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //invert sprite?
            SpriteEffects effects = movingRight? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            // Desenhe o sprite
            spriteBatch.Draw(spritesAnimation[(int)currentState][currentFrame], Position+Origin, null, Color.White, 0f, Origin, 1f, effects, 0f);
        }
    }
}
