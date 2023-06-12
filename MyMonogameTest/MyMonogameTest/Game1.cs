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
using Microsoft.Xna.Framework.Media;
using MyMonogameTest.Powers;

namespace MyMonogameTest
{
    public class Game1 : Game
    {
        public GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public Matrix _viewMatrix;

        public int ScreenWidth;
        public int ScreenHeight;
        //public float ZoomLevel = 2f;
        public float ZoomLevel = 2f;

        public bool usingMouseMovement;

        public int totalScore = 10;
        public int currScore = 0;

        public Song music;
        public bool isFirstMusic = false;
        private ScreenManager screenManager;

        //public int level = -2;
        public int level = -2;
        private bool hKeyPressed = false;

        public float scalingFactor = 1;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            this.Window.AllowUserResizing = true;
            this.Window.Title = "Octocat";

            screenManager = new ScreenManager();
            Components.Add(screenManager);

            //graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height; //pega altura do pc atual i think
            //graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width; //pega largura do pc atual i think

            ScreenWidth = graphics.PreferredBackBufferWidth;
            ScreenHeight = graphics.PreferredBackBufferHeight;

            //base.Window.IsBorderless = true;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            Input.UpMove = new Keys[] { Keys.Up, Keys.W, Keys.Space };
            Input.DownMove = new Keys[] { Keys.Down, Keys.S };
            Input.LeftMove = new Keys[] { Keys.Left, Keys.A };
            Input.RightMove = new Keys[] { Keys.Right, Keys.D };
            Input.Fight = new Keys[] { Keys.D4, Keys.U };
            Input.Portal = new Keys[] { Keys.D1, Keys.P };

            // Calculate the scaling factor based on the screen dimensions
            // Set the appropriate scaling factor based on the minimum scaling axis
            scalingFactor = Math.Min(GraphicsDevice.Viewport.Width / 800, GraphicsDevice.Viewport.Height / 480) * 1.1f;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            music = Content.Load<Song>("music");

            //powers
            PowerManager.portalTex = Content.Load<Texture2D>("portal");

            //to draw
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //load Level
            ChangeLevel();
        }


        protected override void Update(GameTime gameTime)
        {
            //clica para sair do jogo
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            KeyboardState keyboardState = Keyboard.GetState();

            if (MediaPlayer.State != MediaState.Playing && isFirstMusic == false)
            {
                MediaPlayer.Play(music);
                isFirstMusic = true;
            }

            if (keyboardState.IsKeyDown(Keys.H) && !hKeyPressed)
                ChangeLevel();
            else if (keyboardState.IsKeyUp(Keys.H))
                hKeyPressed = false;

            base.Update(gameTime);
        }

        public void ChangeLevel()
        {
            hKeyPressed = true;
            level++;

            currScore = 0;

            Transition levelTransition = new FadeTransition(GraphicsDevice, Color.White);

            switch (level)
            {
                case -3:
                    screenManager.LoadScreen(new Options(this, spriteBatch), levelTransition);
                    break;
                case -2:
                    screenManager.LoadScreen(new Help(this, spriteBatch), levelTransition);
                    break;
                case -1:
                    screenManager.LoadScreen(new Menu(this, spriteBatch), levelTransition);
                    break;
                case 0:
                    screenManager.LoadScreen(new MenuLevels(this, spriteBatch), levelTransition);
                    break;
                case 1:
                    screenManager.LoadScreen(new Level1(this, spriteBatch), levelTransition);
                    break;
                case 2:
                    screenManager.LoadScreen(new Level2(this, spriteBatch), levelTransition);
                    break;
                case 3:
                    screenManager.LoadScreen(new Level3(this), levelTransition);
                    break;
                case 4:
                    screenManager.LoadScreen(new Level4(this), levelTransition);
                    break;
                case 5:
                    screenManager.LoadScreen(new Level5(this), levelTransition);
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