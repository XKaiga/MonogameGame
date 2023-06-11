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
        private Game1 game;

        public Plataform(Texture2D texture, Game1 game, Vector2 position, int width, int height) : base(texture)
        {
            this.game = game;

            Position = position;

            Rectangle = new Rectangle((int)Position.X, (int)Position.Y,
                (int)(width * game.scalingFactor),
                game.level == 2 ? (int)(height / 2 * game.scalingFactor) : (int)(height * game.scalingFactor));
        }

        public Plataform(Texture2D texture, Game1 game, Vector2 position, Vector2 size) : base(texture)
        {
            this.game = game;

            Position = position;

            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, 
                (int)(size.X * game.scalingFactor), 
                game.level == 2 ? (int)(size.Y / 2 * game.scalingFactor) : (int)(size.Y * game.scalingFactor));
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites, List<Sprite> spritesToAdd)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, new Rectangle(Rectangle.X, Rectangle.Y, Rectangle.Width, game.level == 2 ? Rectangle.Height*2: Rectangle.Height), Color.White);
        }
    }
}
