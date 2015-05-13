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
        private Animated_Sprite turret;
        private Sprite gunsight;
        public bool ai;
        private float fireInterval = 2f;
        private float fireCounter = 0f;
        private float mgfireInterval = 0.1f;
        private float mgfireCounter = 0f;
        private Vector2 centroRotacao;
        private float delta = 199.5f; // distance do turret em relaçao ao tank em pixeis
        private float deltaX = 129f;
        private float tankEixo = 127f;
        private Vector2 centroSprite;
        private float velocidadeMax;
        private float reverseMax;
        private float velocidade;
        private int velocidadePositiva;
        private float velocidadeEsq;
        private float turnValue;
        private float tankRot;
        private float distancia;
        private float distanciaTurret;
        private int municaoAP;
        private int municaoHE;
        private int municaoMG;
        private float range;
        private Vector2 pos;
        Vector2 posicaofinal;
        private int oldmousevalue;
        private int tracer;
        EngineStatus status;


        private Vector2 tankDirection;
        public Tank(ContentManager content)
            : base(content, "Tiger I 256")
        {
            
            this.turret = new Animated_Sprite(cManager, "TurretFire Ani", 1, 6);
            this.turret.SetRotation((float)Math.PI / 4);
            this.turret.Loop = false;
            this.turret.Scl(1.4f);
            this.gunsight = new Sprite(content, "Tiger GunSight-01");
            this.gunsight.SetRotation((float)Math.PI / 4);
            this.EnableCollisions();
            centroRotacao = new Vector2(deltaX, delta); // posição final por onde deverá ser feita a rotação
            turret.origem = centroRotacao; // mudo a origem do turret para ser feita sobre o ponto real de rotação
            velocidadeMax = 0.05f;
            velocidade = 0f;
            velocidadePositiva = 0;
            turnValue = 0f;
            reverseMax = -0.02f;
            range = 2;
            municaoAP = 40;
            municaoHE = 20;
            municaoHE = 800;
            oldmousevalue = 0;
            tracer = 0;
            ai = true;

            this.tankDirection = new Vector2((float)Math.Sin(tankRot),
                             (float)Math.Cos(tankRot));
            distancia = this.origem.Y - tankEixo;
            status = EngineStatus.off;
        }
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);//porque este base.draw
            turret.Draw(gameTime);
            gunsight.Draw(gameTime);
        }
        public override void SetScene(Scene s)
        {
            this.scene = s;
            turret.SetScene(s);
            gunsight.SetScene(s);
        }


        public override void Update(GameTime gameTime)
        {

            MouseState mstate = Mouse.GetState();
            Point mpos = mstate.Position; // posição em pixeis
            KeyboardState state = Keyboard.GetState();
            int mousevalue = mstate.ScrollWheelValue;
            
            Vector2 tpos = Camera.WorldPoint2Pixels(position);
            float a = (float)mpos.Y - tpos.Y; // busca a posição Y(coordenadas da camara Pixeis) do rato e subtrai a posição do tank(camara centro)
            float l = (float)mpos.X - tpos.X;
            
            
            float rot = (float)Math.Atan2(a, l);
            rot += (float)Math.PI / 2f;

            turret.SetRotation(rot);
            gunsight.SetRotation(rot);

            if (mousevalue != oldmousevalue)
            {
                if (mousevalue > oldmousevalue)
                {
                    if (range < 6)
                    {
                        range += 0.2f;
                        oldmousevalue = mousevalue;

                    }
                }
                else
                {
                    if (range > 0.5f)
                    {
                        range -= 0.2f;
                        oldmousevalue = mousevalue;

                    }
                }
            }
            /*gunsight.SetPosition(this.turret.position + new Vector2((float)Math.Sin(rot) * (range + 15 / Camera.ratio),
                    (float)Math.Cos(rot) * (range + 15 / Camera.ratio)));*/

            gunsight.SetPosition(this.turret.position + new Vector2((float)Math.Sin(rot) * (range +150 / Camera.ratio),
        (float)Math.Cos(rot) * (range +150 / Camera.ratio)));
            
            fireCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (fireCounter >= fireInterval &&
                mstate.LeftButton == ButtonState.Pressed)
            {

                this.turret.numLoops = 1;
                pos = this.turret.position + new Vector2((float)Math.Cos(rot - Math.PI / 2) * 150 / Camera.ratio,
                    (float)Math.Sin(rot - Math.PI / 2) * -150 / Camera.ratio);
                Bullet bullet = new Bullet(cManager, pos, rot,range);
                scene.AddSprite(bullet);
                fireCounter = 0f;

            }
            turret.Loop = false;
            mgfireCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;
             if (mgfireCounter >= mgfireInterval &&
                 state.IsKeyDown(Keys.Space))
             {
                 Vector2 mgpos = this.turret.position;

                 float x1 = (float)Math.Cos(Math.PI / 2 - rot)* 45f/ Camera.ratio;
                 float y1 = (float)Math.Sin(Math.PI / 2 - rot)* 45f / Camera.ratio;
                 float x2 = (float)Math.Sin(Math.PI / 2 - rot)* 12f / Camera.ratio;
                 float y2 = -(float)Math.Cos(Math.PI / 2 - rot)* 12f / Camera.ratio;

                 mgpos.X =(mgpos.X + x1+x2);
                 mgpos.Y =(mgpos.Y + y1+y2);
                 float mgrange = range + 0.6f;
                 Bullet mgbullet = new Bullet(cManager, mgpos, rot,mgrange);
                 mgbullet.velocity = 5;
                 scene.AddSprite(mgbullet);
                /* if (tracer == 0)
                 {
                    
                     tracer++;
                 }
                 else if (tracer == 4)
                 {
                     tracer = 0;
                 */
                 mgfireCounter = 0f;
             }

            if (state.IsKeyDown(Keys.E) && status == EngineStatus.on)
             {
                     status = EngineStatus.off;

             }
             else if (state.IsKeyDown(Keys.E) && status == EngineStatus.off)
             {
                 status = EngineStatus.on;
             }

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
           

                if (((state.IsKeyUp(Keys.W)) && (state.IsKeyUp(Keys.S))) || status == EngineStatus.off) // nao input de velocidade pelo jogador, atrito do terreno
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

                
                this.position = this.position + tankDirection * velocidade;
                
                posicaofinal = new Vector2(-(float)Math.Cos(tankRot - Math.PI / 2) * distancia / Camera.ratio, (float)Math.Sin(tankRot - Math.PI / 2) * distancia / Camera.ratio);


                turret.position = this.position - posicaofinal;
               

                turret.Update(gameTime);
                gunsight.Update(gameTime);
            Camera.SetTarget(this.position);
            base.Update(gameTime);
            
        }
    }
}
 // tamanho do canhao (vertical, ja que na horizontal esta centrado)
                //float s = 56f;
   // obter posicao onde esta o turret
               // Vector2 pos = turret.Position;
  // usar o angulo de rotacao e o tamanho do canhao para calcular o tamanho da deslocacao X e Y (pensa no canhao como sendo a hipotenusa do triangulo de pitagoras, aqui calculo os dois catetos)
               // Vector2 delta = new Vector2( (float)Math.Cos(rot - Math.PI/2) * s / Camera.Ratio, -(float)Math.Sin(rot - Math.PI/2) * s / Camera.Ratio);
// somar esses catetos à posição
               // pos += delta;

// bala sai dessa posicao
               // CannonBullet bullet = new CannonBullet(content);
               // bullet.SetPosition(pos);