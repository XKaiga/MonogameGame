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
    enum AreaType
    {
        nextLevel
    }

    class Area : Sprite
    {
        private Game1 game;

        private AreaType type;

        private int height;
        private int width;

        public Area(Texture2D texture, Game1 game, Vector2 position, int width, int height, AreaType type) : base(texture)
        {
            Position = position;
            this.width = width;
            this.height = height;
            this.type = type;
            this.game = game;
        }
        public override void Update(GameTime gameTime, List<Sprite> sprites, List<Sprite> spritesToAdd)
        {
            foreach (var sprite in sprites)
            {
                if (sprite is Area)
                    continue;

                // Colission
                if (sprite is Player player && player.Rectangle.Intersects(this.Rectangle))
                {
                    switch (type)
                    {
                        case AreaType.nextLevel:
                            game.ChangeLevel();
                            break;
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, width, height);
            spriteBatch.Draw(_texture, Rectangle, new Rectangle(0, 0, Rectangle.X, Rectangle.Y), Color.White * 0.5f);
        }
    }
}