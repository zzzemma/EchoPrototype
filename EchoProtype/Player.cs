using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace EchoProtype
{
    class Player
    {
        public float X { get; set; } //x position of paddle on screen
        public float Y { get; set; } //y position of paddle on screen
        public float Width { get; set; } //width of paddle
        public float Height { get; set; } //height of paddle
        public float ScreenWidth { get; set; } //width of game screen
        public float ScreenHeight { get; set; } 

        public int Health { get; set; }//health
        public Rectangle playerRect { get; set; }
        public bool Visible { get; set; }

        //private Texture2D imgPlayer { get; set; }  //cached image of the paddle
        private List<Texture2D> _batImages { get; set; }
        private int _currentBatIndex { get; set; }
        private Point _batImageSize { get; set; }
        private Texture2D _sightBlocker { get; set; }
        private Point _sightImageSize { get; set; }

        private float _imageChangeSpan = 0.1f;
        private float _lastChangeTime { get; set; }

        private float _rotationAngle { get; set; }

        private SpriteBatch spriteBatch;  //allows us to write on backbuffer when we need to draw self
        public bool canTakeDamage = true;

        private Color batColor { get; set; }//change

        private bool hurt;


        public Player(float x, float y, float screenWidth, float screenHeight, SpriteBatch spriteBatch, GameContent gameContent)
        {
            //:'(
            Visible = true;
            X = x;
            Y = y;
            Width = 4;//change
            Height = 2;
            Health = 5;


            _batImages = gameContent.batList;
            _currentBatIndex = 0;
            _batImageSize = new Point(_batImages[0].Width, _batImages[0].Height);

            _sightBlocker = gameContent.blacksmall;
            _sightImageSize = new Point(_sightBlocker.Width, _sightBlocker.Height);

            _lastChangeTime = 0;

            _rotationAngle = 0;

            this.spriteBatch = spriteBatch;
            ScreenWidth = screenWidth;
            ScreenHeight = screenHeight;
            
        }

        public void Update(GameTime gameTime)
        {
            var currentTime = (float)gameTime.TotalGameTime.TotalMilliseconds;
            currentTime /= 1000;
            //Console.WriteLine("Player, currentTime: " + currentTime);
            if(currentTime - _lastChangeTime > _imageChangeSpan)
            {
                _currentBatIndex = (_currentBatIndex + 1) % 4;
                //Console.WriteLine(_currentBatIndex);
                _lastChangeTime = currentTime;
            }
        }

        public void Draw()
        {
            if (Visible)
            {
                var batDestinationRec = new Rectangle();
                var batSize = new Point(100, 100);

                batDestinationRec.X = (int)this.X;
                batDestinationRec.Y = (int)this.Y;
                batDestinationRec.Size = batSize;

                var sightOffset = new Point(-10, -30);
                var sightSize = new Point(5000, 10000);

                var sightDestinationRec = new Rectangle();
                sightDestinationRec.X = (int)this.X;
                sightDestinationRec.Y = (int)this.Y;
                //Console.WriteLine("Sight position: " + sightDestinationRec.X + " " + sightDestinationRec.Y);
                sightDestinationRec.Size = sightSize;

                spriteBatch.Begin();

                spriteBatch.Draw(_sightBlocker,
                    sightDestinationRec,
                    null,
                    batColor,
                    _rotationAngle,
                    new Vector2(_sightImageSize.X / 2 + sightOffset.X, _sightImageSize.Y / 2 + sightOffset.Y),
                    SpriteEffects.None,
                    0f
                    );

                spriteBatch.End();

                spriteBatch.Begin();
                //change
                if (!hurt)
                {
                    batColor = Color.White;
                }
                else
                {
                    batColor = Color.Red;
                }

                spriteBatch.Draw(_batImages[_currentBatIndex],
                    batDestinationRec,
                    null,
                    batColor,
                    _rotationAngle,
                    new Vector2(_batImageSize.X / 2, _batImageSize.Y / 2),
                    SpriteEffects.None,
                    0f
                    );
                //change
                spriteBatch.End();
            }
        }

        public void MoveLeft()
        {
            X = X - 5;

        }
        public void MoveUp()
        {
            Y = Y - 5;
            //change
            if(Y < 20)
            {
                Y = 20;
            }
            //_rotationAngle -= 0.1f;
            _rotationAngle = Math.Max(_rotationAngle, -MathHelper.Pi / 2);
        }
        public void MoveDown()
        {
            //change
            Y = Y + 5;

            if (Y > ScreenHeight - 20)
            {
                Y = ScreenHeight - 20;
            }
            //_rotationangle += 0.1f;
            _rotationAngle = Math.Min(_rotationAngle, MathHelper.Pi / 2);
        }
        public void MoveRight()
        {
            //change
            X = X + 5;

            if (X > ScreenWidth - 50)
            {
                X = ScreenWidth - 50;
            }
        }

        //change
        public void Hurt(bool hurt)
        {
            this.hurt = hurt;
        }
    }
}
