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
using System.Timers;

namespace MyMonogameTest.Sprites
{
    enum enemyType
    {
        follow, shooter, followShooter, bomb
    }

    class Enemy : Sprite
    {
        private Game1 game;

        public Texture2D enemyTexStart;

        private Vector2 direction;
        private bool movingRight = false;

        private int damage;

        private enemyType type;

        private float scaleX;
        private float scaleY;

        #region Weapon

        private Texture2D weaponTex;
        private float weaponSpeed;

        // Define a TimeSpan representing the minimum time between shots
        TimeSpan timeBetweenShots = TimeSpan.FromSeconds(1);
        // Define a TimeSpan variable to keep track of the time since the last shot was fired
        public TimeSpan timeSinceLastShot = TimeSpan.Zero;

        #endregion

        public Enemy(Texture2D texture, Game1 game, Vector2 position, int damage, enemyType type, Vector2 size) : base(texture)
        {
            if (type == enemyType.bomb)
            {
                scaleX = size.X / Rectangle.Width * game.scalingFactor;
                scaleY = size.Y / Rectangle.Height * game.scalingFactor;
                Rectangle = new Rectangle((int)position.X, (int)position.Y, (int)(size.X / 1.3f * game.scalingFactor), (int)(size.Y / 1.3f * game.scalingFactor));
                Origin = size / 2;
            }
            this.game = game;

            this.damage = damage;

            this.type = type;

            enemyTexStart = texture;
            Position = position;

            Health = 8;

            Speed = type == enemyType.follow ? 250f : 165f;

            weaponSpeed = type == enemyType.shooter ? 200f : 200f;

            Speed *= game.scalingFactor;
            weaponSpeed *= game.scalingFactor;
        }


        public override void LoadContent()
        {
            weaponTex = game.Content.Load<Texture2D>("yoyo");
        }


        public override void Update(GameTime gameTime, List<Sprite> sprites, List<Sprite> spritesToAdd)
        {
            if (IsRemoved)
                return;

            // Keep the sprite on the screen
            Position = Vector2.Clamp(Position, new Vector2(0, 0), new Vector2(game.ScreenWidth - this.Rectangle.Width, game.ScreenHeight - this.Rectangle.Height));

            foreach (var sprite in sprites)
            {
                if (sprite is Enemy)
                    continue;

                if (sprite is Player plyr)
                {
                    if (this.type == enemyType.follow || this.type == enemyType.followShooter)
                        Move(gameTime, plyr);
                    if (this.type == enemyType.shooter || this.type == enemyType.followShooter)
                        Shoot(gameTime, sprites, spritesToAdd);
                }
                // Colission
                if (sprite.Rectangle.Intersects(this.Rectangle))
                {
                    // Behaviours
                    if (sprite is Plataform)
                    {
                        // Calculate the amount of overlap between the two rectangles
                        Rectangle overlap = Rectangle.Intersect(this.Rectangle, sprite.Rectangle);

                        // Determine which direction to move the enemy to resolve the collision
                        if (overlap.Width > overlap.Height)
                            // Move the enemy up or down, depending on which side of the rectangle the collision occurred
                            if (this.Position.Y < sprite.Position.Y)
                            {
                                this.Position.Y -= overlap.Height;
                            }
                            else
                                this.Position.Y += overlap.Height;
                        else
                        {
                            // Move the enemy left or right, depending on which side of the rectangle the collision occurred
                            if (this.Position.X < sprite.Position.X)
                                this.Position.X -= overlap.Width;
                            else
                                this.Position.X += overlap.Width;
                        }
                    }

                    if (sprite is Player player)
                    {
                        player.Health -= damage;
                    }
                }
            }
        }

        private void Shoot(GameTime gameTime, List<Sprite> sprites, List<Sprite> spritesToAdd)
        {
            //Shoot
            if (timeSinceLastShot >= timeBetweenShots)
            {
                spritesToAdd.Add(new Weapon(weaponTex, this, damage, GetDirection(sprites.Find(spr => spr is Player) as Player), weaponSpeed));
                timeSinceLastShot = TimeSpan.Zero;
            }
            else if (timeSinceLastShot < timeBetweenShots)
                timeSinceLastShot += gameTime.ElapsedGameTime; //Increment the time since the last shot was fired
        }

        public void Move(GameTime gameTime, Player player)
        {
            direction = GetDirection(player);
            if (direction != Vector2.Zero)
                Position += direction * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        private Vector2 GetDirection(Player player)
        {
            _previousKey = _currentKey;
            _currentKey = Keyboard.GetState();

            //define position / direction / moveState
            direction = player.Position - Position;

            //normalize direction
            if (direction != Vector2.Zero && (direction.X < -5 || direction.X > 5 || direction.Y < -5 || direction.Y > 5))
                direction.Normalize(); // Normaliza o vetor para ter comprimento 1
            else
                direction = Vector2.Zero;

            return direction;
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            //invert sprite?
            SpriteEffects effects = movingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            var drawPosition = type == enemyType.bomb ? Position : Position + Origin;

            // Desenhe o sprite
            spriteBatch.Draw(_texture, drawPosition, null, Color.LightPink, 0f, Origin, type == enemyType.bomb ? new Vector2(scaleX, scaleY) : Vector2.One, effects, 0f);
        }
    }
}
