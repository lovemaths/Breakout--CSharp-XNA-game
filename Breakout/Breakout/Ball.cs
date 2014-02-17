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
    public class Ball:Objects
    {
        public static Texture2D ball;
        public bool moveUp = false;
        public bool moveLeft = false;
        public static float gravity = GlobalData.gravity;

        // if the ball changes direction horizontally or vertically
        public bool verticalMoveChange = false;
        public bool horizontalMoveChange = false;

        public Ball(Vector2 position, Vector2 size, Vector2 speed):base(position, size, speed)
        {
 
        }

        public override void Load(ContentManager content)
        {
            ball = content.Load<Texture2D>("ball");
        }

        public override void Update(GameTime gameTime)
        {
            // if the ball reaches the top it can reach, then it move down.
            if (speed.Y <= 0)
            {
                moveUp = false;
                speed.Y = 0;
            }

            // if reach the boundary of the window, change direction
            if (position.X < err)
                moveLeft = false;
            if (position.X + size.X + err > GlobalData.windowSize.X)
                moveLeft = true;
            if (position.Y < err)
                moveUp = false;
            if (position.Y + size.Y + err > GlobalData.windowSize.Y)
                moveUp = true;

            // update position
            if (moveLeft)
                position.X = position.X - speed.X;
            else
                position.X = position.X + speed.X;
            if (moveUp)
                position.Y = position.Y - speed.Y;
            else
                position.Y = position.Y + speed.Y;

            // update speed
            if (moveUp)
                speed.Y = speed.Y - gravity;
            else
                speed.Y = speed.Y + gravity;

            // after the above runs, already changed the direction, so reset 
            // the indicator for vertical and horizontal directions change
            verticalMoveChange = false;
            horizontalMoveChange = false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ball, position, Color.White);
        }

        public override Collision CollideType(Objects objOther)
        {
            // the left/right/top/bottom sides of the object and objOther, use these data to 
            // detect collision
            float xLow = this.position.X;
            float xHigh = this.position.X + size.X;
            float yLow = this.position.Y;
            float yHigh = this.position.Y + size.Y;
            float xlLow = objOther.position.X;
            float xlHigh = objOther.position.X + objOther.size.X;
            float ylLow = objOther.position.Y;
            float ylHigh = objOther.position.Y + objOther.size.Y;

            // res records if collision happens
            Collision res = Collision.NO;

            // left collision
            if (!horizontalMoveChange && moveLeft && xLow - xlHigh >= -err && xLow - xlHigh <= 0
                && (ylLow - yHigh <= -err && yLow - ylHigh <= -err))
            {
                moveLeft = !moveLeft;
                horizontalMoveChange = true;
                res = Collision.UP;
            }

            // right collision
            if (!horizontalMoveChange && !moveLeft && xlLow - xHigh >= -err && xlLow - xHigh <= 0
                && (ylLow - yHigh <= -err && yLow - ylHigh <= -err))
            {
                moveLeft = !moveLeft;
                horizontalMoveChange = true;
                res = Collision.UP;
            }

            // top collision
            if (!verticalMoveChange && moveUp && (xLow - xlHigh <= -err && xlLow - xHigh <= -err)
                && yLow - ylHigh >= -err && yLow - ylHigh <= 0)
            {
                moveUp = !moveUp;
                verticalMoveChange = true;
                res = Collision.UP;
            }

            // down collision
            if (!verticalMoveChange && !moveUp && (xLow - xlHigh <= -err && xlLow - xHigh <= -err)
                && ylLow - yHigh >= -err && ylLow - yHigh <= 0)
            {
                moveUp = !moveUp;
                verticalMoveChange = true;
                res = Collision.UP;
            }

            return res;
        }
    }
}

