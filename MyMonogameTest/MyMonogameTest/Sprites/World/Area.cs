﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Xna.Framework;
using MyMonogameTest.Sprites;
using MyMonogameTest.Models;
using System.Reflection.Metadata;
using MyMonogameTest.Sprites.World;
using Microsoft.Xna.Framework.Media;

namespace MyMonogameTest.Sprites.World
{
    enum AreaType
    {
        nextLevel,
        collectible,
        buttonPlay,
        buttonExit,
        buttonHelp,
        buttonOptions,
        buttonBack,
        buttonLvl1,
        buttonLvl2,
        buttonLvl3,
        buttonLvl4,
        buttonLvlBonus,
        buttonBarra,
        bola,
        lava
    }

    class Area : Sprite
    {
        private Game1 game;

        public AreaType type;

        private int damage;

        public Area(Texture2D texture, Game1 game, Vector2 position, int width, int height, AreaType type, int damage = 0) : base(texture)
        {
            Position = position;
            this.type = type;
            this.game = game;

            this.damage = damage;

            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, width, height);
        }

        public override void Update(GameTime gameTime, List<Sprite> sprites, List<Sprite> spritesToAdd)
        {
            foreach (var sprite in sprites)
            {
                if (sprite is Area)
                    continue;

                // Colission
                if (sprite is Player player && player.Rectangle.Intersects(this.Rectangle))
                {
                    switch (type)
                    {
                        case AreaType.nextLevel:
                            game.ChangeLevel();
                            break;
                        case AreaType.collectible:
                            this.IsRemoved = true;
                            game.currScore++;
                            break;
                        case AreaType.lava:
                            player.Health -= damage;
                            break;
                    }
                }

                //buttons
                if (Mouse.GetState().LeftButton is ButtonState.Pressed && this.Rectangle.Contains(Mouse.GetState().Position))
                {
                    switch (type)
                    {
                        case AreaType.buttonPlay:
                            game.level = -1;
                            game.ChangeLevel();
                            break;
                        case AreaType.buttonExit:
                            game.Exit();
                            break;
                        case AreaType.buttonHelp:
                            game.level = -3;
                            game.ChangeLevel();
                            break;
                        case AreaType.buttonOptions:
                            game.level = -4;
                            game.ChangeLevel();
                            break;
                        case AreaType.buttonBack:
                            game.level = -2;
                            game.ChangeLevel();
                            break;

                        case AreaType.buttonLvl1:
                            game.level = 0;
                            game.ChangeLevel();
                            break;
                        case AreaType.buttonLvl2:
                            game.level = 1;
                            game.ChangeLevel();
                            break;
                        case AreaType.buttonLvl3:
                            game.level = 2;
                            game.ChangeLevel();
                            break;
                        case AreaType.buttonLvl4:
                            game.level = 3;
                            game.ChangeLevel();
                            break;
                        case AreaType.buttonLvlBonus:
                            game.level = 4;
                            game.ChangeLevel();
                            break;
                        case AreaType.buttonBarra:
                            foreach (var spr in sprites)
                            {
                                if (spr is Area bola && bola.type == AreaType.bola)
                                {
                                    bola.Rectangle = new Rectangle((int)(Mouse.GetState().Position.X - bola.Rectangle.Width / 2), bola.Rectangle.Y, bola.Rectangle.Width, bola.Rectangle.Height);
                                    MediaPlayer.Volume = (float)((bola.Rectangle.X - game.ScreenWidth / 4f) / 700f);
                                    break;
                                }
                            }
                            break;
                        case AreaType.bola:
                            continue;
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Color color = type == AreaType.nextLevel ? Color.White * 0.5f : Color.White;
            spriteBatch.Draw(_texture, Rectangle, color);
        }
    }
}