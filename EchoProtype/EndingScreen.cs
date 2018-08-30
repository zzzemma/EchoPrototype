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
    class EndingScreen
    {
        private Texture2D imgGameOver { get; set; }
        private SpriteBatch spriteBatch;
        private GameContent gameContent;
        private int screenWidth;
        private int screenHeight;

        public EndingScreen(int screenWidth, int screenHeight, SpriteBatch spriteBatch, GameContent gameContent)
        {
            imgGameOver = gameContent.imgGameOver;
            this.spriteBatch = spriteBatch;
            this.gameContent = gameContent;
            this.screenHeight = screenHeight;
            this.screenWidth = screenWidth;
        }

        public void Draw()
        {
            spriteBatch.Draw(imgGameOver, new Vector2(0, -50), null, Color.White, 0, new Vector2(0, 0), 0.25f, SpriteEffects.None, 0);
            string startMsg = "Press <Enter> to Restart";
            Vector2 startSpace = gameContent.labelFont.MeasureString(startMsg);
            spriteBatch.DrawString(gameContent.labelFont, startMsg, new Vector2((screenWidth - startSpace.X) / 2, screenHeight - screenHeight / 4), Color.White);
        }
    }
}
