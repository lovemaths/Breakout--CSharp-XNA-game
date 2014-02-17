using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Breakout
{
    /// <summary>
    /// all the global data that is used is stored here
    /// </summary>
    public static class GlobalData
    {
        public static Vector2 windowSize {get{return new Vector2(1000,700);}}
        public static Vector2 padSize {get{return new Vector2(184,26);}}
        public static Vector2 padPosition { get{return new Vector2(100,500);} }
        public static Vector2 padSpeed { get { return new Vector2(0, 10); } }
        public static Vector2 brickSize { get { return new Vector2(50, 50); }}
        public static Vector2 ballSize { get { return new Vector2(32, 32); }}
        public static Vector2 ballSpeed { get { return new Vector2(3, 1); }}
        public static Vector2 ballPosition { get { return new Vector2(20, 20); }}

        // how many rows and cols of bricks are there
        public const int rows = 5;
        public const int cols = 15;

        // the maximum time that be played on one game
        public const int maxTime = 30;

        public static float gravity = 0.06F;

        // allowance of distance error when detect collision
        public static float err = 15F;
    }
}

