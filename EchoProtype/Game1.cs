using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace EchoProtype
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GameContent gameContent;
        private Player player;
        private float damageTimer;
        private float delayTime;
        private Maze maze;
        private int screenWidth = 0;
        private int screenHeight = 0;
        private MouseState oldMouseState;
        private KeyboardState oldKeyboardState;

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
            delayTime = 2; // delay inbetween taking damage
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
            gameContent = new GameContent(Content);
            screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            //set game to 502x700 or screen max if smaller
            if (screenWidth >= 700)
            {
                screenWidth = 700;
            }
            if (screenHeight >= 700)
            {
                screenHeight = 700;
            }
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.ApplyChanges();

            player = new Player(0.0f,650.0f,screenWidth, spriteBatch, gameContent);
           
            maze = new Maze(1.0f, 1.0f, spriteBatch, gameContent);

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

            if (IsActive == false)
            {
                return;  //our window is not active don't update
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            KeyboardState newKeyboardState = Keyboard.GetState();
            MouseState newMouseState = Mouse.GetState();
            
            //process keyboard events                           
            if (newKeyboardState.IsKeyDown(Keys.Left))
            {
                player.MoveLeft();
            }
            if (newKeyboardState.IsKeyDown(Keys.Right))
            {
                player.MoveRight();
            }
            if (newKeyboardState.IsKeyDown(Keys.Up))
            {
                player.MoveUp();
            }
            if (newKeyboardState.IsKeyDown(Keys.Down))
            {
                player.MoveDown();
            }

            player.playerRect = new Rectangle((int)player.X, (int)player.Y, (int)(player.Width + player.Width*.5), (int)(player.Height + player.Height*.5));//keeps track of player position

            //checks for collisions
            for (int i = 0; i < maze.maze.Length; i++)
            {
                if(HitTest(player.playerRect,maze.maze[i].wallRect))
                {
                    //makes player take damage
                    if (player.canTakeDamage)
                    {
                        player.Health -= maze.maze[i].damage;
                        damageTimer = (float)gameTime.TotalGameTime.TotalSeconds;
                        player.canTakeDamage = false;
                    }


                    // collision pushes player in the direction opposite of their movement.
                    if (newKeyboardState.IsKeyDown(Keys.Left))
                    {
                        player.MoveRight();
                    }
                    if (newKeyboardState.IsKeyDown(Keys.Right))
                    {
                        player.MoveLeft();
                    }
                    if (newKeyboardState.IsKeyDown(Keys.Up))
                    {
                        player.MoveDown();                      
                    }
                    if (newKeyboardState.IsKeyDown(Keys.Down))
                    {
                        player.MoveUp();
                    }
                }
            }
            Console.WriteLine(player.Health);
            if(gameTime.TotalGameTime.TotalSeconds >= damageTimer + delayTime)
            {
                player.canTakeDamage = true;
            }

            if(player.Health <= 0 && player.Visible)
            {
                player.Visible = false;
            }
            oldMouseState = newMouseState; // this saves the old state      
            oldKeyboardState = newKeyboardState;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            player.Draw();
            maze.Draw();
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public static bool HitTest(Rectangle r1, Rectangle r2)
        {
            if (Rectangle.Intersect(r1, r2) != Rectangle.Empty)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
