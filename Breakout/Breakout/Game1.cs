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
        Paddle pad;
        Ball ball;
        Texture2D background;
        Texture2D gameover;
        SpriteFont Arial;
        const int rows = GlobalData.rows;
        const int cols = GlobalData.cols;
        Brick[,] Wall;
        int bricksLeft = GameMap.bricksLeft;

        int timeLeft = GlobalData.maxTime;
        int timer = 0;
        bool newGame = true;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = (int)GlobalData.windowSize.X;
            graphics.PreferredBackBufferHeight = (int)GlobalData.windowSize.Y;
            this.Window.Title = "Breakout Game";
                        
            ball = new Ball(GlobalData.ballPosition, GlobalData.ballSize, GlobalData.ballSpeed);
            pad = new Paddle(GlobalData.padPosition, GlobalData.padSize, GlobalData.padSpeed);
            Wall = new Brick[rows, cols];
            for(int i=0;i<rows;i++)
                for (int j = 0; j < cols; j++)
                {
                    Wall[i, j] = new Brick(new Vector2(50 + GlobalData.brickSize.X*j,200 + GlobalData.brickSize.Y*i), 
                        GlobalData.brickSize);
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

            
            pad.Load(Content);
            ball.Load(Content);
            background = Content.Load<Texture2D>("hurricane");
            gameover = Content.Load<Texture2D>("gameover");
            Brick.bricks = Content.Load<Texture2D>("bricks");

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
            if (newGame)
            {
                KeyboardState keystate = Keyboard.GetState();
                if (keystate.IsKeyDown(Keys.Space))
                    newGame = false;
            }
            if (!newGame && timeLeft > 0)
            {
                // collision detection
                // ball with pad
                ball.CollideType(pad);

                // ball with bricks
                foreach (Brick br in Wall)
                {
                    if (br.appear == 0)
                        continue;
                    else
                    {
                        pad.CollideType(br);
                        if (ball.CollideType(br) != Collision.NO)
                        {
                            br.appear = 0;
                            bricksLeft--;
                            break;
                        }
                    }
                }
                timer += gameTime.ElapsedGameTime.Milliseconds;
                if (timer > 1000)
                {
                    timeLeft--;
                    timer -= 1000;
                }
                ball.Update(gameTime);
                pad.Update(gameTime);
                base.Update(gameTime);
            }
            if (timeLeft == 0 || bricksLeft == 0)
            {
                KeyboardState keystate = Keyboard.GetState();
                if (keystate.IsKeyDown(Keys.Space))
                    timeLeft = GlobalData.maxTime;
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

            spriteBatch.Begin();
            spriteBatch.Draw(background, new Vector2(0, 0), Color.White);   
            
            pad.Draw(spriteBatch);
            ball.Draw(spriteBatch);
            spriteBatch.DrawString(Arial, "Time Left: " + timeLeft, new Vector2(800, 10), Color.White);
            spriteBatch.DrawString(Arial, "Bricks Left: " + bricksLeft, new Vector2(780, 40), Color.White);
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                {
                    if (Wall[i, j].appear == 1)
                        spriteBatch.Draw(Brick.bricks, Wall[i, j].position, Color.White);
                }

            if (newGame)
            {
                spriteBatch.DrawString(Arial, "Welcome!", new Vector2(250, 50), Color.Gold, 0F,
                    new Vector2(0, 0), 5F, SpriteEffects.None, 0);
                spriteBatch.DrawString(Arial, "Instructions:\n"+"Space: start the game\n" + "Arrow Keys: move the paddle\n"
                    +"Clear all the bricks in the given time.", 
                    new Vector2(200, 550), Color.GreenYellow, 0F,
                    new Vector2(0, 0), 1F, SpriteEffects.None, 0);
            }

            if (bricksLeft == 0)
            {
                spriteBatch.DrawString(Arial, "You won!", new Vector2(250, 250), Color.Gold, 0F,
                    new Vector2(0, 0), 5F, SpriteEffects.None, 0);
                spriteBatch.DrawString(Arial, "Press Space to restart the game.", new Vector2(200, 350), Color.GreenYellow, 0F,
                    new Vector2(0, 0), 2F, SpriteEffects.None, 0);
            }

            if(timeLeft==0) 
            {
                spriteBatch.DrawString(Arial,"Game Over", new Vector2(250, 250), Color.Gold,0F,
                    new Vector2(0,0),5F,SpriteEffects.None,0);
                spriteBatch.DrawString(Arial, "Press Space to restart the game.", new Vector2(200, 350), Color.GreenYellow, 0F,
                    new Vector2(0, 0), 2F, SpriteEffects.None, 0);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
