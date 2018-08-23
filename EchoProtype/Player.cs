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

        private Texture2D imgPlayer { get; set; }  //cached image of the paddle
        private SpriteBatch spriteBatch;  //allows us to write on backbuffer when we need to draw self


        public Player(float x, float y, float screenWidth, SpriteBatch spriteBatch, GameContent gameContent)
        {
            X = x;
            Y = y;
            imgPlayer = gameContent.imgBall;
            Width = imgPlayer.Width;
            Height = imgPlayer.Height;
            this.spriteBatch = spriteBatch;
            ScreenWidth = screenWidth;
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

        public void Draw()
        {
            spriteBatch.Draw(imgPlayer, new Vector2(X, Y), null, Color.White, 0, new Vector2(0, 0), 2.0f, SpriteEffects.None, 0);
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
