using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;
using MyMonogameTest.Sprites;
using MyMonogameTest.Sprites.World;
using System.Collections.Generic;

namespace MyMonogameTest.Levels
{
    public class Level2 : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;

        public Texture2D fundoEarth;
        public Texture2D fundoEarth2;
        public Texture2D playerTexStart;

        private SpriteFont spriteFont;

        private SpriteBatch spriteBatch;

        private List<Sprite> _sprites;

        public Level2(Game1 game, SpriteBatch spriteBatch) : base(game)
        {
            this.spriteBatch = spriteBatch;
        }

        public override void LoadContent()
        {

            LoadStartContent();


            fundoEarth = Content.Load<Texture2D>("earth_background");
            fundoEarth2 = Content.Load<Texture2D>("earth_background2");

            LoadSprites();

            foreach (var sprite in _sprites)
                sprite.LoadContent();
            base.LoadContent();
        }
        private void LoadStartContent()
        {
            //create a texture to start the player
            playerTexStart = Content.Load<Texture2D>("parado_1");

            spriteFont = Content.Load<SpriteFont>("File");
        }

        private void LoadSprites()
        {
            //all "objects"
            _sprites = new List<Sprite>()
            {
                //create Player
                new Fundo(fundoEarth, Game),
                new Player(playerTexStart, Game)

            };
        }


        public override void Update(GameTime gameTime)
        {
            List<Sprite> spritesToRemove = new List<Sprite>();
            List<Sprite> spritesToAdd = new List<Sprite>();
            foreach (var sprite in _sprites)
            {
                if (sprite is Player player)
                {
                    player.Move(gameTime, spritesToAdd);
                }
                //update all sprites
                sprite.Update(gameTime, _sprites, spritesToAdd);
                //get sprites to be removed
                if (sprite.IsRemoved)
                    spritesToRemove.Add(sprite);
            }

            //remove "dead" sprites
            foreach (Sprite spr in spritesToRemove)
                _sprites.Remove(spr);

            //add new sprites
            _sprites.AddRange(spritesToAdd);
        }
        
        

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            foreach (var sprite in _sprites)
                sprite.Draw(spriteBatch);

            foreach (var spr in _sprites)
                if (spr is Player player)
                {
                    spriteBatch.DrawString(spriteFont, "   Vidas: " + (spr.Health).ToString(), new Vector2(0, 10), Color.Black);
                    spriteBatch.DrawString(spriteFont, "   " + Game.totalScore + " / 10 Pontos", new Vector2(0, 32), Color.Black);
                    //spriteBatch.DrawString(spriteFont, player.GetFacingDirection().ToString(), new Vector2(0, 54), Color.Black);
                }
                else if (spr is Weapon weapon)
                {
                    //spriteBatch.DrawString(spriteFont, weapon.Position.ToString(), new Vector2(0, 54), Color.Black);
                    //spriteBatch.DrawString(spriteFont, "   " + totalScore + " / 10 Pontos", new Vector2(0, 76), Color.Black);
                }
            spriteBatch.End();
        }
    }
}

