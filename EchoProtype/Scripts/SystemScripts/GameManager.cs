﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace EchoProtype
{
    public class GameManager : Game
    {
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public GameContent gameContent;
        public Player player;
        private TitleScreen titleScreen;
        private EndingScreen gameOverScreen;
        private RollingBackGround backGround;
        public int screenWidth = 0;
        public int screenHeight = 0;
        public bool gameStart;
        public bool gameOver;
        private ObstacleSpawner obstacleSpawner;
        public Scoremanager scoreManager;
        public List<SoundEffect> soundEffects;
        private Consumable consumable;
        private Consumable consumableHealth;
        private GameManager game;

        public GameManager()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            Content.RootDirectory = "Content";
            soundEffects = new List<SoundEffect>();
        }

        protected override void Initialize()
        {
            gameStart = false;
            gameOver = false;
            base.Initialize();// don't remove messes up background
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

            player = new Player(200.0f,350.0f,this);
            titleScreen = new TitleScreen(this);
            scoreManager = new Scoremanager(this);
            gameOverScreen = new EndingScreen(this);
            backGround = new RollingBackGround();
            backGround.Load(spriteBatch, gameContent);

            MediaPlayer.Play(gameContent.songbg);

            MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;

            //obstacle code!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            obstacleSpawner = new ObstacleSpawner(50, screenWidth, screenWidth - 100, screenHeight - 10, 10, 750, 250, 9, this);

            consumable = new Consumable(1200, 300, 5, Consumable.Type.AddPoints, this); 
            consumableHealth = new Consumable(1000, 400, 3, Consumable.Type.Health, this);


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
            if (!gameOver)
            {
                backGround.Update(gameTime);
                player.Update(gameTime);
                obstacleSpawner.Update(gameTime);
                consumable.Update(gameTime);
                consumableHealth.Update(gameTime);
            }

            if (IsActive == false)
            {
                return;  //our window is not active don't update
            }


            //process keyboard events      

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState newKeyboardState = Keyboard.GetState();
                     
            
            if (newKeyboardState.IsKeyDown(Keys.Enter))
            {
                if (!gameStart)
                {
                    gameStart = true;
                }
                else
                {
                    //pause;
                }

                if(gameOver)
                {
                    game = new GameManager();
                    game.Run();
                    this.Exit();
                }
            }
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
                scoreManager.flytime = gameTime.TotalGameTime.Seconds;

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
                consumableHealth.Draw();

                spriteBatch.End();

                player.Draw();

                spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null);

                consumable.Draw();

                backGround.Draw();

                scoreManager.Draw(gameTime);

                spriteBatch.End();
            }
            base.Draw(gameTime);
        }
    }
}
