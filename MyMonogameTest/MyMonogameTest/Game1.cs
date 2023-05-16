using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyMonogameTest.Sprites;
using MyMonogameTest.Models;
using System.Runtime.ConstrainedExecution;
using MyMonogameTest.Sprites.World;

namespace MyMonogameTest
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SpriteFont spriteFont;

        public static int ScreenWidth;
        public static int ScreenHeight;

        private List<Sprite> _sprites;

        public Texture2D playerTexStart; 
        public Texture2D plataformTex;

        public int totalScore;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            this.Window.AllowUserResizing = true;
            this.Window.Title = "Octocat";

            ScreenWidth = graphics.PreferredBackBufferWidth;
            ScreenHeight = graphics.PreferredBackBufferHeight;

            //graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height; //pega altura do pc atual i think
            //graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width; //pega largura do pc atual i think
            //base.Window.IsBorderless = true;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            Input.UpMove = new Keys[] { Keys.Up, Keys.W};
            Input.DownMove = new Keys[] { Keys.Down, Keys.S};
            Input.LeftMove = new Keys[] { Keys.Left, Keys.A};
            Input.RightMove = new Keys[] { Keys.Right, Keys.D};
            Input.Fight = new Keys[] { Keys.D4, Keys.U};
            Input.Portal = new Keys[] { Keys.D1, Keys.P};
            base.Initialize();
        }

        protected override void LoadContent()
        {
            LoadStartContent();

            plataformTex = Content.Load<Texture2D>("plataforma_rosa");

            LoadSprites();

            foreach (var sprite in _sprites)
                sprite.LoadContent();
        }

        private void LoadStartContent()
        {
            //to draw
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //text
            spriteFont = Content.Load<SpriteFont>("File");

            //create a texture to start the player
            playerTexStart = Content.Load<Texture2D>("parado_1");
        }

        private void LoadSprites()
        {
            //all "objects"
            _sprites = new List<Sprite>()
            {
                //create Player
                new Player(playerTexStart, this){},
                new Plataform(plataformTex, new Vector2(300,200) ,300, 100){}
            };
        }

        protected override void Update(GameTime gameTime)
        {
            //clica para sair do jogo
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //update all sprites and get sprites to be removed and sprites to be added
            List<Sprite> spritesToRemove = new List<Sprite>();
            List<Sprite> spritesToAdd = new List<Sprite>();
            foreach (var sprite in _sprites)
            {
                //update all sprites
                sprite.Update(gameTime, _sprites, spritesToAdd);
                //get sprites to be removed
                if (sprite.IsRemoved)
                    spritesToRemove.Add(sprite);
            }

            base.Update(gameTime);

            //remove "dead" sprites
            foreach (Sprite spr in spritesToRemove)
                _sprites.Remove(spr);

            //add new sprites
            _sprites.AddRange(spritesToAdd);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            foreach (var sprite in _sprites)
                sprite.Draw(spriteBatch);

            foreach (var spr in _sprites)
                if (spr is Player player)
                {
                    spriteBatch.DrawString(spriteFont, "   Vidas: "+(spr.Health).ToString(), new Vector2(0, 10), Color.Black);
                    spriteBatch.DrawString(spriteFont, "   "+totalScore + " / 10 Pontos", new Vector2(0, 32), Color.Black);
                    //spriteBatch.DrawString(spriteFont, player.GetFacingDirection().ToString(), new Vector2(0, 54), Color.Black);
                }
                else if(spr is Weapon weapon)
                {
                    //spriteBatch.DrawString(spriteFont, weapon.Position.ToString(), new Vector2(0, 54), Color.Black);
                    //spriteBatch.DrawString(spriteFont, "   " + totalScore + " / 10 Pontos", new Vector2(0, 76), Color.Black);
                }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}