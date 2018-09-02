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
    class Consumable
    {

        private bool Points = false;
        private bool Health = false;
        public bool Destroyed;
        public bool Visible;
        public float X;
        public float Y;
        private int speed;
        private float deltaY;
        private float maxY;
        private float minY;
        private Texture2D imgConsumable;
        private Player player;
        private Rectangle consumableRect;
        private Scoremanager scoreManager;
        private SpriteBatch spriteBatch;
        private int numPoints;
        private Random rand;
        private float visionTimer { get; set; }
        private float visionDelayTime { get; set; }

        public enum Type
        {
            Health = 0, AddPoints = 1, MinusPoints = 2
        }

        public Consumable(int x, int y, int speed, Type type, GameManager gameManager)
        {
            visionDelayTime = 1000;
            visionTimer = 0;
            Destroyed = false;
            Visible = false;
            switch (type)
            {
                case Type.Health:
                    {
                        imgConsumable = gameManager.gameContent.imgPlusFruit;
                        Health = true;                       
                        break;
                    }

                case Type.AddPoints:
                    {

                        imgConsumable = gameManager.gameContent.imgFireFly;
                        Points = true;
                        Visible = true;
                        numPoints = 1000;
                        break;
                    }
                case Type.MinusPoints:
                    {
                        //imgConsumable = gameContent.imgMinusFruit;
                        //Points = true;
                        //numPoints = -10;
                        break;
                    }
            }
            rand = new Random();
            this.player = gameManager.player;
            X = x;
            Y = y;
            this.speed = speed;
            minY = Y + 10;
            maxY = Y - 10;
            deltaY = maxY;
            Destroyed = false;
            Visible = false;
            this.scoreManager = gameManager.scoreManager;
            this.spriteBatch = gameManager.spriteBatch;
            consumableRect = new Rectangle((int)X, (int)Y, imgConsumable.Width / 2, imgConsumable.Height / 2);
        }

        public void Draw()
        {
            if (!Destroyed && Visible)
            {
                spriteBatch.Draw(imgConsumable, new Vector2(X, Y), null, Color.White, 0, new Vector2(0, 0), .50f, SpriteEffects.None, 0);
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

        public void Update(GameTime gameTime)
        {
            if (HitTest(player.playerRect, consumableRect) && !Destroyed)
            {
                if (Points)
                {
                    scoreManager.AddPoints(numPoints);
                    Visible = false;
                    Destroyed = true;

                }

                if (Health)
                {
                    player.Health += 1;
                    Visible = false;
                    Destroyed = true;
                }
            }

            //checks wave collisions
            for (int i = 0; i < player.echoWaves.Count; i++)
            {
                for (int j = 0; j < player.echoWaves[i].collisionRectangles.Count; j++)
                    if (!Destroyed && HitTest(player.echoWaves[i].collisionRectangles[j], consumableRect))
                    {
                        visionTimer = (float)gameTime.TotalGameTime.TotalMilliseconds;
                        Visible = true;
                    }
            }


            Move();
            Sway();
            VisionCheck(gameTime);


            if (X <= -50)
            {
                Destroyed = true;
                Visible = false;
            }
        }

        private void VisionCheck(GameTime gameTime)
        {
            if (Visible && !Points)
            {
                if (gameTime.TotalGameTime.TotalMilliseconds >= visionTimer + visionDelayTime)
                {
                    Visible = false;
                }
            }
        }

        public void Sway()
        {
            if (Y == maxY)
            {
                deltaY = minY;
            }

            if (Y == minY)
            {
                deltaY = maxY;
            }

            if (deltaY == maxY)
            {
                Y -= .5f;
            }

            if (deltaY == minY)
            {
                Y += .5f;
            }
        }

        private void Move()
        {
            if (!Destroyed)
            {
                X -= speed;
                consumableRect.X = (int)X;
            }
        }
    }
}
