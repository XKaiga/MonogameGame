using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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

namespace MyMonogameTest.Sprites.World
{
    class Plataform : Sprite
    {
        private int height;
        private int width;

        public Plataform(Texture2D texture, Vector2 position, int width, int height) : base(texture)
        {
            Position = position;
            this.width = width;
            this.height = height;
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites, List<Sprite> spritesToAdd)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, width, height);
            spriteBatch.Draw(_texture, Rectangle, Color.White);

        }
    }
}
