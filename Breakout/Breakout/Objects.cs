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
    /// collision types, from left, right, up, down or no collision
    /// </summary> 
    public enum Collision {NO, LEFT, RIGHT, UP, DOWN};

    /// <summary>
    /// base class for ball, paddle, brick
    /// </summary> 
    public class Objects
    {
        public Vector2 position;
        public Vector2 size;
        public Vector2 speed = new Vector2(0F,0F);

        
        // allowed distance error when do collision detection
        public static float err = GlobalData.err; 

        public Objects(Vector2 position, Vector2 size, Vector2 speed)
        {
            this.position = position;
            this.size = size;
            this.speed = speed;
        }

        public Objects(Vector2 position, Vector2 size)
        {
            this.position = position;
            this.size = size;
        }

        // return the collision type of obj with objOther, here LEFT means
        // objOther is to the left of obj, etc.
        public virtual Collision CollideType(Objects objOther) 
        {
            return Collision.NO;
        }

        // the methods will be overridden in the derived classes
        public virtual void Load(ContentManager content)
        {
        }

        // the methods will be overridden in the derived classes
        public virtual void Update(GameTime gameTime)
        { 
        }

        // the methods will be overridden in the derived classes
        public virtual void Draw(SpriteBatch spriteBatch) 
        {
        }
    }
}
