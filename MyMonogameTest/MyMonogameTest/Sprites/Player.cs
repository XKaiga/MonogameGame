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

enum AnimationState
{
    Parado, Movimento, Lutar, Portal
}

namespace MyMonogameTest.Sprites
{
    class Player : Sprite
    {
        public new KeyboardState _currentKey;
        public new KeyboardState _previousKey;

        private Game1 game;

        private Vector2 previousPosition = new Vector2(100, 100);

        #region PlayerWeapon

        private Texture2D weaponTex;
        private int weaponDamage = 3;

        // Define a TimeSpan representing the minimum time between shots
        TimeSpan timeBetweenShots = TimeSpan.FromSeconds(0.13);

        // Define a TimeSpan variable to keep track of the time since the last shot was fired
        TimeSpan timeSinceLastShot = TimeSpan.Zero;

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

        // Move the player based on input
        Vector2 movement;

        public Player(Texture2D texture, Game1 game) : base(texture)
        {
            this.game = game;
            Speed = 450f;
            Health = 10;
            Position = new Vector2(100, 100);
        }

        public override void LoadContent()
        {
            spritesAnimation = new Texture2D[4][];
            spritesAnimation[(int)AnimationState.Parado] = new[]
            {
                     game.playerTexStart,
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
            Move(gameTime, spritesToAdd);

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
                                this.Position.Y -= overlap.Height;
                            else
                                this.Position.Y += overlap.Height;
                        else
                            // Move the player left or right, depending on which side of the rectangle the collision occurred
                            if (this.Position.X < sprite.Position.X)
                            this.Position.X -= overlap.Width;
                        else
                            this.Position.X += overlap.Width;
                    }
                }
            }

        }

        private void Move(GameTime gameTime, List<Sprite> spritesToAdd)
        {
            _previousKey = _currentKey;
            _currentKey = Keyboard.GetState();

            // Move the player based on input
            movement = new Vector2(0, 0);

            //Up Movement
            if (Input.KeyPressed(Keys.Up, _previousKey, _currentKey))
            {
                movement.Y -= 1.0f;
                if (!inStaticAnimation)
                    ChangeAnimationState(AnimationState.Movimento);
            }

            //Down Movement
            if (Input.KeyPressed(Keys.Down, _previousKey, _currentKey))
            {
                movement.Y += 1.0f;
                if (!inStaticAnimation)
                    ChangeAnimationState(AnimationState.Movimento);
            }

            //Left Movement
            if (Input.KeyPressed(Keys.Left, _previousKey, _currentKey))
            {
                movement.X -= 1.0f;
                if (!inStaticAnimation)
                    ChangeAnimationState(AnimationState.Movimento);
            }

            //Right Movement
            if (Input.KeyPressed(Keys.Right, _previousKey, _currentKey))
            {
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

            // Keep the sprite on the screen
            Position = Vector2.Clamp(Position, new Vector2(0, 0), new Vector2(Game1.ScreenWidth - this.Rectangle.Width, Game1.ScreenHeight - this.Rectangle.Height));


            //Fight Animation
            if (Input.KeyPressed(Keys.F, _previousKey, _currentKey) && timeSinceLastShot >= timeBetweenShots)
            {
                spritesToAdd.Add(new Weapon(weaponTex, this, weaponDamage, GetFacingDirection()));
                ChangeAnimationState(AnimationState.Lutar);

                // Reset the time since the last shot was fired
                timeSinceLastShot = TimeSpan.Zero;

                //is in a static animation
                inStaticAnimation = true;
            }
            else if (timeSinceLastShot < timeBetweenShots)
                timeSinceLastShot += gameTime.ElapsedGameTime; //Increment the time since the last shot was fired
        }

        private Vector2 GetFacingDirection()
        {
            // In the player's update method
            Vector2 currentPosition = Position;
            Vector2 direction = currentPosition - previousPosition;
            if (direction != Vector2.Zero)
                direction.Normalize();

            ////see if player is still
            //isStill = currentPosition == previousPosition;

            // Store the current player position as the previous position for the next
            previousPosition = currentPosition;
            return movement == Vector2.Zero ? new Vector2(1, 0) : direction;
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
            //spriteBatch.Draw(_texture, Position, Color.White);
            spriteBatch.Draw(spritesAnimation[(int)currentState][currentFrame], Position, Color.White);
        }
    }
}
