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
    class Player
    {
        public float X { get; set; } //x position of paddle on screen
        public float Y { get; set; } //y position of paddle on screen
        public float Width { get; set; } //width of paddle
        public float Height { get; set; } //height of paddle
        public float ScreenWidth { get; set; } //width of game screen

        public int Health { get; set; }//health
        public Rectangle playerRect { get; set; }
        public bool Visible { get; set; }

        private Texture2D imgPlayer { get; set; }  //cached image of the paddle
        private SpriteBatch spriteBatch;  //allows us to write on backbuffer when we need to draw self
        public bool canTakeDamage = true;


        public Player(float x, float y, float screenWidth, SpriteBatch spriteBatch, GameContent gameContent)
        {
            Visible = true;
            X = x;
            Y = y;
            Health = 100;
            imgPlayer = gameContent.imgBall;
            Width = imgPlayer.Width;
            Height = imgPlayer.Height;
            this.spriteBatch = spriteBatch;
            ScreenWidth = screenWidth;          
            
        }

        public void Draw()
        {
            if (Visible)
            {
                spriteBatch.Draw(imgPlayer, new Vector2(X, Y), null, Color.White, 0, new Vector2(0, 0), 2.0f, SpriteEffects.None, 0);
            }
        }

        public void MoveLeft()
        {
            X = X - 5;
        }
        public void MoveUp()
        {
            Y = Y - 5;         
        }
        public void MoveDown()
        {
            Y = Y + 5;           
        }
        public void MoveRight()
        {
            X = X + 5;
        }

        
    }
}
