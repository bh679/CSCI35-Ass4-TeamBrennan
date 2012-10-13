using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TankGame
{
    public class PhysicsObject : Object
    {

        #region Methods

        protected bool coliadable;

        #endregion 


        #region Methods

        #region Constructors

        public PhysicsObject(ContentManager cm) : base(cm) { coliadable = false; }

        public PhysicsObject(ContentManager cm, bool _coliadable) : base(cm) { coliadable = _coliadable; }

        public PhysicsObject(ContentManager cm, Vector3 pos) : base(cm, pos) { coliadable = false; }

        public PhysicsObject(ContentManager cm, Vector3 pos, bool _coliadable) : base(cm, pos) { coliadable = _coliadable; }

        #endregion

        public override void LoadContent()
        {
            defaultModel = Content.Load<Model>("glowbox");
            base.LoadContent();
        }

        public override void Update(float dt)
        {
            base.Update(dt);
        }

        public void Gravitate()
        {
            if (!onGround())
            {
                position.Y--;
            }
        }

        protected bool onGround()
        {
            int ground = 0;

            if (position.Y > ground)
                return false;
            return true;
        }

        #endregion
    }
}
