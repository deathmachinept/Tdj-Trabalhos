using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TP_Tank_Game
{



    class Animated_Sprite : Sprite
    {
        private int ncols, nrows;
        private Point currentFrame;
        private float animationInterval = 1f / 12f;
        private float animationTimer = 0f;
        public int numLoops;
        private int contador;
        
        public bool Loop { get; set; }

        public Animated_Sprite(ContentManager contents, String fileName, int nrows, int ncols)
            : base(contents, fileName)
        {
            this.ncols = ncols;
            this.nrows = nrows;
            this.pixelSize.X = this.pixelSize.X / ncols;
            this.pixelSize.Y = this.pixelSize.Y / nrows;

            this.size = new Vector2(1f, (float)pixelSize.Y / (float)pixelSize.X);
            this.origem = this.pixelSize * 0.5f;
            this.currentFrame = Point.Zero;
            this.numLoops = 0;
            Loop = false;
        }

        public void nextFrame()
        {
            if (currentFrame.X < ncols - 1)
            {
                currentFrame.X++;
            }
            else if (currentFrame.Y < nrows - 1)
            {
                currentFrame.X = 0;
                currentFrame.Y++;
            }
            else if(Loop)
            {
                currentFrame = Point.Zero;
            }
            else if (numLoops > 0)
            {
                currentFrame = Point.Zero;
                numLoops--;
            }
            else
            {
                Destroy();
            }
        }



        public override void Update(GameTime gameTime)
        {
                animationTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (animationTimer > animationInterval)
                {
                    animationTimer = 0f;
                    if (numLoops >= 0)
                        nextFrame();
                    else
                        Destroy();
                }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {

            source = new Rectangle((int)currentFrame.X * (int)pixelSize.X, 
                (int)currentFrame.Y * (int)pixelSize.Y, (int)pixelSize.X, (int)pixelSize.Y);
            base.Draw(gameTime);
        }
        public override void EnableCollisions()
        { // diz que temos colisºoes
            this.hasCollisions = true;
            this.radius = (float)Math.Sqrt(Math.Pow(size.X / 2, 2) + Math.Pow(size.Y / 2, 2));


            pixels = new Color[(int)(pixelSize.X * pixelSize.Y)];
            image.GetData<Color>(0, new Rectangle(
            (int)(currentFrame.X * size.X),
            (int)(currentFrame.Y * size.Y),
            (int)pixelSize.X,
            (int)pixelSize.Y),
            pixels, 0,
            (int)(pixelSize.X * pixelSize.Y));
        }
    }
}
