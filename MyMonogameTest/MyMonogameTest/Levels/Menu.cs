using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using MyMonogameTest.Sprites;
using MyMonogameTest.Sprites.World;
using System.Collections.Generic;

namespace MyMonogameTest.Levels
{
    public class Menu : GameScreen
    {
        public Game1 game;
        public SpriteBatch spriteBatch;
        public Texture2D fundo;
        private List<Sprite> _sprites;
        public Texture2D play;
        public Texture2D exit;
        public Texture2D options;
        public Texture2D help;
        private new Game1 Game => (Game1)base.Game;

        public Menu(Game1 game, SpriteBatch spriteBatch) : base(game)
        {
            this.game = game;
            this.spriteBatch=spriteBatch;
        }

        public override void LoadContent()
        {
            fundo = Content.Load<Texture2D>("4");
            play = Content.Load<Texture2D>("7");
            exit= Content.Load <Texture2D>("9");
            help = Content.Load<Texture2D>("10");
            options = Content.Load<Texture2D>("8");
            //all "objects"
            _sprites = new List<Sprite>()
            {
                new Fundo(fundo,game),
                new Area(play,game,new Vector2(game.ScreenWidth/1.5f,game.ScreenHeight/3f),(int)(350*game.scalingFactor),(int)(100*game.scalingFactor),AreaType.buttonPlay),
                new Area(exit,game,new Vector2(0,game.ScreenHeight/1.7f),(int)(350*game.scalingFactor),(int)(100*game.scalingFactor),AreaType.buttonExit),
                new Area(help, game,new Vector2(game.ScreenWidth/6,game.ScreenHeight/4),(int)(350 * game.scalingFactor), (int)(100 * game.scalingFactor),AreaType.buttonHelp),
                new Area(options, game,new Vector2(game.ScreenWidth/7,game.ScreenHeight/5),(int)(350 * game.scalingFactor), (int)(100 * game.scalingFactor),AreaType.buttonOptions)
            };
            foreach (var sprite in _sprites)
                sprite.LoadContent();

            base.LoadContent();
        }


        public override void Update(GameTime gameTime)
        {
            List<Sprite> spritesToAdd = new List<Sprite>();
            foreach (var sprite in _sprites)
            {
                //update all sprites
                sprite.Update(gameTime, _sprites, spritesToAdd);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            foreach (var sprite in _sprites)
                sprite.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}


