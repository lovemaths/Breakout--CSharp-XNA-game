using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Breakout
{
    public static class GameMap
    {
        public static int[,] gameMap =
        {
            {1,1,1,1,0,1,0,0,0,1,0,1,1,1,1},
            {1,0,0,1,0,1,1,0,1,1,0,1,0,0,0},
            {1,1,1,0,0,1,0,1,0,1,0,0,1,1,0},
            {1,0,1,0,0,1,0,0,0,1,0,0,0,0,1},
            {1,0,0,1,0,1,0,0,0,1,0,1,1,1,1}
        };
        public static int bricksLeft = 38;
    }    
}

