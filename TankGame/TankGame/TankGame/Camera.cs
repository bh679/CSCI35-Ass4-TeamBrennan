﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using TankGame;

namespace GeneratedGeometry
{
    /// <summary>
    /// Basic camera class supporting mouse/keyboard/gamepad-based movement.
    /// </summary>
    public class Camera
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
        public Camera(GameController game, Vector3 position, float speed)
        {
            Game = game;
            Position = position;
            Speed = speed;
            Mouse.SetPosition(200, 200);
        }

        /// <summary>
        /// Moves the camera forward using its speed.
        /// </summary>
        /// <param name="dt">Timestep duration.</param>
        public void MoveForward(float dt)
        {
            Position += WorldMatrix.Forward * (dt * Speed);
        }
        /// <summary>
        /// Moves the camera right using its speed.
        /// </summary>
        /// <param name="dt">Timestep duration.</param>
        /// 
        public void MoveRight(float dt)
        {
            Position += WorldMatrix.Right * (dt * Speed);
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
            Yaw += (200 - GameController.mouseState.X) * dt * .12f;
            Pitch += (200 - GameController.mouseState.Y) * dt * .12f;
#endif
            Mouse.SetPosition(200, 200);

            WorldMatrix = Matrix.CreateFromAxisAngle(Vector3.Right, Pitch) * Matrix.CreateFromAxisAngle(Vector3.Up, Yaw);


            float distance = Speed * dt;
#if XBOX360
            //Move based on gamepad input.
                MoveForward(Game.GamePadState.ThumbSticks.Left.Y * distance);
                MoveRight(Game.GamePadState.ThumbSticks.Left.X * distance);
                if (Game.GamePadState.IsButtonDown(Buttons.LeftStick))
                    MoveUp(distance);
                if (Game.GamePadState.IsButtonDown(Buttons.RightStick))
                    MoveUp(-distance);
#else

            //Scoot the camera around depending on what keys are pressed.
            if (GameController.keyboardState.IsKeyDown(Keys.E))
                MoveForward(distance);
            if (GameController.keyboardState.IsKeyDown(Keys.D))
                MoveForward(-distance);
            if (GameController.keyboardState.IsKeyDown(Keys.S))
                MoveRight(-distance);
            if (GameController.keyboardState.IsKeyDown(Keys.F))
                MoveRight(distance);
            if (GameController.keyboardState.IsKeyDown(Keys.A))
                MoveUp(distance*5);
            if (GameController.keyboardState.IsKeyDown(Keys.Z))
                MoveUp(-distance*5);
#endif

            WorldMatrix = WorldMatrix * Matrix.CreateTranslation(Position);
            ViewMatrix = Matrix.Invert(WorldMatrix);
        }
    }
}
