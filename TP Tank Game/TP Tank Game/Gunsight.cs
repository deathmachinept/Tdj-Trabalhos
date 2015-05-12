using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TP_Tank_Game
{
        class Gunsight: Sprite
        {
            public float maxDistance = 2;
            public float velocity = 9;
            private Vector2 sourcePosition;
            private Vector2 direction;

            public Gunsight(ContentManager cManager,
                          Vector2 sourcePosition,
                          float rotation, float maxDistance)
                : base(cManager, "Tiger GunSight-01")
            {
                

            }

            public override void Update(GameTime gameTime)
            {

                /* position = maxDistance;

                 if ((position - sourcePosition).Length() > maxDistance)
                 {
                 this.position = sourcePosition; //11px no turret
                 this.sourcePosition = sourcePosition;
                 this.rotation = rotation;
                 this.Scale(1f);
                 this.maxDistance = maxDistance;
                 this.direction = new Vector2((float)Math.Sin(rotation),
                                              (float)Math.Cos(rotation));
                
                 }*/


                base.Update(gameTime);
            }

            /* public override void changeRange(int valor)
             {
                 maxDistance = valor;
             }*/
        }
}
