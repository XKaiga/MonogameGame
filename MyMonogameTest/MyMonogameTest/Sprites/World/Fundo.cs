using Microsoft.Xna.Framework;
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
    class Fundo : Sprite
    {
        public Fundo(Texture2D texture, Game1 game1) : base(texture)
        {
            float dif = (float)Game1.ScreenWidth / _texture.Width;
            Rectangle = new Rectangle(0,0,(int)(_texture.Width*dif),Game1.ScreenHeight);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
