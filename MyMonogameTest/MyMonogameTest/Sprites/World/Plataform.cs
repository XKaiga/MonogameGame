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

            //Color[] data = new Color[_texture.Height * Game1.ScreenWidth];
            //for (int i = 0; i < data.Length; ++i) data[i] = Color.Green;
            //_texture.SetData(data);

            ////Rectangle rectangle = new Rectangle(0, Game1.ScreenWidth, 100, Game1.ScreenHeight);
            //Rectangle rectangle = new Rectangle(0, Game1.ScreenHeight- _texture.Height, Game1.ScreenWidth, _texture.Height);
            //Rectangle = rectangle;
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, width, height);
            spriteBatch.Draw(_texture, Rectangle, new Rectangle(0,0, Rectangle.X,Rectangle.Y), Color.White);
            
        }
    }
}
