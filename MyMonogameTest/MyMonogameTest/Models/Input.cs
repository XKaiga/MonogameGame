using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MyMonogameTest.Models
{
    public static class Input
    {
        public static Keys[] UpMove;
        public static Keys[] DownMove;
        public static Keys[] LeftMove;
        public static Keys[] RightMove;
        public static Keys[] Fight;
        public static Keys[] Portal;

        public static bool KeyPressed(Keys key, KeyboardState previousKey, KeyboardState currentKey)
        {
            switch (key)
            {
                case Keys.Up:
                    for (int i = 0; i < Input.UpMove.Length; i++)
                        if (Keyboard.GetState().IsKeyDown(Input.UpMove[i]))
                            return true;
                    break;
                case Keys.Down:
                    for (int i = 0; i < Input.DownMove.Length; i++)
                        if (Keyboard.GetState().IsKeyDown(Input.DownMove[i]))
                            return true;
                    break;
                case Keys.Left:
                    for (int i = 0; i < Input.LeftMove.Length; i++)
                        if (Keyboard.GetState().IsKeyDown(Input.LeftMove[i]))
                            return true;
                    break;
                case Keys.Right:
                    for (int i = 0; i < Input.RightMove.Length; i++)
                        if (Keyboard.GetState().IsKeyDown(Input.RightMove[i]))
                            return true;
                    break;
                case Keys.F:
                    for (int i = 0; i < Input.Fight.Length; i++)
                        if (previousKey.IsKeyDown(Input.Fight[i]) && currentKey.IsKeyUp(Input.Fight[i]))
                            return true;
                    break;
            }
            return false;
        }
    }
}
