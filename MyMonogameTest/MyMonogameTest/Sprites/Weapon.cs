﻿using System;
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
using System.Runtime.CompilerServices;
using System.Threading;

namespace MyMonogameTest.Sprites
{
    class Weapon : Sprite
    {
        private int damage;

        //direction * speed
        public Vector2 velocity;

        public Weapon(Texture2D texture, Sprite parentSprite, int damage, Vector2 direction, float speed = 0) : base(texture)
        {
            Parent = parentSprite;
            this.damage = damage;
            Speed = speed != 0? speed : Parent.Speed * 1.5f;
            Position = Parent.Position;

            //direction
            velocity += direction * Speed;

            //scalling down
            if (parentSprite is Player) {
                Rectangle = new Rectangle(Rectangle.X + Parent.Rectangle.Width / 2, Rectangle.Y + Parent.Rectangle.Height / 2, (int)(Rectangle.Width / 18.0f), (int)(Rectangle.Height / 18.0f));
            }
            else
            {
                Rectangle = new Rectangle(Rectangle.X + Parent.Rectangle.Width / 2, Rectangle.Y + Parent.Rectangle.Height / 2, (int)(Rectangle.Width / 4.0f), (int)(Rectangle.Height / 4.0f));
            }
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites, List<Sprite> spritesToAdd)
        {
            Position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Rectangle.Width, Rectangle.Height);

            foreach (var sprite in sprites)
            {
                if (this.Parent is Player)
                {
                    if (sprite is Player || sprite is Weapon || sprite is Fundo || sprite is Area)
                        continue;
                }
                else
                {
                    if (sprite is Enemy || sprite is Weapon || sprite is Fundo || sprite is Area)
                        continue;
                }

                // Colission
                if (sprite.Rectangle.Intersects(this.Rectangle))
                {
                    if (sprite.Health > 0)
                        sprite.Health -= damage;
                    this.IsRemoved = true;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, new Rectangle((int)Position.X, (int)Position.Y, Rectangle.Width, Rectangle.Height), Color.White);
        }
    }
}
