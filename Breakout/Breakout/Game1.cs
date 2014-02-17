using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Breakout
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // the objects to load
        Paddle pad;
        Ball ball;
        Texture2D background;
        SpriteFont Arial;

        // wall contains rows many rows, and cols many columns of bricks
        const int rows = GlobalData.rows;
        const int cols = GlobalData.cols;
        Brick[,] Wall;

        // the number of bricks left (number of bricks show up)
        int bricksLeft = GameMap.bricksLeft;

        // limited amount of time to finish the game, in other words, to
        // clear all the existence bricks.
        int timeLeft = GlobalData.maxTime;
        int timer = 0;

        // game state, if this is the first time to run this game
        bool newGame = true;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // set the game window, the window size is the windowSize value from
            // the class GlobalData
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = (int)GlobalData.windowSize.X;
            graphics.PreferredBackBufferHeight = (int)GlobalData.windowSize.Y;
            this.Window.Title = "Breakout Game";
            
            // initialize the ball, pad objects
            ball = new Ball(GlobalData.ballPosition, GlobalData.ballSize, GlobalData.ballSpeed);
            pad = new Paddle(GlobalData.padPosition, GlobalData.padSize, GlobalData.padSpeed);

            // wall contains rows many rows and cols many columns of bricks, but not all of them will
            // appear on the screen.
            Wall = new Brick[rows, cols];
            for(int i=0;i<rows;i++)
                for (int j = 0; j < cols; j++)
                {
                    // set up the position of the bricks in wall
                    Wall[i, j] = new Brick(new Vector2(50 + GlobalData.brickSize.X*j,200 + GlobalData.brickSize.Y*i), 
                        GlobalData.brickSize);
                    // which brick appears depends on the gameMap
                    Wall[i, j].appear = GameMap.gameMap[i, j];
                }
            
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // load the objects and background
            pad.Load(Content);
            ball.Load(Content);
            background = Content.Load<Texture2D>("hurricane");
            Brick.bricks = Content.Load<Texture2D>("bricks");

            // load the font for the text that will show up on the screen.
            Arial = Content.Load<SpriteFont>("Arial");

        }

        protected override void UnloadContent()
        {
           
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // if this is a new game, use the space key to start the game
            if (newGame)
            {
                KeyboardState keystate = Keyboard.GetState();
                if (keystate.IsKeyDown(Keys.Space))
                    newGame = false;
            }

            // if this is not a new game, and there is time left, update the objects
            if (!newGame && timeLeft > 0)
            {
                // collision detection
                // ball with pad
                ball.CollideType(pad);

                // ball with bricks
                foreach (Brick br in Wall)
                {
                    // if the brick doesn't show up, don't need to detect collision with it
                    if (br.appear == 0)
                        continue;
                    else
                    {
                        pad.CollideType(br);
                        // if collision happens
                        if (ball.CollideType(br) != Collision.NO)
                        {
                            // not show the brick from now on
                            br.appear = 0;
                            bricksLeft--;
                            break;
                        }
                    }
                }

                // timer is used to calculate the seconds elapsed, when timer is 1 second
                // the time left will be subtracted 1 and the timer will be reset to 0
                timer += gameTime.ElapsedGameTime.Milliseconds;
                if (timer > 1000)
                {
                    timeLeft--;
                    timer -= 1000;
                }

                // update all the objects
                ball.Update(gameTime);
                pad.Update(gameTime);
                base.Update(gameTime);
            }

            // if no time left or no bricks left, then you are either win or lose the game
            // use the space key to restart the game, and reset the wall of bricks
            if (timeLeft == 0 || bricksLeft == 0)
            {
                KeyboardState keystate = Keyboard.GetState();
                if (keystate.IsKeyDown(Keys.Space))
                    timeLeft = GlobalData.maxTime;

                // reset the wall of bricks and the number of bricks left
                for (int i = 0; i < rows; i++)
                    for (int j = 0; j < cols; j++)
                    {
                        Wall[i, j].appear = GameMap.gameMap[i, j];
                    }
                bricksLeft = GameMap.bricksLeft;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // use spriteBatch to draw the objects
            spriteBatch.Begin();
            // draw the objects here
            spriteBatch.Draw(background, new Vector2(0, 0), Color.White);               
            pad.Draw(spriteBatch);
            ball.Draw(spriteBatch);
            // show how much time and how many bricks left on the screen.
            spriteBatch.DrawString(Arial, "Time Left: " + timeLeft, new Vector2(800, 10), Color.White);
            spriteBatch.DrawString(Arial, "Bricks Left: " + bricksLeft, new Vector2(780, 40), Color.White);
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                {
                    // only draw the bricks which will show up
                    if (Wall[i, j].appear == 1)
                        spriteBatch.Draw(Brick.bricks, Wall[i, j].position, Color.White);
                }

            // if this is a new game, show the instruction and welcome screen
            // use space key to start the game
            if (newGame)
            {
                spriteBatch.DrawString(Arial, "Welcome!", new Vector2(250, 50), Color.Gold, 0F,
                    new Vector2(0, 0), 5F, SpriteEffects.None, 0);
                spriteBatch.DrawString(Arial, "Instructions:\n"+"Space: start the game\n" + "Arrow Keys: move the paddle\n"
                    +"Clear all the bricks in the given time.", 
                    new Vector2(200, 550), Color.GreenYellow, 0F,
                    new Vector2(0, 0), 1F, SpriteEffects.None, 0);
            }

            // if no bricks left, then show the game win screen, and use space key to restart the game
            if (bricksLeft == 0)
            {
                spriteBatch.DrawString(Arial, "You won!", new Vector2(250, 50), Color.Gold, 0F,
                    new Vector2(0, 0), 5F, SpriteEffects.None, 0);
                spriteBatch.DrawString(Arial, "Press Space to restart the game.", new Vector2(200, 550), Color.GreenYellow, 0F,
                    new Vector2(0, 0), 2F, SpriteEffects.None, 0);
            }

            // if no time is left, show the game over screen and use space key to restart the game
            if(timeLeft==0) 
            {
                spriteBatch.DrawString(Arial,"Game Over", new Vector2(250, 50), Color.Gold,0F,
                    new Vector2(0,0),5F,SpriteEffects.None,0);
                spriteBatch.DrawString(Arial, "Press Space to restart the game.", new Vector2(200, 550), Color.GreenYellow, 0F,
                    new Vector2(0, 0), 2F, SpriteEffects.None, 0);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
