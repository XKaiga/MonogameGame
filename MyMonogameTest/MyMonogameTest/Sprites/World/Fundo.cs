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
        private new Game1 game;

        public Fundo(Texture2D texture, Game1 game1) : base(texture)
        {
            game = game1;

            Rectangle = new Rectangle(0, 0, game.ScreenWidth, game.ScreenHeight);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
