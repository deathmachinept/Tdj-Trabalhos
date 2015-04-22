using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TP_Tank_Game
{
    class Tank : Sprite
    {
        private Sprite turret;
        private float fireInterval = 2f;
        private float fireCounter = 0f;
        private float mgfireInterval = 0.1f;
        private float mgfireCounter = 0f;
        private int delta = 75; // distance do turret em relaçao ao tank em pixeis
        private float velocidadeMax;
        private float reverseMax;
        private float velocidade;
        private int velocidadePositiva;
        public Tank(ContentManager content)
            : base(content, "tank_body")
        {
            this.turret = new Sprite(content, "tank_turret");
            this.turret.SetRotation((float)Math.PI / 4);
            this.EnableCollisions();
            turret.origem.Y = delta;
            turret.origem = new Vector2(turret.origem.X, turret.origem.Y);
            velocidadeMax = 0.05f;
            velocidade = 0f;
            velocidadePositiva = 0;
            reverseMax = -0.02f;
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);//porque este base.draw
            turret.Draw(gameTime);
        }
        public override void SetScene(Scene s)
        {
            this.scene = s;
            turret.SetScene(s);
        }


        public override void Update(GameTime gameTime)
        {

            MouseState mstate = Mouse.GetState();
            Point mpos = mstate.Position; // posição em pixeis
            KeyboardState state = Keyboard.GetState();


            Vector2 tpos = Camera.WorldPoint2Pixels(position);
            float a = (float)mpos.Y - tpos.Y;
            float l = (float)mpos.X - tpos.X;
            float rot = (float)Math.Atan2(a, l);
            rot += (float)Math.PI / 2f;
            turret.SetRotation(rot);

            fireCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (fireCounter >= fireInterval &&
                mstate.LeftButton == ButtonState.Pressed)
            {
                Vector2 pos = this.position
                         + new Vector2((float)Math.Sin(rot) * size.Y / 2, (float)Math.Cos(rot) * size.Y / 2);
                Console.WriteLine(pos);
                Console.WriteLine("xxPosicao dentro do disparo inicialxx");
                Bullet bullet = new Bullet(cManager, pos, rot);
                scene.AddSprite(bullet);
                fireCounter = 0f;
            }
            /*
            mgfireCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (mgfireCounter >= mgfireInterval &&
                state.IsKeyDown(Keys.Space))
            {
                Vector2 mgpos = this.position
                         + new Vector2((float)Math.Sin(rot) * size.Y / 2,
                                       (float)Math.Cos(rot) * size.Y / 2);
                Bullet mgbullet = new Bullet(cManager, mgpos, rot);
                mgbullet.velocity = 4;
                scene.AddSprite(mgbullet);
                mgfireCounter = 0f;
            }*/



            if (state.IsKeyDown(Keys.W)) {
                if (velocidade < 0) // Travões caso a velocidade ainda esteja superior a 0
                {
                    velocidade += 0.001f; // Travões reduz a velocidade mais rápido que o atrito
                }
                else if (velocidade >= 0) // caso a velocidade seja 0 ou menor que zero/ esteja parado ou em marcha atras
                {
                    velocidadePositiva = 1;
                    velocidade += 0.0002f;
                    if (velocidade >= velocidadeMax)
                    {
                        velocidade = velocidadeMax;
                    }
                }
                /*Sprite other;
                Vector2 colPosition;
                if(scene.collides(this, out other, out colPosition))
                {
                    this.position.Y += -0.01f;
                }*/
                this.position.Y += velocidade;
            }
            else if (state.IsKeyDown(Keys.S))
            {
                if (velocidade > 0) // Travões caso a velocidade ainda esteja superior a 0
                {
                    velocidade += -0.001f; // Travões reduz a velocidade mais rápido que o atrito
                }
                else if (velocidade <= 0) // caso a velocidade seja 0 ou menor que zero/ esteja parado ou em marcha atras
                {
                    velocidadePositiva = -1;
                    velocidade += -0.0001f; //acrescenta valor negativo de velocidade
                    if (velocidade < reverseMax)
                    {
                        velocidade = reverseMax;
                    }
                }
                this.position.Y += velocidade;
            }
            if ((state.IsKeyUp(Keys.W)) && (state.IsKeyUp(Keys.S))) // nao input de velocidade pelo jogador
            {
                if (velocidadePositiva == 1)
                { //se a 
                    velocidadePositiva = 1;
                    velocidade += -0.0003f;
                    if (velocidade < 0)
                    {
                        velocidade = 0;
                        velocidadePositiva = 0;
                    }
                }
                else if (velocidadePositiva == -1)
                {
                    velocidadePositiva = -1;
                    velocidade += 0.0003f;
                    if (velocidade > 0)
                    {
                        velocidade = 0;
                        velocidadePositiva = 0;
                    }
                }
                this.position.Y += velocidade;
            }
               /* Sprite other;
                Vector2 colPosition;
                if (scene.collides(this, out other, out colPosition))
                {
                    this.position.Y += 0.01f;
                }*/


            Vector2 posturret = new Vector2 (position.X,position.Y - (delta-pixelSize.Y/2)*size.Y/pixelSize.Y); // size tamanho no mundo, pixelsize tamanho real da sprite
            
            turret.SetPosition(posturret);
            Camera.SetTarget(this.position);
            turret.Update(gameTime);
            base.Update(gameTime);
        }
    }
}
