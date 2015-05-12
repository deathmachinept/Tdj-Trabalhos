using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TP_Tank_Game
{
        class Bullet : Sprite
        {
            public float maxDistance = 2;
            public float velocity = 9;
            private Vector2 sourcePosition;
            private Vector2 direction;

            public Bullet(ContentManager cManager,
                          Vector2 sourcePosition,
                          float rotation, float maxDistance)
                : base(cManager, "bullet")
            {
                this.position = sourcePosition; //11px no turret
                this.sourcePosition = sourcePosition;
                this.rotation = rotation;
                this.Scale(0.05f);
                this.maxDistance = maxDistance;
                this.direction = new Vector2((float)Math.Sin(rotation),
                                             (float)Math.Cos(rotation));
            }

            public override void Update(GameTime gameTime){

                position = position + direction * velocity *
                      (float)gameTime.ElapsedGameTime.TotalSeconds;

                

                if ((position - sourcePosition).Length() > maxDistance) 
                {
                    
                    float valor = (position - sourcePosition).Length();
                    Console.WriteLine(valor);
                    this.Destroy();
                }


                base.Update(gameTime);
            }

           /* public override void changeRange(int valor)
            {
                maxDistance = valor;
            }*/
            public override void Destroy(){
                Animated_Sprite explosion;// adicionar loops

                explosion = new Animated_Sprite(cManager, "BulletHit", 1, 14);
                scene.AddSprite(explosion);
                explosion.SetPosition(this.position);
                explosion.Scale(0.5f);
                explosion.SetRotation(rotation);
                explosion.Loop = false;
                
                base.Destroy();
                
            }
        }
    }
