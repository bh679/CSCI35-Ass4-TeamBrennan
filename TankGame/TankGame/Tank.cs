using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GeneratedGeometry;
using Microsoft.Xna.Framework.Content;

namespace TankGame
{
    public class Tank : PhysicsObject
    {
        #region members

        //properties
        protected uint health;

        //positional 
        protected List<Vector3> Path;
        protected Vector2 aim;
        private double turnAngle = 60;

        //graphics
        Model tankBody;
        Model tankTurret;
        Model tankBarrel;
        Texture2D tankTexture;
        Matrix tankBodyTransform;
        Matrix tankTurretTransform;
        Matrix tankBarrelTransform;

        #endregion

        #region Methods

        #region Constructors

        public Tank(ContentManager cm) : base(cm) { }

        public Tank(ContentManager cm, bool _coliadable) : base(cm, _coliadable) { }

        public Tank(ContentManager cm, Vector3 pos) : base(cm, pos) { }

        public Tank(ContentManager cm, Vector3 pos, bool _coliadable) : base(cm, pos, _coliadable) { }

        #endregion

        protected override void Initialize()
        {
            base.Initialize();
            LoadContent();
        }

        protected void LoadContent()
        {
            // TODO: use this.Content to load your game content here
            tankBody = Content.Load<Model>("chbody");
            tankTurret = Content.Load<Model>("chturret");
            tankBarrel = Content.Load<Model>("chbarrel");
            tankTexture = Content.Load<Texture2D>("Skin0");
        }

        /// <summary>
        /// Repositions tank along 
        /// </summary>
        protected void Move(float dt, Vector3 TargetPosition)
        {
            Vector2 pos2d = new Vector2(base.position.X, base.position.Y);
            Vector2 targ2d = new Vector2(TargetPosition.X, TargetPosition.Y);

            Vector2 runRise = pos2d - targ2d;

            float angle = MathHelper.ToDegrees((float)Math.Atan(runRise.X / runRise.Y)) + 90;

            if (angle - base.rotation.Y > turnAngle) {
                base.rotation.Y -= (float)turnAngle * dt;
                return;
            }
            else if (angle - base.rotation.Y < -turnAngle)
            {
                base.rotation.Y += (float)turnAngle * dt;
                return;
            }
            else{
                if (angle - base.rotation.Y > 0)
                {
                    base.rotation.Y -= (float)turnAngle * dt;
                    return;
                }
                else if (angle - base.rotation.Y < 0)
                {
                    base.rotation.Y += (float)turnAngle * dt;
                    return;
                }
            }
            MoveForward(dt);
        }

        //player controls
        protected void MoveForward(float dt)
        {
            Matrix transform = Matrix.CreateTranslation(Vector3.Right) * Matrix.CreateRotationY(base.rotation.Y) * base.speed * dt;
            position += transform.Translation;
            return;
        }

        protected void MoveBackwards(float dt)
        {
            Matrix transform = Matrix.CreateTranslation(Vector3.Left) * Matrix.CreateRotationY(base.rotation.Y) * base.speed * dt;
            position += transform.Translation;
            return;
        }

        /// <summary>
        /// Calculates trajectory for shooting target
        /// </summary>
        private void Aim(Vector3 target)
        {

        }

        /// <summary>
        /// Fires projectile
        /// </summary>
        /// <param name="Target"></param>
        protected void Shoot(Vector3 Target)
        {

        }

        /// <summary>
        /// Does pathfinding to target
        /// </summary>
        /// <returns></returns>
        protected void CalculatePath(Vector3 target)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(float dt)
        {
            tankBodyTransform = Matrix.CreateRotationY(aim.X) * Matrix.CreateTranslation(base.position);
            tankTurretTransform = tankBodyTransform * Matrix.CreateRotationY(rotation.Y);
            tankBarrelTransform = tankTurretTransform;
            base.Update(dt);
        }
        
        /// <summary>
        /// 
        /// </summary>
        public override void Draw(RTSCamera camera)
        {
            // TODO: Add your drawing code here
            CopyMatrix(camera);
            DrawModel(tankBody, tankTexture, tankBodyTransform);
            DrawModel(tankTurret, tankTexture, tankTurretTransform);
            DrawModel(tankBarrel, tankTexture, tankBarrelTransform);

        }

        public void DrawHealth()
        {

        }

        #endregion
    }
}
