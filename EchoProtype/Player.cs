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
    class Player
    {
        public float X { get; set; } //x position of paddle on screen
        public float Y { get; set; } //y position of paddle on screen
        public float Width { get; set; } //width of paddle
        public float Height { get; set; } //height of paddle
        public float ScreenWidth { get; set; } //width of game screen

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


        public Player(float x, float y, float screenWidth, SpriteBatch spriteBatch, GameContent gameContent)
        {
            Visible = true;
            X = x;
            Y = y;
            Health = 100;

            //imgPlayer = gameContent.imgBall;

            _batImages = gameContent.batList;
            _currentBatIndex = 0;
            _batImageSize = new Point(_batImages[0].Width, _batImages[0].Height);

            _sightBlocker = gameContent.blacksmall;
            _sightImageSize = new Point(_sightBlocker.Width, _sightBlocker.Height);

            _lastChangeTime = 0;

            _rotationAngle = 0;

            //Width = imgPlayer.Width;
            //Height = imgPlayer.Height;

            this.spriteBatch = spriteBatch;
            ScreenWidth = screenWidth;          
            
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

                var sightOffset = new Point(-10, -22);
                var sightSize = new Point(2000, 2000);

                var sightDestinationRec = new Rectangle();
                sightDestinationRec.X = (int)this.X;
                sightDestinationRec.Y = (int)this.Y;
                //Console.WriteLine("Sight position: " + sightDestinationRec.X + " " + sightDestinationRec.Y);
                sightDestinationRec.Size = sightSize;

                spriteBatch.Begin();

                spriteBatch.Draw(_sightBlocker,
                    sightDestinationRec,
                    null,
                    Color.White,
                    _rotationAngle,
                    new Vector2(_sightImageSize.X / 2 + sightOffset.X, _sightImageSize.Y / 2 + sightOffset.Y),
                    SpriteEffects.None,
                    0f
                    );

                spriteBatch.End();

                spriteBatch.Begin();

                spriteBatch.Draw(_batImages[_currentBatIndex],
                    batDestinationRec,
                    null,
                    Color.White,
                    _rotationAngle,
                    new Vector2(_batImageSize.X / 2, _batImageSize.Y / 2),
                    SpriteEffects.None,
                    0f
                    );

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
            _rotationAngle -= 0.1f;
            _rotationAngle = Math.Max(_rotationAngle, -MathHelper.Pi / 2);
        }
        public void MoveDown()
        {
            Y = Y + 5;
            _rotationAngle += 0.1f;
            _rotationAngle = Math.Min(_rotationAngle, MathHelper.Pi / 2);
        }
        public void MoveRight()
        {
            X = X + 5;
        }
    }
}
