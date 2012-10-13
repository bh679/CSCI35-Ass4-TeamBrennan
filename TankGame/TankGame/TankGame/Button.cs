using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TankGame
{
    public class Button
    {
        public Vector2 position;
        public Texture2D tex;

        //Set up variables
        /// <summary>
        /// Interface optoin
        /// </summary>
        /// <param name="Position"></param>
        /// <param name="Texture"></param>
        public Button(Vector2 position, Texture2D tex) //Our constructor
        {
            this.position = position; //Position in 2D
            this.tex = tex; //Our texture to draw
        }

        public bool MouseOn()
        {
            MouseState mouse = GameController.mouseState;
            if ((mouse.X >= position.X && mouse.X <= position.X + tex.Width)
            || (mouse.Y >= position.Y && mouse.Y <= position.Y + tex.Height))
                return true;
            return false;
        }

        public void Draw(SpriteBatch batch) //Draw function, same as mousehandler one.
        {
            batch.Draw(tex, position, Color.White);
        }
    }
}

