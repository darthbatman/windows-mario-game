using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace NewGameEngine
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D brickTexture;
        Texture2D itemTexture;

        SpriteFont testFont;

        Rectangle[] brickRectangles = new Rectangle[960];
        int brickRectanglesCount = 10;

        ItemBlock[] itemBlocks = new ItemBlock[960];
        int itemBlockCount = 0;
        int otherItemBlockCount = 0;

        int isUpCounter = 10;

        Player player;

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
            player = new Player(50, 400);
            player.LoadContent(Content);
            brickTexture = Content.Load<Texture2D>("bricksprite");
            itemTexture = Content.Load<Texture2D>("itemsprite");
            testFont = Content.Load<SpriteFont>("testFont");
            // TODO: use this.Content to load your game content here
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

            for (int i = 0; i < brickRectanglesCount; i++)
            {
                if (player.boundingRectangle.Intersects(brickRectangles[i]) && player.boundingRectangle.Bottom >= (brickRectangles[i].Top))
                {
                    player.position.Y--;
                    player.boundingRectangle.Y--;
                    player.isOnGround = true;
                }
                if (player.boundingRectangle.Intersects(brickRectangles[i]) && player.boundingRectangle.Right >= (brickRectangles[i].Left))
                {
                    player.position.X--;
                    player.boundingRectangle.X--;
                }
                if (player.boundingRectangle.Intersects(brickRectangles[i]) && player.boundingRectangle.Left <= (brickRectangles[i].Right))
                {
                    player.position.X++;
                    player.boundingRectangle.X++;
                }
                if (player.boundingRectangle.Intersects(brickRectangles[i]) && player.boundingRectangle.Top <= (brickRectangles[i].Bottom))
                {
                    //player.position.Y += 40;
                    //player.boundingRectangle.Y += 40;
                    player.jumping = false;
                }
            }


            for (int p = 0; p < itemBlockCount; p++)
            {
                if (player.boundingRectangle.Intersects(itemBlocks[p].boundingRectangle) && player.boundingRectangle.Bottom >= (itemBlocks[p].boundingRectangle.Top))
                {
                    player.position.Y--;
                    player.boundingRectangle.Y--;
                    player.isOnGround = true;
                }
                if (player.boundingRectangle.Intersects(itemBlocks[p].boundingRectangle) && player.boundingRectangle.Top <= (itemBlocks[p].boundingRectangle.Bottom))
                {
                    //player.position.Y += 40;
                    //player.boundingRectangle.Y += 40;
                    player.jumping = false;
                    
                    itemBlocks[p].wasStruck();
                }

                if (itemBlocks[p].isUp)
                {
                    itemBlocks[p].position.Y++;
                    itemBlocks[p].boundingRectangle.Y++;
                    isUpCounter--;
                    System.Diagnostics.Debug.WriteLine(isUpCounter);
                    if (isUpCounter == 0)
                    {
                        itemBlocks[p].isUp = false;
                        isUpCounter = 10;
                    }
                }
            }

            

                // TODO: Add your update logic here
            player.Update(gameTime);
            base.Update(gameTime);
        }

        bool firstDraw = true;

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            int lineNum = 0;

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            string line; 

            System.IO.StreamReader file = new System.IO.StreamReader("C:\\Rishi\\level1.txt");
            while ((line = file.ReadLine()) != null)
            {
                int pixelCount = 0;

                //System.Diagnostics.Debug.WriteLine(lineNum);
                for (int w = 0; w < line.Length; w++)
                {
                    if (line[w] == '1')
                    {
                        spriteBatch.Draw(brickTexture, new Vector2(pixelCount, (lineNum * 20)), Color.White);

                        if (firstDraw)
                        {
                            brickRectangles[brickRectanglesCount] = new Rectangle(pixelCount, (lineNum * 20), 20, 20);
                            brickRectanglesCount++;
                        }

                        pixelCount += 20;
                    }
                    else if (line[w] == '2')
                    {
                        
                        if (firstDraw)
                        {
                            spriteBatch.Draw(itemTexture, new Vector2(pixelCount, (lineNum * 20)), Color.White);
                            itemBlocks[itemBlockCount] = new ItemBlock(pixelCount, (lineNum * 20));
                            itemBlockCount++;
                        }
                        else
                        {
                            spriteBatch.Draw(itemTexture, new Vector2(itemBlocks[otherItemBlockCount].position.X, itemBlocks[otherItemBlockCount].position.Y), Color.White);
                            otherItemBlockCount++;
                        }

                        pixelCount += 20;
                    }
                    else if (line[w] == '0')
                    {
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

            player.Draw(spriteBatch);
            spriteBatch.DrawString(testFont, "[=][=][=][=][=][=][=][=][=][=]", new Vector2(30, 30), Color.Black);
            spriteBatch.End();

            firstDraw = false;
            otherItemBlockCount = 0;

            base.Draw(gameTime);
        }
    }
}
