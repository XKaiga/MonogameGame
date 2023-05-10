using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

enum Directions
{
    Movimento, Portal, Lutar, Parado
}

namespace MyMonogameTest
{
    class Player
    {
        public Point position;
        private Game1 game;
        private Texture2D[][] sprites;
        private int speed =1;
        Timer timer;



        public Player(Game1 game, Point position)
        {
            this.game = game;
            this.position = position;
        }

        public void PlayerPosition(int x, int y)
        {
            position = new Point(x, y);
        }

        public void LoadContents()
        {
            sprites = new Texture2D[4][];
            sprites[(int)Directions.Movimento] = new[] {
                game.Content.Load<Texture2D>("movimento_1"),
                game.Content.Load<Texture2D>("movimento_2"),
                game.Content.Load<Texture2D>("movimento_3"),
                game.Content.Load<Texture2D>("movimento_4"),
                game.Content.Load<Texture2D>("movimento_5"),
                game.Content.Load<Texture2D>("movimento_6")
            };
            sprites[(int)Directions.Lutar] = new[]
            {
                game.Content.Load<Texture2D>("lutar_1"),
                game.Content.Load<Texture2D>("lutar_2"),
                game.Content.Load<Texture2D>("lutar_3"),
                game.Content.Load<Texture2D>("lutar_4"),
                game.Content.Load<Texture2D>("lutar_5"),
                game.Content.Load<Texture2D>("lutar_6")
            };
            sprites[(int)Directions.Portal] = new[]
            {
                game.Content.Load<Texture2D>("portal_1"),
                game.Content.Load<Texture2D>("portal_2"),
                game.Content.Load<Texture2D>("portal_3"),
                game.Content.Load<Texture2D>("portal_4"),
                game.Content.Load<Texture2D>("portal_5"),
                game.Content.Load<Texture2D>("portal_6"),
                game.Content.Load<Texture2D>("portal_7")
            };
            sprites[(int)Directions.Parado] = new[]
            {
                 game.Content.Load<Texture2D>("parado_1"),
                 game.Content.Load<Texture2D>("parado_2")
            };
        }

        public void Update(GameTime gameTime)
        {

            if (Keyboard.GetState().IsKeyDown(Keys.Right) || Keyboard.GetState().IsKeyDown(Keys.D))
            {
                position.X=(position.X+5) * speed;

            }
            if(Keyboard.GetState().IsKeyDown(Keys.Left) || Keyboard.GetState().IsKeyDown(Keys.A)){ 
                position.X=(position.X-5)*speed;
            }
            if(Keyboard.GetState().IsKeyDown(Keys.Down) || Keyboard.GetState().IsKeyDown(Keys.S)) {
                position.Y=(position.Y+5)*speed;
            }
            if(Keyboard.GetState().IsKeyDown(Keys.Up) || Keyboard.GetState().IsKeyDown(Keys.W))
            {
                position.Y = (position.Y - 5) * speed;
            }

        }

        public void Draw(SpriteBatch sb)
        {
            
            Rectangle rect = new Rectangle(position.ToVector2().ToPoint(), new Point(100, 100));
            //if (!Keyboard.GetState().IsKeyDown(Keys.S))
            //{
            //    if()
            //    sb.Draw(sprites[frame][0], rect, Color.White);
            //    sb.Draw(sprites[frame][1],rect,Color.White);
            //}
           

        }

    }
}
