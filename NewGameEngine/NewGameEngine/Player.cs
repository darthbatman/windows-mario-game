using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace NewGameEngine
{
    class Player
    {
        public Vector2 position;
        public int frameIndex;
        public Texture2D texture;
        public Texture2D jumpTexture;
        public Rectangle[] frameRectangles = new Rectangle[4];
        public Rectangle boundingRectangle;
        public bool isOnGround = true;
        public bool jumping = false;
        private int currentJumpHeight = 0;
        public double gravity = 0.5;
        public double velocity = 0;

        private double timeElapsed = 0;
        private double timeToUpdate = 0.25;

        public Player(int x, int y)
        {
            position = new Vector2(x, y);
            boundingRectangle = new Rectangle(x, y, 17, 20);
        }

        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("mariowalk");
            jumpTexture = content.Load<Texture2D>("mariojump");
            addAnimation();
        }

        private void addAnimation()
        {
            int width = texture.Width / 4;
            for (int i = 0; i < 4; i++)
            {
                frameRectangles[i] = new Rectangle(i * width, 0, width, texture.Height);
            }
        }

        public void Update(GameTime gameTime)
        {
            timeElapsed += gameTime.ElapsedGameTime.TotalSeconds;

            if (timeElapsed > timeToUpdate)
            {
                timeElapsed -= timeToUpdate;

                if (frameIndex < (frameRectangles.Length - 1) && (Keyboard.GetState().IsKeyDown(Keys.Right) || Keyboard.GetState().IsKeyDown(Keys.Left)) && isOnGround && !jumping)
                {
                    frameIndex++;
                }
                else
                {
                    frameIndex = 0;
                }
            }
            
            position.Y++;
            boundingRectangle.Y++;

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                position.X++;
                boundingRectangle.X++;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                position.X--;
                boundingRectangle.X--;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                position.Y++;
                boundingRectangle.Y++;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up) && !jumping)
            {
                if (isOnGround)
                {
                    jumping = true;
                    //position.Y -= 100;
                    //boundingRectangle.Y -= 100;
                    isOnGround = false;
                }
            }

            if (jumping)
            {
                if (currentJumpHeight < 120)
                {
                    position.Y -= 6;
                    boundingRectangle.Y -= 6;
                    currentJumpHeight += 6;

                }
                else
                {
                    jumping = false;
                    currentJumpHeight = 0;
                }
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (jumping)
            {
                spriteBatch.Draw(jumpTexture, position, frameRectangles[frameIndex], Color.White);
                System.Diagnostics.Debug.WriteLine("JUMPING");
            }
            else
            {
                spriteBatch.Draw(texture, position, frameRectangles[frameIndex], Color.White);
            }     
        }

    }
}
