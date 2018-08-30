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
    class ObstacleSpawner
    {
        private int maxX;
        private int minX;
        private int maxY;
        private int minY;
        private int maxTime;
        private int minTime;
        private int speed;
        private int counter;
        private float spawnTimer;
        private float deltaTime;
        private GameTime gameTime;
        public Stalagmite[] obstacles;
        private Random rand;

        public ObstacleSpawner(int totalNumObs, int maxX,int minX,int maxY, int minY,int maxTime,int minTime, int speed,SpriteBatch spriteBatch, GameContent gameContent)
        {
            this.maxTime = maxTime;
            this.minTime = minTime;
            this.maxX = maxX;
            this.minX = minX;
            this.minY = minY;
            this.maxY = maxY;
            this.speed = speed;
            obstacles = new Stalagmite[totalNumObs];
            gameTime = new GameTime();
            rand = new Random();
            spawnTimer = (float)gameTime.TotalGameTime.TotalMilliseconds;
            deltaTime = rand.Next(minTime, maxTime);
            for (int i = 0; i < obstacles.Length; i++)
            {
                float X = rand.Next(minX, maxX);
                float Y = rand.Next(minY, maxY);

                obstacles[i] = new Stalagmite((float)X,(float)Y,false,spriteBatch,gameContent);
            }
        }
        public void Draw()
        {          
            for (int i = 0; i < obstacles.Length; i++)
            {
                if(obstacles[i].Destoyed)
                {
                    obstacles[i].Draw();
                }
            }

            Move();
        }
        
        public void Move()
        {
            for(int i = 0; i < obstacles.Length; i++)
            {
                if (obstacles[i].Destoyed)
                {
                    obstacles[i].X -= speed;
                    obstacles[i].hitBox = new Rectangle((int)(obstacles[i].X - speed), (int)obstacles[i].Y, obstacles[i].hitBox.Width, obstacles[i].hitBox.Height);
                }
            }
        }

        public void CleanUp()
        {
            for (int i = 0; i < obstacles.Length; i++)
            {
                if (obstacles[i].Destoyed && obstacles[i].X < -50)
                {
                    obstacles[i].Destoyed = false;                  
                }
            }

        }

        public void Spawn(GameTime gameTime, bool gameStart)
        {
            if (gameTime.TotalGameTime.TotalMilliseconds >= (spawnTimer + deltaTime) && counter < obstacles.Length && gameStart)
            {              
                obstacles[counter].Destoyed = true;
                counter++;
                deltaTime = rand.Next(minTime, maxTime);
                spawnTimer = (float)gameTime.TotalGameTime.TotalMilliseconds;
            }

            if (counter == obstacles.Length & obstacles[obstacles.Length - 1].Destoyed == false) //change
            {
                for (int i = 0; i < obstacles.Length; i++)
                {
                    obstacles[i].X = rand.Next(minX, maxX);
                    obstacles[i].Y = rand.Next(minY, maxY);
                }
                speed += 1;
                counter = 0;
            }
        }
    }
}
