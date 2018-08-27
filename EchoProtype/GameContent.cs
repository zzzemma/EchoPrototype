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
        public Texture2D imgStag { get; set; }
        public SoundEffect echoAmb { get; set; }
   

      
        public GameContent(ContentManager Content)
        {
            echoAmb = Content.Load<SoundEffect>("echoAmb");
            imgBall = Content.Load<Texture2D>("Ball");
            labelFont = Content.Load<SpriteFont>("Arial20");
            imgBrick = Content.Load<Texture2D>("Brick");
            imgTitle = Content.Load<Texture2D>("Title");
            imgStag = Content.Load<Texture2D>("vertbrick");
        }
    }
}
