﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace EchoProtype
{
    public class GameContent
    {
        public static GameContent instance;

        public Texture2D imgBrick { get; set; }
        public Texture2D imgSquare { get; set; }
        public Texture2D imgTitle { get; set; }
        public Texture2D imgGameOver { get; set; }
        public Texture2D imgPlusFruit { get; set; }
        public Texture2D imgFireFly { get; set; }
        public SpriteFont labelFont { get; set; }
        public SoundEffect echoAmb { get; set; }

        public Texture2D foregroundTexture { get; set; }
   
        public Texture2D blacksmall { get; set; }
        public Texture2D redheart { get; set; }
        public List<Texture2D> blackEchoList { get; set; }

        public List<Texture2D> batList { get; set; }
        public Texture2D imgStag { get; set; }
        public Song songbg { get; set; }

        public GameContent(ContentManager Content)
        {
            instance = this;

            echoAmb = Content.Load<SoundEffect>("SoundEffects/echoAmb");
            labelFont = Content.Load<SpriteFont>("Fonts/Arial20");
            imgTitle = Content.Load<Texture2D>("Sprites/Title");
            imgFireFly = Content.Load<Texture2D>("Sprites/powerupfirefly");
            imgGameOver = Content.Load<Texture2D>("Sprites/gameoverscreen");
            imgPlusFruit = Content.Load<Texture2D>("Sprites/powerupfruit");
            foregroundTexture = Content.Load<Texture2D>("Sprites/FG");
            blacksmall = Content.Load<Texture2D>("Sprites/black");
            redheart = Content.Load<Texture2D>("Sprites/redheart");
            imgStag = Content.Load<Texture2D>("Sprites/vertbrick");
            songbg = Content.Load<Song>("SoundEffects/bgsound");

            blackEchoList = new List<Texture2D>();
            for (var i = 0; i < 3; i++)
            {
                var index = i + 1;
                var currentEcho = Content.Load<Texture2D>("Sprites/black0" + index);
                blackEchoList.Add(currentEcho);
            }

            batList = new List<Texture2D>();
            for (var i = 0; i < 4; i++)
            {
                var index = i + 1;
                var currentBat = Content.Load<Texture2D>("Sprites/bat0" + index);
                batList.Add(currentBat);
            }
        }
    }
}
