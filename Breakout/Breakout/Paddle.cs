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
    public class Paddle:Objects
    {
        public static Texture2D pad;
        public Collision collideDirectionPad = Collision.NO;
        public bool upValid = true;
        public bool downValid = true;
        public bool leftValid = true;
        public bool rightValid = true;

        public Paddle(Vector2 position, Vector2 size, Vector2 speed):base(position, size, speed)
        {            
        }

        public override void Load(ContentManager content)
        {
            pad = content.Load<Texture2D>("paddle2");
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keystate = Keyboard.GetState();

            // update the position
            if (keystate.IsKeyDown(Keys.Up) && upValid)
                position.Y = position.Y - speed.Y;
            if (keystate.IsKeyDown(Keys.Down) && downValid)
                position.Y = position.Y + speed.Y;
            if (keystate.IsKeyDown(Keys.Left) && leftValid)
                position.X = position.X - speed.Y;
            if (keystate.IsKeyDown(Keys.Right) && rightValid)
                position.X = position.X + speed.Y;

            // check the boundary
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

            // reset
            upValid = true;
            downValid = true;
            leftValid = true;
            rightValid = true;
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(pad, position, Color.White);
        }


        public override Collision CollideType(Objects objOther)
        {
            float xLow = this.position.X;
            float xHigh = this.position.X + size.X;
            float yLow = this.position.Y;
            float yHigh = this.position.Y + size.Y;
            float xlLow = objOther.position.X;
            float xlHigh = objOther.position.X + objOther.size.X;
            float ylLow = objOther.position.Y;
            float ylHigh = objOther.position.Y + objOther.size.Y;
            Collision res = Collision.NO;


            if ( xLow - xlHigh >= -err && xLow - xlHigh <= 0
                && (ylLow - yHigh <= -err && yLow - ylHigh <= -err))
            {
                leftValid = false;
                res = Collision.UP;
            }

            
            if ( xlLow - xHigh >= -err && xlLow - xHigh <= 0
                && (ylLow - yHigh <= -err && yLow - ylHigh <= -err))
            {
                rightValid = false;
                res = Collision.UP;
            }

            // just up collision
            if ( (xLow - xlHigh <= -err && xlLow - xHigh <= -err)
                && yLow - ylHigh >= -err && yLow - ylHigh <= 0)
            {
                upValid = false;
                res = Collision.UP;
            }

            // just down collision
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
