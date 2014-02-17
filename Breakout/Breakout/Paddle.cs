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
    /// <summary>
    /// Paddle class
    /// </summary>
    public class Paddle:Objects
    {
        public static Texture2D pad;

        // the direction where pad is collide with other objects
        public Collision collideDirectionPad = Collision.NO;

        // can the pad go up, down, left or right
        public bool upValid = true;
        public bool downValid = true;
        public bool leftValid = true;
        public bool rightValid = true;

        public Paddle(Vector2 position, Vector2 size, Vector2 speed):base(position, size, speed)
        {            
        }

        public override void Load(ContentManager content)
        {
            pad = content.Load<Texture2D>("paddle");
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keystate = Keyboard.GetState();

            // update the position
            // use the arrow keys to decide which direction to go, but need to check if 
            // that direction is a legal move
            if (keystate.IsKeyDown(Keys.Up) && upValid)
                position.Y = position.Y - speed.Y;
            if (keystate.IsKeyDown(Keys.Down) && downValid)
                position.Y = position.Y + speed.Y;
            if (keystate.IsKeyDown(Keys.Left) && leftValid)
                position.X = position.X - speed.Y;
            if (keystate.IsKeyDown(Keys.Right) && rightValid)
                position.X = position.X + speed.Y;

            // check the boundary, make sure the pad won't move out of the screen
            float windowWidth = GlobalData.windowSize.X;
            float windowHeight = GlobalData.windowSize.Y;
            if (position.X < 0)
                position.X = 0;
            if (position.X > windowWidth - size.X)
                position.X = windowWidth - size.X;
            if (position.Y < 0)
                position.Y = 0;
            if (position.Y > windowHeight - size.Y)
                position.Y = windowHeight - size.Y;

            // after the move, reset the valid directions to move
            upValid = true;
            downValid = true;
            leftValid = true;
            rightValid = true;
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(pad, position, Color.White);
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="objOther">Another object to check if collide happens</param>
        /// <returns> return the type of collision occurs</returns>
        public override Collision CollideType(Objects objOther)
        {
            // the left/right/top/bottom of the two objects, use them to detect
            // collision
            float xLow = this.position.X;
            float xHigh = this.position.X + size.X;
            float yLow = this.position.Y;
            float yHigh = this.position.Y + size.Y;
            float xlLow = objOther.position.X;
            float xlHigh = objOther.position.X + objOther.size.X;
            float ylLow = objOther.position.Y;
            float ylHigh = objOther.position.Y + objOther.size.Y;

            // if collision happens, res = UP (just use UP to indict no collision, 
            // doesn't matter if choose something else),
            // otherwise, res = NO.
            Collision res = Collision.NO;

            // check left collision
            if ( xLow - xlHigh >= -err && xLow - xlHigh <= 0
                && (ylLow - yHigh <= -err && yLow - ylHigh <= -err))
            {
                leftValid = false;
                res = Collision.UP;
            }

            // check right collision
            if ( xlLow - xHigh >= -err && xlLow - xHigh <= 0
                && (ylLow - yHigh <= -err && yLow - ylHigh <= -err))
            {
                rightValid = false;
                res = Collision.UP;
            }

            // check top collision
            if ( (xLow - xlHigh <= -err && xlLow - xHigh <= -err)
                && yLow - ylHigh >= -err && yLow - ylHigh <= 0)
            {
                upValid = false;
                res = Collision.UP;
            }

            // just bottom collision
            if ( (xLow - xlHigh <= -err && xlLow - xHigh <= -err)
                && ylLow - yHigh >= -err && ylLow - yHigh <= 0)
            {
                downValid = false;
                res = Collision.UP;
            }

            return res;
        }
    }
}
