using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using MyMonogameTest.Sprites;
using MyMonogameTest.Sprites.World;
using System.Collections.Generic;
using System.Security.AccessControl;

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

            var title = new Sprite(levelMenu);
            title.Rectangle = new Rectangle(game.ScreenWidth / 4, 0, game.ScreenWidth/2, game.ScreenHeight/6);
            title.Position = new Vector2(title.Rectangle.X, title.Rectangle.Y);

            var lvlY = game.ScreenHeight / 1.6f * game.scalingFactor;
            var lvlWidth = (int)(175 * game.scalingFactor);
            var lvlHeight = (int)(50 * game.scalingFactor);

            var firstLvlX = game.ScreenWidth / 30f * game.scalingFactor;

            //all "objects"
            _sprites = new List<Sprite>()
            {
                new Fundo(fundo,game),
                title,
                new Area(level1,game,new Vector2(firstLvlX,lvlY),lvlWidth,lvlHeight,AreaType.buttonLvl1),
                new Area(level2,game,new Vector2(firstLvlX+lvlWidth,game.ScreenHeight / 1.57f * game.scalingFactor),lvlWidth,lvlHeight,AreaType.buttonLvl2),
                new Area(level3,game,new Vector2(firstLvlX+lvlWidth*2,lvlY),lvlWidth,(int)(60 * game.scalingFactor),AreaType.buttonLvl3),
                new Area(level4,game,new Vector2(firstLvlX+lvlWidth*3,game.ScreenHeight / 1.57f * game.scalingFactor),lvlWidth,lvlHeight,AreaType.buttonLvl4),
                new Area(levelBonus,game,new Vector2(title.Position.X, game.ScreenHeight/1.01f-title.Rectangle.Height),title.Rectangle.Width,title.Rectangle.Height,AreaType.buttonLvlBonus)
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
