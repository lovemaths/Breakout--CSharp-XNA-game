using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Breakout
{
    public class Brick:Objects
    {
        // if the brick still appears in the game
        public int appear = 1; // 1 for appear and 0 not appear
        public static Texture2D bricks;

        public Brick(Vector2 position, Vector2 size):base(position, size)
        {            
        }
    }
}

