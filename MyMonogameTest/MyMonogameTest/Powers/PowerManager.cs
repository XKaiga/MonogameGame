using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Xna.Framework;
using MyMonogameTest.Sprites;
using MyMonogameTest.Models;
using System.Reflection.Metadata;
using MonoGame.Extended.Timers;
using System.Net.NetworkInformation;
using System.Net;

namespace MyMonogameTest.Powers
{
    static class PowerManager
    {
        #region portal
        static public Texture2D portalTex;

        static public bool portalUnlocked = false;

        // Define a TimeSpan representing the minimum time between portals
        private static TimeSpan timeBetweenPortals = TimeSpan.FromSeconds(10);
        // Define a TimeSpan variable to keep track of the time since the last portal
        private static TimeSpan timeSinceLastPortal = TimeSpan.FromSeconds(10);

        #endregion


        static public bool earthUnlocked = false;

        static public void portal(GameTime gameTime, Game1 game, Player player, List<Sprite> sprites, List<Sprite> spritesToAdd)
        {
            if (portalUnlocked && !game.usingMouseMovement)
            {
                MouseState mouseState = Mouse.GetState();
                if (timeSinceLastPortal >= timeBetweenPortals)
                {
                    foreach (var sprite in sprites)
                        if (sprite.Type == SpriteType.Portal)
                            sprite.IsRemoved = true;

                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        Sprite portal = new Sprite(portalTex);
                        portal.Type = SpriteType.Portal;
                        portal.Position = player.Position;



                        var scaleX = portal.Rectangle.Width / player.Rectangle.Width / 2;
                        var scaleY = portal.Rectangle.Height / player.Rectangle.Height / 2;
                        portal.Rectangle = new Rectangle((int)portal.Position.X, (int)portal.Position.Y, player.Rectangle.Width / scaleX, portal.Rectangle.Height / scaleY);
                        spritesToAdd.Add(portal);

                        //is in a static animation
                        player.inStaticAnimation = true;
                        player.ChangeAnimationState(AnimationState.Portal);

                        //teleport player
                        player.Position = Vector2.Transform(new Vector2(mouseState.Position.X - player.Origin.X, mouseState.Position.Y - player.Origin.Y), Matrix.Invert(game._viewMatrix));

                        // Reset the time since the last portal
                        timeSinceLastPortal = TimeSpan.Zero;
                    }
                }
                if (timeSinceLastPortal < timeBetweenPortals)
                    timeSinceLastPortal += gameTime.ElapsedGameTime; //Increment the time since the last portal
            }
        }
    }
}
