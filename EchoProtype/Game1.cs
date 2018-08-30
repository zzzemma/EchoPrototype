using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
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
        private Player player; //change
        private TitleScreen titleScreen;
        private EndingScreen gameOverScreen;
        private RollingBackGround backGround;
        private float damageTimer;
        private float delayTime;
        private int screenWidth = 0;
        private int screenHeight = 0;
        private MouseState oldMouseState;
        private KeyboardState oldKeyboardState;
        private bool gameStart;
        private bool gameOver;
        private ObstacleSpawner obstacleSpawner;
        private Consumable consumable;

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
            delayTime = 750; // delay inbetween taking damage change
            gameStart = false;
            gameOver = false;
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

            player = new Player(200.0f,350.0f,screenWidth,screenHeight, spriteBatch, gameContent);
            titleScreen = new TitleScreen(screenWidth, screenHeight, spriteBatch,gameContent);
            gameOverScreen = new EndingScreen(screenWidth, screenHeight, spriteBatch, gameContent); //change
            backGround = new RollingBackGround();
            backGround.Load(spriteBatch, gameContent);

            MediaPlayer.Play(gameContent.songbg);

            MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;

            //obstacle code!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            obstacleSpawner = new ObstacleSpawner(50, screenWidth, screenWidth - 100, screenHeight - 10, 10, 750, 250, 9, spriteBatch, gameContent);//Change
            consumable = new Consumable(500, 300, Consumable.Type.Health, gameContent, player); //example change


        }

        void MediaPlayer_MediaStateChanged(object sender, System.EventArgs e)
        {
            // 0.0f is silent, 1.0f is full volume
            MediaPlayer.Volume -= 0.1f;
            MediaPlayer.Play(gameContent.songbg);
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
            backGround.Update(gameTime);
            player.Update(gameTime);
            consumable.Update(gameTime);//change

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

            player.playerRect = new Rectangle((int)player.X, (int)player.Y, (int)player.Width, (int)player.Height);//keeps track of player position change

            //checks for collisions
            for (int i = 0; i < obstacleSpawner.obstacles.Length; i++)
            {
                if(obstacleSpawner.obstacles[i].Visible && HitTest(player.playerRect, obstacleSpawner.obstacles[i].hitBox))
                {
                    //makes player take damage
                    if (player.canTakeDamage)
                    {
                        player.Health -= obstacleSpawner.obstacles[i].damage;
                        damageTimer = (float)gameTime.TotalGameTime.TotalMilliseconds;//change
                        player.canTakeDamage = false;
                        player.Hurt(true);
                    }


                    // collision pushes player in the direction opposite of their movement.
                    if (newKeyboardState.IsKeyDown(Keys.Left))
                    {
                        player.MoveRight();
                        player.MoveRight();
                        player.MoveRight();
                    }
                    if (newKeyboardState.IsKeyDown(Keys.Up))
                    {
                        player.MoveLeft();
                        player.MoveLeft();
                        player.MoveLeft();
                        player.MoveDown();
                    }
                    if (newKeyboardState.IsKeyDown(Keys.Down))
                    {
                        player.MoveLeft();
                        player.MoveLeft();
                        player.MoveLeft();
                        player.MoveUp();
                    }
                    else
                    {
                        player.MoveLeft();
                        player.MoveLeft();
                        player.MoveLeft();
                    }
                }
            }

            if(gameTime.TotalGameTime.TotalMilliseconds >= damageTimer + delayTime)//change
            {
                player.canTakeDamage = true;
                player.Hurt(false);
            }

            if(player.Health <= 0 && player.Visible || player.X <= -50) //change
            {

                player.Visible = false;
                gameOver = true;
            }
            oldMouseState = newMouseState; // this saves the old state      
            oldKeyboardState = newKeyboardState;
            obstacleSpawner.Spawn(gameTime,gameStart);
            obstacleSpawner.CleanUp();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here
            //Title Screen
            if (!gameStart)
            {
                spriteBatch.Begin();
                titleScreen.Draw();

                spriteBatch.End();
            }
            else if (gameOver)
            {
                spriteBatch.Begin();
                gameOverScreen.Draw();

                spriteBatch.End();
            }
            else
            {

                spriteBatch.Begin();

                obstacleSpawner.Draw();

                spriteBatch.End();

                player.Draw();

                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null);

                consumable.Draw(spriteBatch);
                backGround.Draw();

                spriteBatch.End();
            }

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
