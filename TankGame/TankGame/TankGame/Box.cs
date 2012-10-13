using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Text;
using TexturedQuad;

namespace TexturedBox
{
    class Box
    {
        public Quad[] quad = new Quad[6];

        protected float width,
               height,
               depth;
        public Vector3 Pos;
        public Vector3 Position
        {
            get { return Pos; }
            set { Pos = value; }
        }
        private Vector3 scale;
        public Vector3 Scale
        {
            get { return scale; }
            set { scale = value; }
        }
        int up = -1;

        VertexDeclaration vertexDeclaration;

        Texture2D[] texture = new Texture2D[3];
        bool twoTextures;
        BasicEffect quadEffect;

        public Box()
        {
            Pos = Vector3.Zero;
            width = 1;
            height = 1;
            depth = 1;
            Initialize();
        }

        public Box(bool _insideout)
        {
            if (_insideout)
                up = 1;
            else
                up = -1;
            Pos = Vector3.Zero;
            width = 1;
            height = 1;
            depth = 1;
            Initialize();
        }

        public Box(float x, float y, float z)
        {
            Pos.X = x;
            Pos.Y = y;
            Pos.Z = z;
            Initialize();
        }

        public Box(float x, float y, float z, bool _insideout)
        {
            if (_insideout)
                up = 1;
            else
                up = -1;
            Pos.X = x;
            Pos.Y = y;
            Pos.Z = z;
            Initialize();
        }

        public Box(float x, float y, float z, float _width, float _height, float _depth)
        {
            Pos.X = x;
            Pos.Y = y;
            Pos.Z = z;
            width = _width;
            height = _height;
            depth = _depth;
            Initialize();
        }

        public Box(float x, float y, float z, float _width, float _height, float _depth, bool _insideout)
        {
            if (_insideout)
                up = 1;
            else
                up = -1;
            Pos.X = x;
            Pos.Y = y;
            Pos.Z = z;
            width = _width;
            height = _height;
            depth = _depth;
            Initialize();
        }

        protected void Initialize()
        {
            //                    Position x  y  z                  normal  x  y  z                    Up   x  y  z
            resetQuads();

            texture[0] = null;
            texture[1] = null;
           //base.Initialize();
        }


        protected void resetQuads(float newwidth, float newheight, float newdepth)
        {
            //scale = new Vector3(newwidth, newheight, newdepth);
            quad[0] = new Quad(new Vector3(Pos.X, Pos.Y, Pos.Z - newdepth / 2), new Vector3(0, 0, up), new Vector3(0, 1, 0), newwidth, newheight);//back
            quad[1] = new Quad(new Vector3(Pos.X - newwidth / 2, Pos.Y, Pos.Z), new Vector3(up, 0, 0), new Vector3(0, 1, 0), newdepth, newheight);//left
            quad[2] = new Quad(new Vector3(Pos.X + newwidth / 2, Pos.Y, Pos.Z), new Vector3(-up, 0, 0), new Vector3(0, 1, 0), newdepth, newheight);//right
            quad[3] = new Quad(new Vector3(Pos.X, Pos.Y + newheight / 2, Pos.Z), new Vector3(0, -up, 0), new Vector3(0, 0, 1), newwidth, newdepth);//top
            quad[4] = new Quad(new Vector3(Pos.X, Pos.Y - newheight / 2, Pos.Z), new Vector3(0, up, 0), new Vector3(0, 0, 1), newwidth, newdepth);//bottom
            quad[5] = new Quad(new Vector3(Pos.X, Pos.Y, Pos.Z + newdepth / 2), new Vector3(0, 0, -up), new Vector3(0, 1, 0), newwidth, newheight);//behind
        }

