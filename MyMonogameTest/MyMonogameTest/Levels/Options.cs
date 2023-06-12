using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using MyMonogameTest.Sprites;
using MyMonogameTest.Sprites.World;
using System.Collections.Generic;


namespace MyMonogameTest.Levels
{
    public class Options : GameScreen
    {
        private Game1 game;
        private SpriteBatch spriteBatch;
        public Texture2D fundo;
        public Texture2D button;
        private List<Sprite> _sprites;

        public Options(Game1 game, SpriteBatch spriteBatch) : base(game)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;
        }

        public override void LoadContent()
        {
            fundo = Content.Load<Texture2D>("fundo_espaco");
            button = Content.Load<Texture2D>("voltar");
            _sprites = new List<Sprite>()
            {
                new Fundo(fundo,game),
                new Area(button,game,Vector2.Zero,100,100,AreaType.buttonBack)
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
