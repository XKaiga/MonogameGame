using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;


namespace MyMonogameTest.Levels
{
    public class Level5 : GameScreen
    {
        private new Game1 game;

        public Level5(Game1 game) : base(game)
        {
            this.game = game;
            this.game.totalScore = 0;
        }

        public override void LoadContent()
        {
            base.LoadContent();
        }


        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime)
        {

        }
    }
}

