using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EchoProtype
{
    class Stalagmite
    {

        public float X { get; set; } //x position of brick on screen
        public float Y { get; set; } //y position of brick on screen
        public float Width { get; set; } //width of brick
        public float Height { get; set; } //height of brick

        private float damageTimer { get; set; }
        private float delayTime { get; set; }

        public float speed;
        public bool Destroyed { get; set; } //does brick still exist?
        public bool Visible { get; set; }
        public Rectangle hitBox { get; set; }

        public int damage { get; set; }

        private GameManager gameManager;

        private Texture2D imgStag { get; set; }  //cached image of the brick
        private SpriteBatch spriteBatch;  //allows us to write on backbuffer when we need to draw self
        private Player player;

        public Stalagmite(float x, float y, float speed, bool vert, GameManager gameManager)
        {
            X = x;
            Y = y;
            this.speed = speed;
            damage = 1;
            player = gameManager.player;
            imgStag = gameManager.gameContent.imgStag;
            Width = imgStag.Width;
            Height = imgStag.Height;
            delayTime = 750;
            damageTimer = 0;
            this.spriteBatch = gameManager.spriteBatch;
            hitBox = new Rectangle((int)X, (int)Y, (int)(Width + (Width * 0.60)), (int)(Height + Height * 0.60));// Rectangle for the wall collider
            Destroyed = true;
            Visible = true;
            this.gameManager = gameManager;
        }

        public void Draw()
        {            
            if (!Destroyed)
            {
                spriteBatch.Draw(imgStag, new Vector2(X, Y), null, Color.White, 0, new Vector2(0, 0), 2.0f, SpriteEffects.None, 0);
            }
        }

        public void Update(GameTime gameTime)
        {
            Move();

            KeyboardState newKeyboardState = Keyboard.GetState();

            if (damageTimer > 0 && gameTime.TotalGameTime.TotalMilliseconds >= (damageTimer + delayTime))
            {
                player.hurt = false;
                damageTimer = 0;
                player.canTakeDamage = true;
            }

            //checks for collisions          
            if (!Destroyed && HitTest(player.playerRect, hitBox))
            {               
                //makes player take damage
                if (player.canTakeDamage)
                {                  
                    player.Health -= damage;
                    damageTimer = (float)gameTime.TotalGameTime.TotalMilliseconds;                  
                    player.canTakeDamage = false;
                    player.hurt = true;
                    gameManager.soundEffects[1].CreateInstance().Play();
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

        private bool HitTest(Rectangle r1, Rectangle r2)
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

        private void Move()
        {
                if (!Destroyed)
                {
                    X -= speed;
                    hitBox = new Rectangle((int)(X - speed), (int)Y, hitBox.Width,hitBox.Height);
                }
            }
        }
    }


