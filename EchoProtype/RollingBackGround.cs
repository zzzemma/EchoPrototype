using System.Threading.Tasks;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace EchoProtype
{
    
        class RollingBackGround
        {
            // public references & variables


            // private references & variables
            private Texture2D backgroundTexture;
            private SpriteBatch spriteBatch;
            private Point windowSize;
            private float startTime;
            private float currentTime;
            private float rollSpeed = 5;

            // constructor
            public RollingBackGround()
            {
                // TODO: might use the GameContentClass later

                Initialize();
            }

            // use this for initialize
            public void Initialize()
            {

            }

            public void Load(SpriteBatch spriteBatch, GameContent content)
            {
                this.spriteBatch = spriteBatch;
                backgroundTexture = content.foregroundTexture;
                windowSize = spriteBatch.GraphicsDevice.PresentationParameters.Bounds.Size;
            }

            public void Update(GameTime gameTime)
            {
                //Console.WriteLine("gameTime: "+gameTime.TotalGameTime.TotalMilliseconds);
                currentTime = (float)gameTime.TotalGameTime.TotalMilliseconds / 100;
            }

            public void Draw()
            {
                var destinationRec = new Rectangle();
                destinationRec.Size = windowSize;
                var sourceRec = new Rectangle();
                sourceRec.Size = new Point(backgroundTexture.Width, backgroundTexture.Height);
                sourceRec.Offset(currentTime * rollSpeed, 0);
                spriteBatch.Draw(backgroundTexture,
                    destinationRec,
                    sourceRec,
                    Color.White
                    );
            }
        }
    }