        protected void resetQuads()
        {
            scale = new Vector3(width, height, depth);
            quad[0] = new Quad(new Vector3(Pos.X, Pos.Y, Pos.Z - depth / 2), new Vector3(0, 0, up), new Vector3(0, 1, 0), width, height);//back
            quad[1] = new Quad(new Vector3(Pos.X - width / 2, Pos.Y, Pos.Z), new Vector3(up, 0, 0), new Vector3(0, 1, 0), depth, height);//left
            quad[2] = new Quad(new Vector3(Pos.X + width / 2, Pos.Y, Pos.Z), new Vector3(-up, 0, 0), new Vector3(0, 1, 0), depth, height);//right
            quad[3] = new Quad(new Vector3(Pos.X, Pos.Y + height / 2, Pos.Z), new Vector3(0, -up, 0), new Vector3(0, 0, 1), width, depth);//top
            quad[4] = new Quad(new Vector3(Pos.X, Pos.Y - height / 2, Pos.Z), new Vector3(0, up, 0), new Vector3(0, 0, 1), width, depth);//bottom
            quad[5] = new Quad(new Vector3(Pos.X, Pos.Y, Pos.Z + depth / 2), new Vector3(0, 0, -up), new Vector3(0, 1, 0), width, height);//behind
        }

        public void LoadTexture(Texture2D _texture, GraphicsDeviceManager graphics, Matrix View, Matrix Projection)
        {
            //loads texture
            texture[0] = _texture;
            quadEffect = new BasicEffect(graphics.GraphicsDevice);
            quadEffect.EnableDefaultLighting();

            quadEffect.World = Matrix.Identity;
            quadEffect.View = View;
            quadEffect.Projection = Projection;
            quadEffect.TextureEnabled = true;
            quadEffect.Texture = texture[0];

            LoadContent();
        }

        public void LoadTexture(Texture2D text1, Texture2D text2, Texture2D text3, GraphicsDeviceManager graphics, Matrix View, Matrix Projection)
        {
            //loads texture
            texture[0] = text1;
            texture[1] = text2;
            texture[2] = text3;
            twoTextures = true;
            quadEffect = new BasicEffect(graphics.GraphicsDevice);
            quadEffect.EnableDefaultLighting();

            quadEffect.World = Matrix.Identity;
            quadEffect.View = View;
            quadEffect.Projection = Projection;
            quadEffect.TextureEnabled = true;
            quadEffect.Texture = texture[0];

            LoadContent();
        }

        protected void LoadContent()
        {

            vertexDeclaration = new VertexDeclaration(new VertexElement[]
                {
                    new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                    new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
                    new VertexElement(24, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0)
                }
            );
        }

        public bool InRange(Vector2 Pos1,Vector2 Pos2)
        {
            if(((Pos.X > Pos1.X && Pos.X < Pos2.X ) || (Pos.X < Pos1.X && Pos.X > Pos2.X)) 
            && ((Pos.Y > Pos1.Y && Pos.Y < Pos2.Y) || (Pos.Y < Pos1.Y && Pos.Y > Pos2.Y)))
                return true;
            return false;
        }

        public void Update(Matrix View)
        {
            quadEffect.View = View;
        }

        public void Draw(GraphicsDevice GraphicsDevice)
        {
            foreach (EffectPass pass in quadEffect.CurrentTechnique.Passes)
            {
                if (twoTextures == false)
                {

                    pass.Apply();
                    for (int i = 0; i < 6; i++)
                        GraphicsDevice.DrawUserIndexedPrimitives
                            <VertexPositionNormalTexture>(
                            PrimitiveType.TriangleList,
                            quad[i].Vertices, 0, 4,
                            quad[i].Indexes, 0, 2);
                }
                else
                {

                    quadEffect.Texture = texture[0];
                    pass.Apply();
                    for (int i = 0; i < 3; i++)
                        GraphicsDevice.DrawUserIndexedPrimitives
                            <VertexPositionNormalTexture>(
                            PrimitiveType.TriangleList,
                            quad[i].Vertices, 0, 4,
                            quad[i].Indexes, 0, 2);

                    quadEffect.Texture = texture[1];
                    pass.Apply();
                    for (int i = 3; i < 5; i++)
                        GraphicsDevice.DrawUserIndexedPrimitives
                            <VertexPositionNormalTexture>(
                            PrimitiveType.TriangleList,
                            quad[i].Vertices, 0, 4,
                            quad[i].Indexes, 0, 2);

                    quadEffect.Texture = texture[2];
                    pass.Apply();
                    for (int i = 5; i < 6; i++)
                        GraphicsDevice.DrawUserIndexedPrimitives
                            <VertexPositionNormalTexture>(
                            PrimitiveType.TriangleList,
                            quad[i].Vertices, 0, 4,
                            quad[i].Indexes, 0, 2);
                }

            }
        }

    }
}
