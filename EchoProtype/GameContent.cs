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
        public SpriteFont labelFont { get; set; }
        public Texture2D imgBall { get; set; }
        public SoundEffect startSound { get; set; }
        public SoundEffect brickSound { get; set; }
        public SoundEffect paddleBounceSound { get; set; }
        public SoundEffect wallBounceSound { get; set; }
        public SoundEffect missSound { get; set; }

      
        public GameContent(ContentManager Content)
        {
            startSound = Content.Load<SoundEffect>("StartSound");
            brickSound = Content.Load<SoundEffect>("BrickSound");
            paddleBounceSound = Content.Load<SoundEffect>("PaddleBounceSound");
            wallBounceSound = Content.Load<SoundEffect>("WallBounceSound");
            missSound = Content.Load<SoundEffect>("MissSound");
            imgBall = Content.Load<Texture2D>("Ball");
            labelFont = Content.Load<SpriteFont>("Arial20");
            imgBrick = Content.Load<Texture2D>("Brick");
        }
    }
}
