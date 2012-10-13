using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace TankGame
{
    public class AITank : Tank
    {
        private AITank target;
        private bool AI_On = true;
        private bool Player_Controlled = false;
        private Vector3 waypoint;
        private float timer, delay;
        private float attackRadius;
        private float rotSpeed;
        private Vector2 mousePos;

        #region Constructorcs
        
        public AITank(ContentManager cm) : base(cm) { Initialize(); }

        public AITank(ContentManager cm, bool _coliadable) : base(cm, _coliadable) { Initialize(); }

        public AITank(ContentManager cm, Vector3 pos) : base(cm, pos) { Initialize();  }

        public AITank(ContentManager cm, Vector3 pos, bool _coliadable) : base(cm, pos, _coliadable) { Initialize(); }

        #endregion

        protected override void Initialize()
        {
            timer = 0f;
            rotSpeed = 100f;
            delay = 1f;
            attackRadius = 100f;

            base.Initialize();
        }

        public void UpdateTarget(float dt, List<AITank> Tanks)
        {
            target = null;
            double distanceToTarget = -1;
            double distanceToTank;
            foreach (AITank aTank in Tanks)
            {
                distanceToTank = Math.Sqrt((getPosition().X * getPosition().X - aTank.getPosition().X * aTank.getPosition().X) + (getPosition().Z * getPosition().Z - aTank.getPosition().Z * aTank.getPosition().Z));
                distanceToTank = Math.Abs(distanceToTank);
                if (distanceToTank < distanceToTarget || distanceToTarget == -1 && aTank != this)
                {
                    target = aTank;
                    distanceToTarget = distanceToTank;
                }
            }
            UpdateState(dt, distanceToTarget);
        }

        private Vector3 getPosition()
        {
            return base.position;
        }

        private void findNewTarget()
        {

        }

        public void Select()
        {
            Player_Controlled = true;
        }

        public void Unselect()
        {
            Player_Controlled = false;
        }

        public void TurnOnAI()
        {
            AI_On = true;
        }

        public void TurnOffAI()
        {
            AI_On = false;
        }

        public void UpdateAI(float dt, List<AITank> Tanks)
        {
            if (AI_On)
            {
                //MoveBackwards(dt);
                UpdateTarget(dt, Tanks);
            }
            if (Player_Controlled)
            {
                UpdateInput(dt);
            }
        }

        private void UpdateInput(float dt)
        {
            //Update Position
            if (GameController.keyboardState.IsKeyDown(Keys.Up))
            {
                base.MoveForward(dt);
            }
            else if (GameController.keyboardState.IsKeyDown(Keys.Down))
            {
                base.MoveBackwards(dt);
            }
            //Update Rotation
            if (GameController.keyboardState.IsKeyDown(Keys.Up))
            {
                base.rotation.Y += rotSpeed * dt;
            }
            else if (GameController.keyboardState.IsKeyDown(Keys.Down))
            {
                base.rotation.Y -= rotSpeed * dt;
            }

            //Update Turrent Rotation
            if (GameController.mouseState.X != mousePos.X)
            {
                base.aim.X += (GameController.mouseState.X - mousePos.X) * dt;
            }

            //Update Turret Height
            if (GameController.mouseState.Y != mousePos.Y)
            {
                base.aim.Y += (GameController.mouseState.Y - mousePos.Y) * dt;
            }

            mousePos = new Vector2(GameController.mouseState.X, GameController.mouseState.Y);
            return;
        }

        private void UpdateState(float dt, double distance)
        {
            if (target != null)
            {
                if (distance <= attackRadius && distance > 0)
                {
                    AttackState(dt);
                }
                else
                {
                    PursueState(dt);
                }
            }
            else
            {
                MoveForward(dt);
            }
            return;
        }

        private void AttackState(float dt)
        {
            if (timer == 0)
            {
                base.Shoot(target.getPosition());
            }
            else if (timer >= delay)
            {
                timer = 0;
            }
            else
            {
                timer += dt;
            }
        }

        private void PursueState(float dt)
        {
            if(target != null)
                base.Move(dt, target.Position);
        }

        private void MoveState(float dt)
        {
            base.Move(dt, waypoint);
        }
    }
}
