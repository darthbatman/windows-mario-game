using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace newJumpTest
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D marioSprite;
        bool isOnGround = true;
        Vector2 position = new Vector2(40, 440);
        float jumpVelocity = -16;
        float acceleration = 1;
        Rectangle marioBound = new Rectangle(40, 442, 17, 20);
        float fallVelocity = 0;

        bool jumping = false;


        Texture2D brickSprite;
        Rectangle[] brickBounds = new Rectangle[960];
        int actualBrickCount = 0;
        bool firstDraw = true;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

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

            // TODO: use this.Content to load your game content here
            marioSprite = Content.Load<Texture2D>("mariojump");
            brickSprite = Content.Load<Texture2D>("bricksprite");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                position.X += 2;
                marioBound.X += 2;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                position.X -= 2;
                marioBound.X -= 2;
            }

            // TODO: Add your update logic here

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                //isOnGround = false;
                jumping = true;
            }

            if (!isOnGround && jumping)
            {
                position.Y += jumpVelocity;
                marioBound.Y += (int)jumpVelocity;
                jumpVelocity += acceleration;
            }
            else
            {
                jumpVelocity = -16;

            }

            if (!isOnGround && !jumping)
            {
                marioBound.Y += (int)fallVelocity;
                position.Y += (int)fallVelocity;
                fallVelocity += acceleration;
            }

            for (int b = 0; b < actualBrickCount; b++)
            {
                if (marioBound.Intersects(brickBounds[b]) && marioBound.Bottom > brickBounds[b].Top && !(marioBound.Intersects(brickBounds[b]) && marioBound.Top < brickBounds[b].Bottom && (brickBounds[b].Y + 20) < marioBound.Bottom))
                {
                    isOnGround = true;
                    jumping = false;
                    fallVelocity = 0;
                    position.Y -= 1;
                    marioBound.Y -= 1;
                }
                else
                {
                    isOnGround = false;
                }
            
                if (marioBound.Intersects(brickBounds[b]) && marioBound.Top < brickBounds[b].Bottom && (brickBounds[b].Y + 20) < marioBound.Bottom)
                {
                    isOnGround = false;
                    jumping = false;
                    if (!isOnGround)
                    {
                        position.Y += 8;
                        marioBound.Y += 8;
                    }
                }

                if (marioBound.Intersects(brickBounds[b]) && marioBound.Right > brickBounds[b].Left)
                {
                    position.X--;
                    marioBound.X--;
                }

                if (marioBound.Intersects(brickBounds[b]) && marioBound.Left < brickBounds[b].Right)
                {
                    position.X++;
                    marioBound.X++;
                }

            }

                base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            spriteBatch.Begin();

            int lineNum = 0;

            int brickCount = 0;

            string line;

            System.IO.StreamReader file = new System.IO.StreamReader("C:\\Rishi\\level1.txt");
            while ((line = file.ReadLine()) != null)
            {
                int pixelCount = 0;

                for (int w = 0; w < line.Length; w++)
                {
                    if (line[w] == '1')
                    {
                        Vector2 brickPos = new Vector2(pixelCount, (lineNum * 20));
                        Rectangle brick = new Rectangle((int)brickPos.X, (int)brickPos.Y, 20, 20);
                        brickBounds[brickCount] = brick;
                        brickCount++;
                        if (firstDraw)
                        {
                            actualBrickCount++;
                        }
                        spriteBatch.Draw(brickSprite, brickPos, Color.White);
                        pixelCount += 20;
                    }
                    else if (line[w] == '2')
                    {
                        
                        pixelCount += 20;
                    }
                    else if (line[w] == '0')
                    {
                        //blank space
                        pixelCount += 20;
                    }
                    else if (line[w] == ' ')
                    {
                        //nothing
                    }

                }
                lineNum++;
            }
            file.Close();

            firstDraw = false;
 
            spriteBatch.Draw(marioSprite, position, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}