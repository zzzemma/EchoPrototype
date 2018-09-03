﻿using System;
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
        private int numPoints;
        private Random rand;

        public enum Type
        {
            Health = 0, AddPoints = 1, MinusPoints = 2 
        }

          public Consumable(int x, int y,int speed, Type type, GameContent gameContent, Player player, Scoremanager scoreManager)
        {
            switch(type)
            {
                case Type.Health:
                    {
                        imgConsumable = gameContent.imgPlusFruit;
                        Health = true;                     
                        break;
                    }

                case Type.AddPoints:
                    {
                        
                        imgConsumable = gameContent.imgFireFly;
                        Points = true;
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
            this.player = player;
            X = x;
            Y = y;
            this.speed = speed;
            minY = Y + 10;
            maxY = Y - 10;
            deltaY = maxY;
            Destroyed = true;
            this.scoreManager = scoreManager;
            consumableRect = new Rectangle((int)X,(int)Y,imgConsumable.Width/2,imgConsumable.Height/2);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Destroyed)
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
            if (HitTest(player.playerRect, consumableRect) && Destroyed)
            {
                if(Points)
                {
                    scoreManager.AddPoints(numPoints);
                    Destroyed = false;                   

                }

                if(Health)
                {
                    player.Health += 1;
                    Destroyed = false;
                }
            }

            X -= speed;
            consumableRect.X = (int) X;

            Sway();

            if(X <= -50)
            {
                Destroyed = false;
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
            
            if(deltaY == maxY)
            {
                Y -= .5f; 
            }

            if (deltaY == minY)
            {
                Y += .5f;
            }
        }
    }
}