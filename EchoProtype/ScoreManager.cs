using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoProtype
{
    class Scoremanager
    {

        private SpriteBatch spriteBatch;
        private GameContent gameContent;
        private int screenWidth;
        private int screenHeight;
        public int flytime;
        private int currenttime;
        public bool flag = false;
        private int extraPoints;
        public Scoremanager(int screenWidth, int screenHeight, SpriteBatch spriteBatch, GameContent gameContent)
        {
            this.spriteBatch = spriteBatch;
            this.gameContent = gameContent;
            this.screenHeight = screenHeight;
            this.screenWidth = screenWidth;
        }
        public void Draw(GameTime gameTime)
        {

            string scoreMsg = "Score : ";
            if (!flag)
            {
                currenttime = gameTime.TotalGameTime.Seconds - flytime + extraPoints;
            }
            scoreMsg += currenttime;
            Vector2 stringSpace = gameContent.labelFont.MeasureString(scoreMsg);
            spriteBatch.DrawString(gameContent.labelFont, scoreMsg, new Vector2((screenWidth - stringSpace.X) - 100, screenHeight - 100), Color.White);
        }

        public void AddPoints(int points)
        {
            extraPoints += points;
        }

        public void SubtractPoints(int points)
        {
            extraPoints -= points;
        }
    }
}
