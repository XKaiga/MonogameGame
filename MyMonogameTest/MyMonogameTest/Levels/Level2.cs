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
        private Matrix _viewMatrix;

        private new Game1 game;
        private Player _Player;

        public Texture2D fundoEarth;
        public Texture2D fundoEarth2;
        public Texture2D playerTexStart;
        public Texture2D enemyTexStart;
        public Texture2D plataforma;
        public Texture2D coracao;

        private SpriteFont spriteFont;

        private SpriteBatch spriteBatch;

        private List<Sprite> _sprites;

        public Level2(Game1 game, SpriteBatch spriteBatch) : base(game)
        {
            this.game = game;
            this.game.totalScore = 0;
            this.spriteBatch = spriteBatch;
        }

        public override void LoadContent()
        {

            LoadStartContent();


            fundoEarth = Content.Load<Texture2D>("earth_background");
            fundoEarth2 = Content.Load<Texture2D>("earth_background2");
            plataforma = Content.Load<Texture2D>("earth_platform");
            coracao = Content.Load<Texture2D>("10");

            LoadSprites();

            foreach (var sprite in _sprites)
                sprite.LoadContent();
            base.LoadContent();
        }

        private void LoadStartContent()
        {
            //create a texture to start the player
            playerTexStart = Content.Load<Texture2D>("parado_1");
            enemyTexStart = Content.Load<Texture2D>("parado_2");

            spriteFont = Content.Load<SpriteFont>("File");
        }

        private void LoadSprites()
        {
            _Player = new Player(playerTexStart, game);

            //all "objects"
            _sprites = new List<Sprite>()
            {
                //create Player
                new Fundo(fundoEarth, game),
                _Player
            };
        }


        public override void Update(GameTime gameTime)
        {
            CalculateTranslation();

            List<Sprite> spritesToRemove = new List<Sprite>();
            List<Sprite> spritesToAdd = new List<Sprite>();
            foreach (var sprite in _sprites)
            {
                //update all sprites
                sprite.Update(gameTime, _sprites, spritesToAdd);

                if (sprite is Player player)
                    player.Move(gameTime, spritesToAdd);

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

        private void CalculateTranslation()
        {
            var dx = (game.ScreenWidth / 2) - ((_Player.Position.X + _Player.Origin.X) * game.ZoomLevel);
            var dy = (game.ScreenHeight / 2) - ((_Player.Position.Y + _Player.Origin.Y) * game.ZoomLevel);
            dx = MathHelper.Clamp(dx, -(game.ScreenWidth * game.ZoomLevel - game.ScreenWidth), 0);
            dy = MathHelper.Clamp(dy, -(game.ScreenHeight * game.ZoomLevel - game.ScreenHeight), 0);
            var _translation = Matrix.CreateTranslation(dx, dy, 0f);

            var zoomMatrix = Matrix.CreateScale(game.ZoomLevel);
            _viewMatrix = zoomMatrix * _translation;
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(transformMatrix: _viewMatrix);

            foreach (var sprite in _sprites)
            {
                sprite.Draw(spriteBatch);
                if (sprite is Player player)
                {
                    // Apply the inverse transformation to the position
                    Vector2 healthPosition = Vector2.Transform(new Vector2(10, 10), Matrix.Invert(_viewMatrix));
                    Vector2 scorePosition = Vector2.Transform(new Vector2(10, 30), Matrix.Invert(_viewMatrix));
                    Vector2 testPosition = Vector2.Transform(new Vector2(10, 50), Matrix.Invert(_viewMatrix));

                    spriteBatch.DrawString(spriteFont, "   Vidas: " + player.Health.ToString(), healthPosition, Color.Black);
                    spriteBatch.DrawString(spriteFont, "   " + game.totalScore + " / 10 Pontos", scorePosition, Color.Black);
                    spriteBatch.DrawString(spriteFont, player.Imune.ToString(), testPosition, Color.Black);
                }
                else if (sprite is Enemy enemy)
                {
                    spriteBatch.DrawString(spriteFont, enemy.a.ToString(), new Vector2(200, 200), Color.Black);
                }
                else if (sprite is Area)
                {
                    spriteBatch.DrawString(spriteFont, "abc", new Vector2(200, 300), Color.Black);
                }
            }

            spriteBatch.End();
        }
    }
}

