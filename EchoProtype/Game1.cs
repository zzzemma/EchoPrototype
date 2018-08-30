using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace EchoProtype
{
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
        private Scoremanager time;
        public List<SoundEffect> soundEffects;
        private Consumable consumable;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            Content.RootDirectory = "Content";
            soundEffects = new List<SoundEffect>();
        }

        protected override void Initialize()
        {
            delayTime = 750; // delay inbetween taking damage change
            gameStart = false;
            gameOver = false;
            base.Initialize();
        }

        protected override void LoadContent()
        {
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

            player = new Player(200.0f,350.0f,screenWidth,screenHeight, spriteBatch, gameContent, this);
            titleScreen = new TitleScreen(screenWidth, screenHeight, spriteBatch,gameContent);
            time = new Scoremanager(screenWidth, screenHeight, spriteBatch, gameContent);
            gameOverScreen = new EndingScreen(screenWidth, screenHeight, spriteBatch, gameContent); //change
            backGround = new RollingBackGround();
            backGround.Load(spriteBatch, gameContent);

            MediaPlayer.Play(gameContent.songbg);

            MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;

            //obstacle code!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            obstacleSpawner = new ObstacleSpawner(50, screenWidth, screenWidth - 100, screenHeight - 10, 10, 750, 250, 9, spriteBatch, gameContent);//Change

            consumable = new Consumable(500, 300, 5, Consumable.Type.Health, gameContent, player); //example change


            soundEffects.Add(Content.Load<SoundEffect>("wings"));
            soundEffects.Add(Content.Load<SoundEffect>("hit01"));
        }

        void MediaPlayer_MediaStateChanged(object sender, System.EventArgs e)
        {
            // 0.0f is silent, 1.0f is full volume
            MediaPlayer.Volume -= 0.1f;
            MediaPlayer.Play(gameContent.songbg);
        }

        protected override void UnloadContent()
        {
        }

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
                        soundEffects[1].CreateInstance().Play();
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
                this.time.flag = true;
                gameOver = true;
            }
            oldMouseState = newMouseState; // this saves the old state      
            oldKeyboardState = newKeyboardState;
            obstacleSpawner.Spawn(gameTime,gameStart);
            obstacleSpawner.CleanUp();
            base.Update(gameTime);
        }

        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here
            //Title Screen
            if (!gameStart)
            {
                time.flytime = gameTime.TotalGameTime.Seconds;

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

                time.Draw(gameTime);

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
