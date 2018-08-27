using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace EchoProtype
{
    class GameContent
    {
        public Texture2D imgBrick { get; set; }
        public Texture2D imgTitle { get; set; }
        public SpriteFont labelFont { get; set; }
        public Texture2D imgBall { get; set; }
        public SoundEffect echoAmb { get; set; }

        public Texture2D backgroundTexture { get; set; }
   
        public Texture2D blacksmall { get; set; }

        public List<Texture2D> blackEchoList { get; set; }

        public List<Texture2D> batList { get; set; }
        public Texture2D imgStag { get; set; }

        public GameContent(ContentManager Content)
        {
            echoAmb = Content.Load<SoundEffect>("echoAmb");
            imgBall = Content.Load<Texture2D>("Ball");
            labelFont = Content.Load<SpriteFont>("Arial20");
            imgBrick = Content.Load<Texture2D>("Brick");
            imgTitle = Content.Load<Texture2D>("Title");
            backgroundTexture = Content.Load<Texture2D>("newCaveBackground");
            blacksmall = Content.Load<Texture2D>("black");
            imgStag = Content.Load<Texture2D>("vertbrick");

            blackEchoList = new List<Texture2D>();
            for (var i = 0; i < 3; i++)
            {
                var index = i + 1;
                var currentEcho = Content.Load<Texture2D>("black0" + index);
                blackEchoList.Add(currentEcho);
                Console.WriteLine("Load Texture: black0" + index);
            }

            batList = new List<Texture2D>();
            for (var i = 0; i < 4; i++)
            {
                var index = i + 1;
                var currentBat = Content.Load<Texture2D>("bat0" + index);
                batList.Add(currentBat);
                Console.WriteLine("Load Texture: bat0" + index);
            }
        }
    }
}
