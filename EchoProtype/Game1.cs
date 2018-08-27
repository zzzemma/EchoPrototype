using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        private TitleScreen titleScreen;
        private float damageTimer;
        private float delayTime;
        private ObstacleSpawner obstacleSpawner;
        private int screenWidth = 0;
        private int screenHeight = 0;
        private MouseState oldMouseState;
        private KeyboardState oldKeyboardState;
        private bool gameStart;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
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
            gameStart = false;
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
            if (screenWidth >= 1275)
            {
                screenWidth = 1275;
            }
            if (screenHeight >= 725)
            {
                screenHeight = 725;
            }
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.ApplyChanges();

            player = new Player(0.0f,650.0f,screenWidth, spriteBatch, gameContent);
            titleScreen = new TitleScreen(screenWidth, screenHeight, spriteBatch,gameContent);
           
            obstacleSpawner = new ObstacleSpawner(8,screenWidth, screenWidth-100, screenHeight-100,100,300,100,5, spriteBatch, gameContent);

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
            if (newKeyboardState.IsKeyDown(Keys.Enter))
            {
                if (!gameStart)
                {
                    gameStart = true;
                }
                else
                {
                  //pause?
                }
            }

            player.playerRect = new Rectangle((int)player.X, (int)player.Y, (int)(player.Width + player.Width*.5), (int)(player.Height + player.Height*.5));//keeps track of player position

            //checks for collisions
            for (int i = 0; i < obstacleSpawner.obstacles.Length; i++)
            {
                if(obstacleSpawner.obstacles[i].Visible && HitTest(player.playerRect,obstacleSpawner.obstacles[i].hitBox))
                {
                    //makes player take damage
                    if (player.canTakeDamage)
                    {
                        player.Health -= obstacleSpawner.obstacles[i].damage;
                        damageTimer = (float)gameTime.TotalGameTime.TotalSeconds;
                        player.canTakeDamage = false;
                    }


                    // collision pushes player in the direction opposite of their movement.
                    if (newKeyboardState.IsKeyDown(Keys.Left))
                    {
                        player.MoveRight();
                        player.MoveRight();
                    }
                    if (newKeyboardState.IsKeyDown(Keys.Up))
                    {
                        player.MoveLeft();
                        player.MoveLeft();
                        player.MoveDown();                      
                    }
                    if (newKeyboardState.IsKeyDown(Keys.Down))
                    {
                        player.MoveLeft();
                        player.MoveLeft();
                        player.MoveUp();
                    }
                    else
                    {
                        player.MoveLeft();
                        player.MoveLeft();
                    }
                }
            }
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
            obstacleSpawner.Spawn(gameTime);
            obstacleSpawner.CleanUp();
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
            //Title Screen
            if (!gameStart)
            {
                PlaySound(gameContent.echoAmb);
                titleScreen.Draw();
            }
            else
            {
                player.Draw();
                obstacleSpawner.Draw();
            }
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

        public static void PlaySound(SoundEffect sound)
        {
            float volume = 1;
            float pitch = 0.0f;
            float pan = 0.0f;
            sound.Play(volume, pitch, pan);
        }
    }
}
