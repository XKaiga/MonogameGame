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
using MonoGame.Extended.Screens;
using MyMonogameTest.Levels;
using MonoGame.Extended.Screens.Transitions;
using System.ComponentModel;

namespace MyMonogameTest
{
    public class Game1 : Game
    {
        public GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public int ScreenWidth;
        public int ScreenHeight;

        public int totalScore;

        private ScreenManager screenManager;

        public int level = 2;
        private bool hKeyPressed = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            this.Window.AllowUserResizing = true;
            this.Window.Title = "Octocat";

            ScreenWidth = graphics.PreferredBackBufferWidth;
            ScreenHeight = graphics.PreferredBackBufferHeight;

            screenManager = new ScreenManager();
            Components.Add(screenManager);

            //graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height; //pega altura do pc atual i think
            //graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width; //pega largura do pc atual i think
            //base.Window.IsBorderless = true;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            Input.UpMove = new Keys[] { Keys.Up, Keys.W , Keys.Space};
            Input.DownMove = new Keys[] { Keys.Down, Keys.S };
            Input.LeftMove = new Keys[] { Keys.Left, Keys.A };
            Input.RightMove = new Keys[] { Keys.Right, Keys.D };
            Input.Fight = new Keys[] { Keys.D4, Keys.U };
            Input.Portal = new Keys[] { Keys.D1, Keys.P };
            base.Initialize();
            //LoadMenu();
            LoadLevel2();
        }

        protected override void LoadContent()
        {
            //to draw
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        private void LoadMenu()
        {
            screenManager.LoadScreen(new Menu(this), new FadeTransition(GraphicsDevice, Color.White));
        }

        private void LoadLevel1()
        {
            screenManager.LoadScreen(new Level1(this, spriteBatch), new FadeTransition(GraphicsDevice, Color.White));
        }

        private void LoadLevel2()
        {
            screenManager.LoadScreen(new Level2(this, spriteBatch), new FadeTransition(GraphicsDevice, Color.White));
        }

        private void LoadLevel3()
        {
            screenManager.LoadScreen(new Level3(this), new FadeTransition(GraphicsDevice, Color.White));
        }

        private void LoadLevel4()
        {
            screenManager.LoadScreen(new Level4(this), new FadeTransition(GraphicsDevice, Color.White));
        }

        private void LoadLevel5()
        {
            screenManager.LoadScreen(new Level5(this), new FadeTransition(GraphicsDevice, Color.White));
        }
        
        protected override void Update(GameTime gameTime)
        {
            //clica para sair do jogo
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.H) && !hKeyPressed)
                ChangeLevel();
            else if (keyboardState.IsKeyUp(Keys.H))
                hKeyPressed = false;

            base.Update(gameTime);

        }

        protected void ChangeLevel()
        {
            hKeyPressed = true;
            level++;
            switch (level)
            {
                case 1:
                    LoadLevel1();
                    break;
                case 2:
                    LoadLevel2();
                    break;
                case 3:
                    LoadLevel3();
                    break;
                case 4:
                    LoadLevel4();
                    break;
                case 5:
                    LoadLevel5();
                    break;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}