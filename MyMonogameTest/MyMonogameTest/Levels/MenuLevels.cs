using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using MyMonogameTest.Sprites;
using MyMonogameTest.Sprites.World;
using System.Collections.Generic;

namespace MyMonogameTest.Levels
{
    public class MenuLevels : GameScreen
    {
        private Game1 game;
        private SpriteBatch spriteBatch;
        public Texture2D fundo;
        public Texture2D levelMenu;
        public Texture2D level1;
        public Texture2D level2;
        public Texture2D level3;
        public Texture2D level4;
        public Texture2D levelBonus;
        private List<Sprite> _sprites;   

        public MenuLevels(Game1 game, SpriteBatch spriteBatch) : base(game)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;
        }

        private new Game1 Game => (Game1)base.Game;
        public override void LoadContent()
        {
            fundo = Content.Load<Texture2D>("fundo_espaco");
            levelMenu = Content.Load<Texture2D>("13");
            level1 = Content.Load<Texture2D>("14");
            level2 = Content.Load<Texture2D>("15");
            level3 = Content.Load<Texture2D>("16");
            level4 = Content.Load<Texture2D>("17");
            levelBonus = Content.Load<Texture2D>("18");
            //all "objects"
            _sprites = new List<Sprite>()
            {
                new Fundo(fundo,game),
                new Sprite(levelMenu),
                new Sprite(level1),
                new Sprite(level2),
                new Sprite(level3),
                new Sprite(level4), 
                new Sprite(levelBonus)
            };
            foreach (var sprite in _sprites)
                sprite.LoadContent();
            base.LoadContent();
        }


        public override void Update(GameTime gameTime)
        {

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
