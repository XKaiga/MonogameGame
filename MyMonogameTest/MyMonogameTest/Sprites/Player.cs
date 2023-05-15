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

enum AnimationState
{
    Parado, Movimento, Lutar, Portal
}

namespace MyMonogameTest.Sprites
{
    class Player : Sprite
    {
        private Game1 game;

        private AnimationState currentState = AnimationState.Parado;
        private int currentFrame = 0;
        private int delta = 0;

        private float timer;
        private float interval = 0;

        public bool isDead
        {
            get => IsRemoved = Health <= 0;
        }

        public Player(Texture2D texture, Game1 game) : base(texture)
        {
            this.game = game;
            Speed = 5f;
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
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites)
        {
            delta = 0;
            Move(); //Attack & Powers

            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (timer > interval)
            {
                currentFrame++;
                if (currentFrame >= spritesAnimation[(int)currentState].Length)
                    currentFrame = 0;
                timer = 0f;
            }

            if (isDead)
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

        public void Move()
        {
            _previousKey = _currentKey;
            _currentKey = Keyboard.GetState();

            //Up Movement
            if (Input.KeyPressed(Keys.Up))
            {
                Position.Y -= Speed;
                ChangeAnimationState(AnimationState.Movimento);
            }

            //Down Movement
            if (Input.KeyPressed(Keys.Down))
            {
                Position.Y += Speed;
                ChangeAnimationState(AnimationState.Movimento);            
            }

            //Left Movement
            if (Input.KeyPressed(Keys.Left))
            {
                Position.X -= Speed;
                ChangeAnimationState(AnimationState.Movimento);
            }

            //Right Movement
            if (Input.KeyPressed(Keys.Right))
            {
                Position.X += Speed;
                ChangeAnimationState(AnimationState.Movimento);
            }

            //Not Moving
            if (delta == 0)
                ChangeAnimationState(AnimationState.Parado);

            // Keep the sprite on the screen
            Position = Vector2.Clamp(Position, new Vector2(0, 0), new Vector2(Game1.ScreenWidth - this.Rectangle.Width, Game1.ScreenHeight - this.Rectangle.Height));
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
                    interval = 167f;
                    break;
                case AnimationState.Portal:
                    interval = 143f;
                    break;
            }

            //Starts Animation from start
            if (currentState != animationState)
                currentFrame = 0;

            //change animation state and delta
            currentState = animationState;
            delta = (int)animationState;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(_texture, Position, Color.White);
            spriteBatch.Draw(spritesAnimation[(int)currentState][currentFrame], Position, Color.White);
        }
    }
}
