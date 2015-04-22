﻿using Microsoft.Xna.Framework;
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
            public float velocity = 4;
            private Vector2 sourcePosition;
            private Vector2 direction;

            public Bullet(ContentManager cManager,
                          Vector2 sourcePosition,
                          float rotation)
                : base(cManager, "bullet")
            {
                this.position = sourcePosition; //11px no turret
                this.sourcePosition = sourcePosition;
                this.rotation = rotation;
                this.Scale(0.05f);
                this.direction = new Vector2((float)Math.Sin(rotation),
                                             (float)Math.Cos(rotation));
            }

            public override void Update(GameTime gameTime)
            {
                position = position + direction * velocity *
                      (float)gameTime.ElapsedGameTime.TotalSeconds;
                Console.WriteLine("Bala está no ar");
                Console.WriteLine(position);
                if ((position - sourcePosition).Length() > maxDistance)
                {
                    Console.WriteLine("Explosão da bala na maxdistance!");
                    Console.WriteLine(position);
                    this.Destroy();
                }


                base.Update(gameTime);
            }
            public override void Destroy(){
                Animated_Sprite explosion;
                explosion = new Animated_Sprite(cManager, "explosion",1,12);
                scene.AddSprite(explosion);
                explosion.SetPosition(this.position);
                Console.WriteLine("this posição1");
                Console.WriteLine(this.position);
                explosion.Scale(.3f);
                explosion.Loop = false;
                base.Destroy();
            }
        }
    }