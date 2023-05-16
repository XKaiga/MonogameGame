using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMonogameTest.Sprites
{
    class Sprite
    {
        protected KeyboardState _currentKey;
        protected KeyboardState _previousKey;

        public Texture2D[][] spritesAnimation;

        protected Texture2D _texture;
        public Vector2 Position;
        public Vector2 Origin;
        public float Speed = 0f;

        private int health = -1;
        public int Health { get => this.health; set { this.health = value < 0 ? 0 : value; } }

        private bool isRemoved = false;
        public bool IsRemoved
        {
            get => isRemoved || Health == 0;
            set { isRemoved = value; }
        }

        public Sprite Parent;

        public Sprite(Texture2D texture)
        {
            _texture = texture;

            Origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
        }

        private Rectangle rectangle;
        public Rectangle Rectangle
        {
            get
            {
                if (_texture != null)
                    return rectangle.IsEmpty ? new Rectangle((int)Position.X, (int)Position.Y, _texture.Width, _texture.Height) : rectangle;
                throw new Exception("Unknown sprite");
            }
            set { rectangle = value; }
        }

        public virtual void LoadContent()
        {
        }

        public virtual void Update(GameTime gameTime, List<Sprite> sprite, List<Sprite> spritesToAdd)
        {
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Rectangle, Color.White);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}


