using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using MyMonogameTest.Models;
using MyMonogameTest.Powers;
using MyMonogameTest.Sprites;
using MyMonogameTest.Sprites.World;
using System;
using System.Collections.Generic;


namespace MyMonogameTest.Levels
{
    public class Level3 : GameScreen
    {
        private new Game1 game;
        private Player _Player;

        public Texture2D fundoEarth;
        public Texture2D fundoEarth2;
        public Texture2D playerTexStart;
        public Texture2D enemyTexStart;
        public Texture2D plataforma;
        public Texture2D coracao;

        public bool playerAlive;

        private SpriteFont spriteFont;

        private SpriteBatch spriteBatch;

        private List<Sprite> _sprites;

        private bool lvlUnlock = false;

        // Define a TimeSpan representing the minimum time between shots
        TimeSpan timeBetweenHearts = TimeSpan.FromSeconds(1);
        // Define a TimeSpan variable to keep track of the time since the last shot was fired
        public TimeSpan timeSinceLastHeart = TimeSpan.Zero;


        public Level3(Game1 game, SpriteBatch spriteBatch) : base(game)
        {
            this.game = game;
            this.spriteBatch = spriteBatch;

            game.totalScore = 10;

            game.ZoomLevel = 1.5f;
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
            fundoEarth = Content.Load<Texture2D>("volcao");
            fundoEarth2 = Content.Load<Texture2D>("lava");

            //create a texture to start the player and enemies
            playerTexStart = Content.Load<Texture2D>("parado_1");
            enemyTexStart = Content.Load<Texture2D>("1");

            //world
            plataforma = Content.Load<Texture2D>("fire_plat");
            coracao = Content.Load<Texture2D>("23");
        }

        private void LoadSprites()
        {
            Vector2 platSize = new Vector2(150, 70);
            
            _Player = new Player(playerTexStart, game, new Vector2(game.ScreenWidth / 2f / 2f, game.ScreenHeight / 1.2f - platSize.Y / 4 - playerTexStart.Height));

            //all "objects"
            _sprites = new List<Sprite>()
            {
                //create background
                new Fundo(fundoEarth, game),
                new Area(fundoEarth2, game,new Vector2(0,game.ScreenHeight-(int)(fundoEarth2.Height/3f*game.scalingFactor)),fundoEarth2.Width, (int)(fundoEarth2.Height/3f*game.scalingFactor),AreaType.lava, 1),
                //plataforms from top to bottom
                new Plataform(plataforma, game, new Vector2(game.ScreenWidth/2f/2f-platSize.X/2, game.ScreenHeight/1.2f-platSize.Y/4),platSize),
                new Plataform(plataforma, game, new Vector2(game.ScreenWidth/2f-platSize.X/2, game.ScreenHeight/1.2f-platSize.Y/4),platSize),
                new Plataform(plataforma, game, new Vector2(game.ScreenWidth/1.2f-platSize.X/2, game.ScreenHeight/1.2f-platSize.Y/4),platSize),
                //create enemy
                new Enemy(enemyTexStart, game, new Vector2(0, 0),1,enemyType.shooter, new Vector2(enemyTexStart.Width, enemyTexStart.Height)),
                new Enemy(enemyTexStart, game, new Vector2(game.ScreenWidth, game.ScreenHeight/2/2),1,enemyType.followShooter, new Vector2(enemyTexStart.Width, enemyTexStart.Height)),
                new Enemy(enemyTexStart, game, new Vector2(game.ScreenWidth, 0),1,enemyType.shooter, new Vector2(enemyTexStart.Width, enemyTexStart.Height)),
                //create Player
                _Player
            };
        }


        public override void Update(GameTime gameTime)
        {
            playerAlive = false;
            lvlUnlock = game.currScore == game.totalScore;
            if (lvlUnlock)
            {
                game.level = -2;
                game.ChangeLevel();
            }

            CalculateTranslation();

            List<Sprite> spritesToRemove = new List<Sprite>();
            List<Sprite> spritesToAdd = new List<Sprite>();


            //spawn heart
            if (timeSinceLastHeart >= timeBetweenHearts)
            {
                var rnd = new Random();
                _sprites.Add(new Area(coracao, game, new Vector2(rnd.Next(game.ScreenWidth), 100), (int)(_Player.Rectangle.Width / 2f * game.scalingFactor), (int)(_Player.Rectangle.Height / 2f * game.scalingFactor), AreaType.collectible));

                timeSinceLastHeart = TimeSpan.Zero;
            }
            else if (timeSinceLastHeart < timeBetweenHearts)
                timeSinceLastHeart += gameTime.ElapsedGameTime; //Increment the time since the last Heart was fired

            
            foreach (var sprite in _sprites)
            {
                //update all sprites
                sprite.Update(gameTime, _sprites, spritesToAdd);

                if (sprite is Player player)
                {
                    playerAlive = true;
                    player.Move(gameTime, spritesToAdd);
                }else if(sprite is Area area && area.type == AreaType.collectible) {
                    area.Position.Y += 300f * game.scalingFactor * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    area.Rectangle = new Rectangle(area.Rectangle.X, (int)area.Position.Y, area.Rectangle.Width, area.Rectangle.Height);
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
                PowerManager.earthUnlocked = true;
                game.level = -1;
                game.ChangeLevel();
            }

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
                    Vector2 healthPosition = Vector2.Transform(new Vector2(10, 40), Matrix.Invert(game._viewMatrix));
                    Vector2 scorePosition = Vector2.Transform(new Vector2(10, 75), Matrix.Invert(game._viewMatrix));

                    spriteBatch.DrawString(spriteFont, "   Vidas: " + player.Health.ToString(), healthPosition, Color.Black);
                    spriteBatch.DrawString(spriteFont, $"    {game.currScore} / {game.totalScore} Pontos", scorePosition, Color.Black);
                }
            }

            spriteBatch.End();
        }
    }
}

