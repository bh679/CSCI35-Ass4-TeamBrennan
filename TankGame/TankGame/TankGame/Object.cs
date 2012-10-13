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
    public class Object
    {
        #region Members

        protected int id;
        public int Id
        {
            get { return id; }
        }

        //properties
        private bool alive;
        public bool isAlive
        {
            get { return alive; }
        }

        //positional memberrs
        protected Vector3 position;
        public Vector3 Position
        {
            get {return position;}   
        }
        protected float speed;
        protected Vector3 rotation;

        //model members
        protected ContentManager Content;
        protected Model defaultModel;
        protected float scale = 0.1f;
        protected Matrix Scale;
        protected Matrix world;
        protected Matrix view;
        protected Matrix projection;
        Texture2D defaultTex;


        #endregion

        #region Methods

        /// <summary>
        /// Makes Object at random position.
        /// </summary>
        /// <param name="ContentManager"></param>
        public Object(ContentManager cm)
        {
            Content = cm;
            position = Vector3.Down;
            Initialize();
        }

        /// <summary>
        /// Creates Obejct at position.
        /// </summary>
        /// <param name="ContentManager"></param>
        /// <param name="Position"></param>
        public Object(ContentManager cm, Vector3 pos)
        {
            Content = cm;
            position = pos;
            Initialize();
            Console.WriteLine(position);
        }

        protected virtual void Initialize()
        {
            id = GameController.objectCount++;
            if (position == Vector3.Down)
                position = new Vector3(GameController.rand.Next(-GameController.worldSize / 2, GameController.worldSize / 2), 0, GameController.rand.Next(-GameController.worldSize / 2, GameController.worldSize/2));
            rotation = new Vector3(0, 0, 0);
            speed = 5;
            Scale = Matrix.CreateScale(0.1f);

            LoadContent();
        }

        public virtual void LoadContent()
        {
            defaultModel = Content.Load<Model>("cube");
        }

        public virtual void Update(float dt)
        {
            world = Matrix.CreateTranslation(position * (1/scale)) * Scale;//camera.WorldMatrix;
        }

        /// <summary>
        /// Sets alive to false
        /// </summary>
        public void Destroy()
        {
            alive = false;
        }

        public virtual void Draw(RTSCamera camera)
        {
            CopyMatrix(camera);
            DrawModel(defaultModel,defaultTex,world);
        }

        protected void CopyMatrix(RTSCamera camera)
        {
            view = camera.ViewMatrix;
            projection = camera.ProjectionMatrix;
        }

        /// <summary>
        /// Draws Model passed in
        /// </summary>
        /// <param name="Model"></param>
        /// <param name="Texture"></param>
        /// <param name="Tranfsormation"></param>
        protected void DrawModel(Model m, Texture2D tex, Matrix tranfsorm)
        {
            Matrix[] transforms = new Matrix[m.Bones.Count];
            m.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in m.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.Texture = tex;
                    effect.TextureEnabled = true;

                    effect.View = view;
                    effect.Projection = projection;
                    effect.World = world * transforms[mesh.ParentBone.Index];
                }
                mesh.Draw();
            }
        }

        #endregion
    }
}
