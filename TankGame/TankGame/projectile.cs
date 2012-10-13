using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace TankGame
{
    public class projectile : PhysicsObject
    {
        #region members

        private Vector3 angle;

        #endregion

        #region Methods

        public projectile(ContentManager cm, Vector3 position, Vector3 angle)
            : base(cm, position)
        {
            this.angle = angle;
            speed = 10;
        }

        public override void Update(float dt)
        {
            if (onGround())
                Destroy();
            MoveForward(dt);
            base.Update(dt);
        }

        //player controls
        protected void MoveForward(float dt)
        {
            Matrix transform = Matrix.CreateTranslation(Vector3.Right) * Matrix.CreateRotationY(base.rotation.Y) * Matrix.CreateRotationX(base.rotation.X) * Matrix.CreateRotationZ(base.rotation.Z) * speed * dt;
            position += transform.Translation;
            return;
        }


        #endregion
    }
}
