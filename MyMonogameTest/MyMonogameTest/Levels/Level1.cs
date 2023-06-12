using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using MyMonogameTest.Powers;
using MyMonogameTest.Sprites;
using MyMonogameTest.Sprites.World;
using System.Collections.Generic;

namespace MyMonogameTest.Levels
{
    class Level1 : GameScreen
    {
        private new Game1 game;

        private bool playerAlive = true;

        private bool showPowers = false;

        public Texture2D playerTexStart;
        public Texture2D plataformTex;
        public Texture2D fundoWater;
        public Texture2D mina;
        public Texture2D coracao;

        private SpriteBatch spriteBatch;
        private SpriteFont spriteFont;

        private List<Sprite> _sprites;

        public Level1(Game1 game, SpriteBatch spriteBatch) : base(game)
        {
            this.game = game;
            this.game.totalScore = 5;
            this.spriteBatch = spriteBatch;
            game.usingMouseMovement = true;
        }

        public override void LoadContent()
        {

            LoadStartContent();


            fundoWater = Content.Load<Texture2D>("water_fall");

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

            mina = Content.Load<Texture2D>("mina");

            coracao = Content.Load<Texture2D>("23");
        }

        private void LoadSprites()
        {
            var player = new Player(playerTexStart, game, new Vector2(0, game.ScreenHeight / 2));

            int heartSize = (int)(player.Rectangle.Width / 2f * game.scalingFactor);

            //all "objects"
            _sprites = new List<Sprite>()
            {
                new Fundo(fundoWater, game),

                //minas
                new Enemy(mina, game, new Vector2(game.ScreenWidth-player.Rectangle.Width*1.5f, game.ScreenHeight/2.75f), 1, enemyType.bomb, new Vector2(player.Rectangle.Width*1.5f,player.Rectangle.Height*1.5f)),
                new Enemy(mina, game, new Vector2(game.ScreenWidth*0.8f, game.ScreenHeight/2.5f), 1, enemyType.bomb, new Vector2(player.Rectangle.Width*1.5f,player.Rectangle.Height*1.5f)),
                new Enemy(mina, game, new Vector2(game.ScreenWidth*0.70f, game.ScreenHeight/2f), 1, enemyType.bomb, new Vector2(player.Rectangle.Width*1.5f,player.Rectangle.Height*1.5f)),
                new Enemy(mina, game, new Vector2(game.ScreenWidth*0.70f, game.ScreenHeight/1.5f), 1, enemyType.bomb, new Vector2(player.Rectangle.Width*1.5f,player.Rectangle.Height*1.5f)),
                new Enemy(mina, game, new Vector2(game.ScreenWidth*0.45f, game.ScreenHeight-player.Rectangle.Height*1.5f), 1, enemyType.bomb, new Vector2(player.Rectangle.Width*1.5f,player.Rectangle.Height*1.5f)),
                new Enemy(mina, game, new Vector2(game.ScreenWidth*0.45f, game.ScreenHeight/3.5f), 1, enemyType.bomb, new Vector2(player.Rectangle.Width*1.5f,player.Rectangle.Height*1.5f)),
                new Enemy(mina, game, new Vector2(game.ScreenWidth*0.37f, game.ScreenHeight/1.33f), 1, enemyType.bomb, new Vector2(player.Rectangle.Width*1.5f,player.Rectangle.Height*1.5f)),
                new Enemy(mina, game, new Vector2(game.ScreenWidth*0.3f, game.ScreenHeight/1.5f), 1, enemyType.bomb, new Vector2(player.Rectangle.Width*1.5f,player.Rectangle.Height*1.5f)),
                new Enemy(mina, game, new Vector2(game.ScreenWidth*0.15f, game.ScreenHeight/3.5f), 1, enemyType.bomb, new Vector2(player.Rectangle.Width*1.5f,player.Rectangle.Height*1.5f)),
                new Enemy(mina, game, new Vector2(0, game.ScreenHeight/1.5f), 1, enemyType.bomb, new Vector2(player.Rectangle.Width*1.5f,player.Rectangle.Height*1.5f)),

                new Area(coracao, game, new Vector2(game.ScreenWidth-heartSize, game.ScreenHeight/4f),heartSize,heartSize,AreaType.collectible),
                new Area(coracao, game, new Vector2(game.ScreenWidth-player.Rectangle.Width*1.5f, game.ScreenHeight/1.75f),heartSize,heartSize,AreaType.collectible),
                new Area(coracao, game, new Vector2(game.ScreenWidth*0.45f, game.ScreenHeight/1.4f),heartSize,heartSize,AreaType.collectible),
                new Area(coracao, game, new Vector2(game.ScreenWidth*0.35f, game.ScreenHeight/3.5f),heartSize,heartSize,AreaType.collectible),
                new Area(coracao, game, new Vector2(0, game.ScreenHeight - heartSize),heartSize,heartSize,AreaType.collectible),
                //create Player
                player
            };
        }

        public override void Update(GameTime gameTime)
        {
            playerAlive = false;

            List<Sprite> spritesToRemove = new List<Sprite>();
            List<Sprite> spritesToAdd = new List<Sprite>();
            foreach (var sprite in _sprites)
            {
                //update all sprites
                sprite.Update(gameTime, _sprites, spritesToAdd);

                if (sprite is Player player)
                {
                    playerAlive = true;
                    player.MouseMove(gameTime, spritesToAdd);
                    if (player.Position.Y < 100 * (game.scalingFactor + (0.4f * game.scalingFactor - 0.4f)))
                        player.Position.Y = 100 * (game.scalingFactor + (0.4f * game.scalingFactor - 0.4f));
                }

                //get sprites to be removed
                if (sprite.IsRemoved)
                    spritesToRemove.Add(sprite);
            }

            //remove "dead" sprites
            foreach (Sprite spr in spritesToRemove)
                _sprites.Remove(spr);

            //add new sprites
            _sprites.AddRange(spritesToAdd);

            if (!playerAlive)
            {
                game.level = -1;
                game.ChangeLevel();
            }

            if (game.currScore == game.totalScore)
            {
                showPowers = true;
                PowerManager.portalUnlocked = true;
                //_sprites.Add()
            }
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            foreach (var sprite in _sprites)
                sprite.Draw(spriteBatch);

            foreach (var spr in _sprites)
                if (spr is Player player)
                {
                    spriteBatch.DrawString(spriteFont, "" + player.Health, new Vector2(10, 10), Color.Black);
                    spriteBatch.DrawString(spriteFont, $"   {game.currScore} / {game.totalScore} Pontos", new Vector2(10, 30), Color.Black);
                }
            spriteBatch.End();
        }
    }
}
