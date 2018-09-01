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
    class TitleScreen
    {
        private Texture2D imgTitle { get; set; }
        private SpriteBatch spriteBatch;
        private GameContent gameContent;
        private int screenWidth;
        private int screenHeight;

        public TitleScreen(GameManager gameManager)
        {
            this.spriteBatch = gameManager.spriteBatch;
            this.gameContent = gameManager.gameContent;
            this.screenHeight = gameManager.screenHeight;
            this.screenWidth = gameManager.screenWidth;
            imgTitle = gameContent.imgTitle;
        }

        public void Draw()
        {
            spriteBatch.Draw(imgTitle, new Vector2(0, -50), null, Color.White, 0, new Vector2(0, 0), 0.25f, SpriteEffects.None, 0);
            string startMsg = "Press <Enter> to Start";
            Vector2 startSpace = gameContent.labelFont.MeasureString(startMsg);
            spriteBatch.DrawString(gameContent.labelFont, startMsg, new Vector2((screenWidth - startSpace.X) / 2, screenHeight / 2), Color.White);
        }
    }
}
