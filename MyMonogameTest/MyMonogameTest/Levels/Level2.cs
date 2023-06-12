using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using MyMonogameTest.Sprites;
using MyMonogameTest.Sprites.World;
using System.Collections.Generic;

namespace MyMonogameTest.Levels
{
    public class Level2 : GameScreen
    {
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

        private bool lvlUnlock = false;

        public Level2(Game1 game, SpriteBatch spriteBatch) : base(game)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;

            game.totalScore = 7;

            game.usingMouseMovement = false;
        }

        public override void LoadContent()
        {
            LoadStartContent();

            LoadSprites();

            foreach (var sprite in _sprites)
                sprite.LoadContent();
            base.LoadContent();
        }

        private void LoadStartContent()
        {
            //text
            spriteFont = Content.Load<SpriteFont>("File");

            //background
            fundoEarth = Content.Load<Texture2D>("earth_background");
            fundoEarth2 = Content.Load<Texture2D>("earth_background2");

            //create a texture to start the player and enemies
            playerTexStart = Content.Load<Texture2D>("parado_1");
            enemyTexStart = Content.Load<Texture2D>("parado_2");

            //world
            plataforma = Content.Load<Texture2D>("earth_platform");
            coracao = Content.Load<Texture2D>("23");
        }

        private void LoadSprites()
        {
            _Player = new Player(playerTexStart, game, new Vector2(100, game.ScreenHeight - playerTexStart.Height));

            Vector2 platSize = new Vector2(150, 70);

            //all "objects"
            _sprites = new List<Sprite>()
            {
                //create background
                new Fundo(fundoEarth, game),
                //plataforms from top to bottom
                new Plataform(plataforma, game, new Vector2(game.ScreenWidth/2.2f-platSize.X/2, game.ScreenHeight/6.4f-platSize.Y/4),platSize),
                new Plataform(plataforma, game, new Vector2(game.ScreenWidth/1.5f+platSize.X/2, game.ScreenHeight/4.5f-platSize.Y/4),platSize),
                new Plataform(plataforma, game, new Vector2(game.ScreenWidth/9-platSize.X/2, game.ScreenHeight/4.2f-platSize.Y/4),platSize),
                new Plataform(plataforma, game, new Vector2(game.ScreenWidth/3.5f-platSize.X/2, game.ScreenHeight/2-platSize.Y/4),platSize),
                new Plataform(plataforma, game, new Vector2(game.ScreenWidth/1.5f+platSize.X/2, game.ScreenHeight/1.9f-platSize.Y/4),platSize),
                new Plataform(plataforma, game, new Vector2(game.ScreenWidth/2-platSize.X/2, game.ScreenHeight/1.3f-platSize.Y/4),platSize),
                new Plataform(plataforma, game, new Vector2(0, game.ScreenHeight/1.2f-platSize.Y/4),platSize),
                //create enemy
                new Enemy(enemyTexStart, game, new Vector2(game.ScreenWidth/2, game.ScreenHeight/2),1,enemyType.follow, new Vector2(enemyTexStart.Width, enemyTexStart.Height)),
                //create Player
                _Player
            };

            var hearts = new List<Sprite>();
            foreach (var pltf in _sprites)
                if (pltf is Plataform)
                    hearts.Add(new Area(coracao, game, new Vector2(pltf.Position.X + pltf.Rectangle.Width / 2f - _Player.Rectangle.Width / 2f / 2f * game.scalingFactor, pltf.Position.Y - _Player.Rectangle.Height / 2f * game.scalingFactor), (int)(_Player.Rectangle.Width / 2f * game.scalingFactor), (int)(_Player.Rectangle.Height / 2f * game.scalingFactor), AreaType.collectible));

            _sprites.AddRange(hearts);
        }


        public override void Update(GameTime gameTime)
        {
            lvlUnlock = game.currScore == game.totalScore;

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
            game._viewMatrix = zoomMatrix * _translation;
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(transformMatrix: game._viewMatrix);

            foreach (var sprite in _sprites)
            {
                sprite.Draw(spriteBatch);
                if (sprite is Player player)
                {
                    // Apply the inverse transformation to the position
                    Vector2 healthPosition = Vector2.Transform(new Vector2(10, 10), Matrix.Invert(game._viewMatrix));
                    Vector2 scorePosition = Vector2.Transform(new Vector2(10, 40), Matrix.Invert(game._viewMatrix));
                    Vector2 testPosition = Vector2.Transform(new Vector2(10, 70), Matrix.Invert(game._viewMatrix));

                    spriteBatch.DrawString(spriteFont, "   Vidas: " + player.Health.ToString(), healthPosition, Color.Black);
                    spriteBatch.DrawString(spriteFont, $"    {game.currScore} / {game.totalScore} Pontos", scorePosition, Color.Black);
                    spriteBatch.DrawString(spriteFont, $"{lvlUnlock}", testPosition, Color.Black);
                }
            }

            spriteBatch.End();
        }
    }
}

