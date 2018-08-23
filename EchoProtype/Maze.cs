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

    class Maze
    {
        private float W { get; set; }
        private float H { get; set; }
        public Wall[] maze { get; set; }
        private Texture2D imgWall { get; set; }

        public Maze(float x, float y, SpriteBatch spriteBatch, GameContent gameContent)
        {
            imgWall = gameContent.imgBrick;
            W = imgWall.Width;
            H = imgWall.Height;
            maze = new Wall[4];
            maze[0] = new Wall(1*W, 1*H,true, spriteBatch, gameContent);
            maze[1] = new Wall(1 * W, 1 * H, false, spriteBatch, gameContent);
            maze[2] = new Wall(5 * W, 1 * H, false, spriteBatch, gameContent);
            maze[3] = new Wall(3 * W, 1 * H, false, spriteBatch, gameContent);
        }

        public void Draw()
        {
            for (int i = 0; i < maze.Length; i++)
            {
                maze[i].Draw();
            }
        }
    }
}
