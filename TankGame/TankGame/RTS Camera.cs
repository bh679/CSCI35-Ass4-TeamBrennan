using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using TankGame;

namespace GeneratedGeometry
{
    /// <summary>
    /// Basic camera class supporting mouse/keyboard/gamepad-based movement.
    /// </summary>
    public class RTSCamera
    {
        /// <summary>
        /// Gets or sets the position of the camera.
        /// </summary>
        public Vector3 Position { get; set; }
        float yaw;
        float pitch;
        const float zoomOutVal = 45, zoomInVal = 10, zooms = 4;
        /// <summary>
        /// Gets or sets the yaw rotation of the camera.
        /// </summary>
        public float Yaw
        {
            get
            {
                return yaw;
            }
            set
            {
                yaw = MathHelper.WrapAngle(value);
            }
        }
        /// <summary>
        /// Gets or sets the pitch rotation of the camera.
        /// </summary>
        public float Pitch
        {
            get
            {
                return pitch;
            }
            set
            {
                pitch = MathHelper.Clamp(value, -MathHelper.PiOver2, MathHelper.PiOver2);
            }
        }

        /// <summary>
        /// Gets or sets the speed at which the camera moves.
        /// </summary>
        public float Speed { get; set; }

        /// <summary>
        /// Gets the view matrix of the camera.
        /// </summary>
        public Matrix ViewMatrix { get; private set; }
        float FOV = zoomOutVal;
        /// <summary>
        /// Gets or sets the projection matrix of the camera.
        /// </summary>
        public Matrix ProjectionMatrix {
            get
            {
                return Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(FOV), 4f / 3f, .1f, 10000.0f);
            }
            
        }

        /// <summary>
        /// Gets the world transformation of the camera.
        /// </summary>
        public Matrix WorldMatrix { get; private set; }

        /// <summary>
        /// Gets the game owning the camera.
        /// </summary>
        public GameController Game { get; private set; }

        /// <summary>
        /// Constructs a new camera.
        /// </summary>
        /// <param name="game">Game that this camera belongs to.</param>
        /// <param name="position">Initial position of the camera.</param>
        /// <param name="speed">Initial movement speed of the camera.</param>
        public RTSCamera(GameController game, Vector3 position, float speed, float _pitch)
        {
            Game = game;
            Position = position;
            Speed = speed;
            Pitch = _pitch;
            Mouse.SetPosition(200, 200);
        }

        /// <summary>
        /// Moves the camera forward using its speed.
        /// </summary>
        /// <param name="dt">Timestep duration.</param>
        public void MoveForward(float dt)
        {
            Position += new Vector3(0,0,1) * (dt * Speed);//WorldMatrix.Forward * 
        }
        /// <summary>
        /// Moves the camera right using its speed.
        /// </summary>
        /// <param name="dt">Timestep duration.</param>
        /// 
        public void MoveRight(float dt)
        {
            Position += new Vector3(1, 0, 0) * (dt * Speed);// *WorldMatrix.Right;
        }
        /// <summary>
        /// Moves the camera up using its speed.
        /// </summary>
        /// <param name="dt">Timestep duration.</param>
        /// 
        public void MoveUp(float dt)
        {
            Position += new Vector3(0, (dt * Speed), 0);
        }


        public void Zoom(bool Sniper)
        {
            if (Sniper)
            {
                for (float i = 0; i <= zooms; i++)
                {
                    if (zoomOutVal + i * (zoomInVal - zoomOutVal) / zooms == FOV)
                    {
                        FOV = zoomOutVal - (i + 1) / zooms * (zoomOutVal - zoomInVal);
                        i = zooms + 1;
                    }
                }
            }
            else
                FOV = zoomOutVal;


            if (FOV < zoomInVal)
                FOV = zoomOutVal;
            /*
                        if (FOV != zoomInVal)
                        {
                            FOV = zoomInVal;
                        }
                        else
                            FOV = zoomOutVal;*/
        }

        /// <summary>
        /// Updates the camera's view matrix.
        /// </summary>
        /// <param name="dt">Timestep duration.</param>
        public void Update(float dt)
        {
#if XBOX360
            //Turn based on gamepad input.
            Yaw += Game.GamePadState.ThumbSticks.Right.X * -1.5f * dt;
            Pitch += Game.GamePadState.ThumbSticks.Right.Y * 1.5f * dt;
#else
            //Turn based on mouse input.
            Yaw = 0;//Yaw += (200 - Game.MouseState.X) * dt * .12f;
            //Pitch = -10;//Pitch += (200 - Game.MouseState.Y) * dt * .12f;
#endif
            //Mouse.SetPosition(200, 200);

            WorldMatrix = Matrix.CreateFromAxisAngle(Vector3.Right, Pitch) * Matrix.CreateFromAxisAngle(Vector3.Up, Yaw);


            float distance = Speed * dt;
            float MSpeed = Speed / 50;
            float mouseDistance = (GameController.mouseState.X);//Game.Window.ClientBounds.Width -
            if (mouseDistance > Game.Window.ClientBounds.Width)
                MoveRight(MSpeed * dt * (mouseDistance - Game.Window.ClientBounds.Width));
            if (mouseDistance < 0)
                MoveRight(MSpeed * dt * (mouseDistance));

            mouseDistance = (GameController.mouseState.Y);//Game.Window.ClientBounds.Width -
            if (mouseDistance > Game.Window.ClientBounds.Height)
                MoveForward(MSpeed * dt * (mouseDistance - Game.Window.ClientBounds.Height));
            if (mouseDistance < 0)
                MoveForward(MSpeed * dt * (mouseDistance));
            int boost = 1;
            if (GameController.keyboardState.IsKeyDown(Keys.LeftShift))
                boost = 5;

            //Scoot the camera around depending on what keys are pressed.
            if (GameController.keyboardState.IsKeyDown(Keys.W))
                MoveForward(distance);
            if (GameController.keyboardState.IsKeyDown(Keys.S))
                MoveForward(-distance);
            if (GameController.keyboardState.IsKeyDown(Keys.A))
                MoveRight(-distance);
            if (GameController.keyboardState.IsKeyDown(Keys.D))
                MoveRight(distance);
            if (GameController.keyboardState.IsKeyDown(Keys.X))
                MoveUp(distance * boost);
            if (GameController.keyboardState.IsKeyDown(Keys.Z))
                MoveUp(-distance * boost);

            WorldMatrix = WorldMatrix * Matrix.CreateTranslation(Position);
            ViewMatrix = Matrix.Invert(WorldMatrix);
        }
    }
}
