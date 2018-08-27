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
        public bool Visible { get; set; } //does brick still exist?
        public Rectangle hitBox { get; set; }

        public int damage { get; set; }

        private Texture2D imgStag { get; set; }  //cached image of the brick
        private SpriteBatch spriteBatch;  //allows us to write on backbuffer when we need to draw self

        public Stalagmite(float x, float y, bool vert, SpriteBatch spriteBatch, GameContent gameContent)
        {
            X = x;
            Y = y;
            damage = 10;
            imgStag = gameContent.imgStag;
            Width = imgStag.Width;
            Height = imgStag.Height;
            this.spriteBatch = spriteBatch;
            hitBox = new Rectangle((int)X, (int)Y, (int)(Width + (Width * 0.60)), (int)(Height + Height * 0.60));// Rectangle for the wall collider
            Visible = false;
        }

        public void Draw()
        {
            if (Visible)
            {
                spriteBatch.Draw(imgStag, new Vector2(X, Y), null, Color.White, 0, new Vector2(0, 0), 2.0f, SpriteEffects.None, 0);
            }
        }
    }
}

