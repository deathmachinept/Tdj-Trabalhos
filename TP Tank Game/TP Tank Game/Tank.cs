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
        enum EngineStatus
        {
            on,
            off,
            fire,
            damaged,
            destroyed,
        };
        private Sprite turret;
        private Sprite gunsight;
        private float fireInterval = 2f;
        private float fireCounter = 0f;
        private float mgfireInterval = 0.1f;
        private float mgfireCounter = 0f;
        private Vector2 centroRotacao;
        private float delta = 199.5f; // distance do turret em relaçao ao tank em pixeis
        private float deltaX = 128f;
        private float tankEixo = 148.5f;
        private Vector2 centroSprite;
        private float velocidadeMax;
        private float reverseMax;
        private float velocidade;
        private int velocidadePositiva;
        private float velocidadeEsq;
        private float turnValue;
        private float tankRot;
        private float distancia;
        private int municaoAP;
        private int municaoHE;
        private int municaoMG;
        Vector2 posicaofinal;
        EngineStatus status;

        private Vector2 tankDirection;
        public Tank(ContentManager content)
            : base(content, "Tiger I 256")
        {
            this.turret = new Sprite(content, "Tiger Turret 256");
            //this.gunsight = new Sprite(content, "Tiger GunSight-01");
            this.turret.SetRotation((float)Math.PI / 4);
           // this.gunsight.SetRotation((float)Math.PI / 4);
            this.EnableCollisions();
            centroRotacao = new Vector2(deltaX, delta); // posição final por onde deverá ser feita a rotação
            centroSprite = turret.origem; // centro da sprite 128 pixeis
            turret.origem = centroRotacao; // mudo a origem do turret para ser feita sobre o ponto real de rotação
            velocidadeMax = 0.05f;
            velocidade = 0f;
            velocidadePositiva = 0;
            turnValue = 0f;
            reverseMax = -0.02f;
            municaoAP = 40;
            municaoHE = 20;
            municaoHE = 800;
            this.tankDirection = new Vector2((float)Math.Sin(tankRot),
                             (float)Math.Cos(tankRot));
            distancia = this.origem.Y - tankEixo;
            status = EngineStatus.on;
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
            float a = (float)mpos.Y - tpos.Y; // busca a posição Y(coordenadas da camara Pixeis) do rato e subtrai a posição do tank(camara centro)
            float l = (float)mpos.X - tpos.X;


            float rot = (float)Math.Atan2(a, l);
            rot += (float)Math.PI / 2f;

            turret.SetRotation(rot);

            fireCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (fireCounter >= fireInterval &&
                mstate.LeftButton == ButtonState.Pressed)
            {
                Vector2 pos = this.position
                         + new Vector2((float)Math.Sin(rot) * size.Y / 2, (float)Math.Cos(rot) * size.Y / 2); //?
                Bullet bullet = new Bullet(cManager, pos, rot);
                scene.AddSprite(bullet);
                fireCounter = 0f;
            }

            mgfireCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;
             if (mgfireCounter >= mgfireInterval &&
                 state.IsKeyDown(Keys.Space))
             {
                 Vector2 mgpos = this.position
                          + new Vector2((float)Math.Sin(rot) * 0.1f,
                                        (float)Math.Cos(rot) * 0.1f);
                 Bullet mgbullet = new Bullet(cManager, mgpos, rot);
                 mgbullet.velocity = 5;
                 scene.AddSprite(mgbullet);
                 mgfireCounter = 0f;
             }

            /*if (state.IsKeyDown(Keys.E) && status == EngineStatus.on)
             {
                     status = EngineStatus.off;

             }
             else if (state.IsKeyDown(Keys.E) && status == EngineStatus.off)
             {
                 status = EngineStatus.on;
             }*/

            if (state.IsKeyDown(Keys.W) && status == EngineStatus.on)
                {
                    if (velocidade < 0) // se estiver andar para tras trava o tank
                    {
                        velocidade += 0.001f; 
                    }
                    else if (velocidade >= 0) // caso a velocidade seja 0 ou maior que zero está parado ou andar
                    {
                        velocidadePositiva = 1; // declara que tem uma velocidade positiva
                        velocidade += 0.0002f; // aumenta com menor velocidade
                        if (velocidade >= velocidadeMax) // se a velocidade for maior ou igual a velocidade do tank
                        {
                            velocidade = velocidadeMax; // então põe a velocidade ao maximo permitido
                        }
                    }
                    Console.WriteLine(velocidade);
                }
            else if (state.IsKeyDown(Keys.S) && status == EngineStatus.on)
                {
                    if (velocidade > 0) // Travões caso a velocidade ainda esteja superior a 0
                    {
                        velocidade += -0.001f; // Travões reduz a velocidade mais rápido que o atrito
                    }
                    else if (velocidade <= 0) // caso a velocidade seja 0 ou menor que zero/ esteja parado ou em marcha atras
                    {
                        velocidadePositiva = -1; // então o tank tem velocidade negativa
                        velocidade += -0.0001f; //acrescenta valor negativo, aumentando a velocidade (marcha atrás)
                        if (velocidade <= reverseMax) // igual a velocidade negativa a maxima velocidade permitida
                        {
                            velocidade = reverseMax;
                        }
                    }
                }

                if ((state.IsKeyUp(Keys.W)) && (state.IsKeyUp(Keys.S))) // nao input de velocidade pelo jogador, atrito do terreno
                {
                    if (velocidadePositiva == 1) //Atrito Reduz velocidade positiva
                    { //se a 
                        velocidadePositiva = 1;
                        velocidade += -0.0003f;
                        if (velocidade < 0)
                        {
                            velocidade = 0;
                            velocidadePositiva = 0;
                        }
                    }
                    else if (velocidadePositiva == -1) //caso esteja a andar para trás
                    {
                        velocidadePositiva = -1;
                        velocidade += 0.0003f; //atrito
                        if (velocidade > 0) // se neste instante a velocidade ultrapassar 0 então ele para
                        {
                            velocidade = 0;
                            velocidadePositiva = 0;
                        }
                    }

                }
                if (state.IsKeyDown(Keys.A) && velocidade != 0)
                {

                    tankRot += -0.01f;//valor de incremento
                    turnValue += -0.0001f;
                    this.SetRotation(tankRot);
                    this.tankDirection = new Vector2((float)Math.Sin(tankRot),
                     (float)Math.Cos(tankRot));
                }
                if (state.IsKeyDown(Keys.D) && velocidade != 0)
                {
                    tankRot += 0.01f;//valor de incremento
                    turnValue += 0.0001f;
                    this.SetRotation(tankRot);
                    //turret.position 
                    this.tankDirection = new Vector2((float)Math.Sin(tankRot),
                     (float)Math.Cos(tankRot));

                }

                
                //distancia = -20;
                this.position = this.position + tankDirection * velocidade;
                
                posicaofinal = new Vector2(-(float)Math.Cos(tankRot - Math.PI / 2) * distancia / Camera.ratio, (float)Math.Sin(tankRot - Math.PI / 2) * distancia / Camera.ratio);
                Console.WriteLine(distancia);

                turret.position = this.position - posicaofinal; 
                //Vector2 posturret = new Vector2(position.X -(centroRotacao.X - pixelSize.X/2)*size.X/pixelSize.X, position.Y - (centroRotacao.Y - pixelSize.Y / 2) * size.Y / pixelSize.Y); //size tamanho no mundo, pixelsize tamanho real da sprite

                //turret.SetPosition(posturret);
                turret.Update(gameTime);
            

            Camera.SetTarget(this.position);
            
            base.Update(gameTime);
        }
    }
}
