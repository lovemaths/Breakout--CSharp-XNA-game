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

namespace Breaks
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        const int WIDTH = 600;
        const int HEIGHT = 800;
        const int ROWS = 15;
        const int COLS = 25;

        Texture2D paddle;
        Vector2 paddleCoor = new Vector2(100, 100);
        Texture2D ball;
        Vector2 ballCoor = new Vector2(0, 0);
        Texture2D[] bricks = new Texture2D[ROWS*COLS];
        Vector2[] brickCoor = new Vector2[ROWS * COLS];
        bool[] brickState = new bool[ROWS * COLS];

        bool moveUp = true;
        bool moveLeft = false;
  

        public bool ballBrickCollision(int brickPosition)
        {
            float x = brickCoor[brickPosition].X; float y = brickCoor[brickPosition].Y;

            if (moveUp && moveLeft && ballCoor.Y >= (y + bricks[0].Height - 3)
                && ballCoor.Y <= (y + bricks[0].Height + 3)
                && ballCoor.X <= (x + bricks[0].Width + 3) && ballCoor.X >= (x + bricks[0].Width))
            {
                moveUp = false; moveLeft = false; brickState[brickPosition] = false; return true;
            }
            if (!moveUp && moveLeft && ballCoor.Y >= (y - ball.Height - 3)
                && ballCoor.Y <= (y - ball.Height + 3)
                && ballCoor.X <= (x + bricks[0].Width + 3) && ballCoor.X >= (x + bricks[0].Width))
            {
                moveUp = true; moveLeft = false; brickState[brickPosition] = false; return true;
            }
            if (moveUp && !moveLeft && ballCoor.Y >= (y + bricks[0].Height - 3)
                && ballCoor.Y <= (y + bricks[0].Height + 3)
                && ballCoor.X <= (x - ball.Width + 3) && ballCoor.X >= (x - ball.Width - 3))
            {
                moveUp = false; moveLeft = true; brickState[brickPosition] = false; return true;
            }
            if (!moveUp && !moveLeft && ballCoor.Y >= (y - ball.Height - 3)
                && ballCoor.Y <= (y - ball.Height + 3)
                && ballCoor.X <= (x - ball.Width + 3) && ballCoor.X >= (x - ball.Width - 3))
            {
                moveUp = true; moveLeft = true; brickState[brickPosition] = false; return true;
            }
            if (moveUp && ballCoor.Y >= (y + bricks[0].Height - 3)
                && ballCoor.Y <= (y + bricks[0].Height + 3)
                && ballCoor.X <= (x + bricks[0].Width + 3) && ballCoor.X >= (x - ball.Width - 3))
            {
                moveUp = false; brickState[brickPosition] = false; return true;
            }
            if (!moveUp && ballCoor.Y >= (y - ball.Height - 3)
                && ballCoor.Y <= (y + 3)
                && ballCoor.X <= (x + bricks[0].Width + 3) && ballCoor.X >= (x - ball.Width - 3))
            {
                moveUp = true; brickState[brickPosition] = false; return true;
            }
            if (!moveLeft && ballCoor.Y >= (y - ball.Height - 3)
                && ballCoor.Y <= (y + bricks[0].Height + 3)
                && ballCoor.X <= (x - ball.Width + 3) && ballCoor.X >= (x - ball.Width - 3))
            {
                moveLeft = true; brickState[brickPosition] = false; return true;
            }
            if (moveLeft && ballCoor.Y >= (y - ball.Height - 3)
                && ballCoor.Y <= (y + bricks[0].Height + 3)
                && ballCoor.X <= (x + bricks[0].Width + 3) && ballCoor.X >= (x + bricks[0].Width - 3))
            {
                moveLeft = false; brickState[brickPosition] = false; return true;
            }
            return false;
        }

        enum Gamestate { MainMenu, StageOne, StageTwo }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = WIDTH;
            graphics.PreferredBackBufferWidth = HEIGHT;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            paddle = Content.Load<Texture2D>("paddle");
            paddleCoor = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2 - 50, graphics.GraphicsDevice.Viewport.Height - 50);
            ball = Content.Load<Texture2D>("ball");
            ballCoor = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2+100, graphics.GraphicsDevice.Viewport.Height-50 );

            for (int i = 0; i < ROWS; i++)
                for (int j = 0; j < COLS; j++)
                {
                    int k = COLS * i + j;
                    bricks[k] = Content.Load<Texture2D>("bricks");
                    brickCoor[k].X = bricks[k].Width * j;
                    brickCoor[k].Y = bricks[k].Height * i + 50;
                    if (j>=13&&((j-13)/3)%2==0) brickState[k] = true;
                    else if (j >= 10 && (i/3)%2==1) brickState[k] = true;
                    else if (j < 3) brickState[k] = true;
                    else if (j >= 3 && j < 10 && (i <= 2 || i >= 11)) brickState[k] = true;
                    else brickState[k] = false;
                }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            //move the paddle based on the key pressed
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Left) && paddleCoor.X > 0) paddleCoor.X -= 10;
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Right) && paddleCoor.X < graphics.GraphicsDevice.Viewport.Width - paddle.Width) paddleCoor.X += 10;

            // move the ball up/down/left/right automatically
            if (moveLeft && ballCoor.X <= 0) moveLeft = false;
            if (!moveLeft && ballCoor.X > graphics.GraphicsDevice.Viewport.Width - ball.Width) moveLeft = true;
            if (moveUp && ballCoor.Y <= 0) moveUp = false;
            if (moveLeft) ballCoor.X -= 3;
            else ballCoor.X += 3;
            if (moveUp) ballCoor.Y -= 3;
            else ballCoor.Y += 3;


            //collision detection
            if (ballCoor.X > paddleCoor.X - ball.Width
                && ballCoor.X < paddleCoor.X + paddle.Width
                && ballCoor.Y >= paddleCoor.Y - ball.Height - 1.5
                && ballCoor.Y <= paddleCoor.Y - ball.Height + 1.5)
                moveUp = true;

            //collision detection with the bricks
            for (int k = 0; k < ROWS*COLS; k++)
                if (brickState[k] == true)
                    ballBrickCollision(k);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            spriteBatch.Draw(paddle, paddleCoor, Color.White);
            for (int i = 0; i < ROWS*COLS; i++)
                if (brickState[i] == true)
                    spriteBatch.Draw(bricks[i], brickCoor[i], Color.White);
            spriteBatch.Draw(ball, ballCoor, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
