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
using TexturedQuad;
using TexturedBox;
using GeneratedGeometry;

namespace TankGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GameController : Microsoft.Xna.Framework.Game
    {
        #region memebers

        public static float gravity = 9.8f;
        public static int objectCount = 0;
        const int initialNoOfTanks = 5;
        const int initialNoOfPhysicalObjects = 10;
        public static int worldSize = 500;
        public static Random rand;

        //controls
        public static MouseState mouseState;
        public static KeyboardState keyboardState;
        public RTSCamera camera;

        //graphics
        RenderTarget2D renderTarget;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Box floor;

        Model testplane;

        //lists
        private List<AITank> tanks = new List<AITank>();
        private List<Object> objects = new List<Object>();
        private List<Button> buttons = new List<Button>();

        #endregion


        #region Initilisation

        public GameController()
        {
            mouseState = Mouse.GetState();
            keyboardState = Keyboard.GetState();
            
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            rand = new Random();
            // TODO: Add your initialization logic here
            floor = new Box(0, 0, 0, worldSize, 1, worldSize);
            camera = new RTSCamera(this, new Vector3(0, 560, 0), 5,-10);


            reset();
            base.Initialize();
        }

        public void reset()
        {
            tanks.Clear();
            objects.Clear();

            for (int i = 0; i < initialNoOfTanks; i++)
            {

                var tank = new AITank(Content);
                tanks.Add(tank);
                objects.Add(tank);
                //add tank to map at random position
                //add tank to tank list
                //add tank to object list

            }

            for (int i = 0; i < initialNoOfPhysicalObjects; i++)
            {

            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            Texture2D text = Content.Load<Texture2D>("ubuntu");
            floor.LoadTexture(text, graphics, camera.ViewMatrix, camera.ProjectionMatrix);

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            PresentationParameters pp = GraphicsDevice.PresentationParameters;
            renderTarget = new RenderTarget2D(
                                                GraphicsDevice,
                                                pp.BackBufferWidth,
                                                pp.BackBufferHeight,
                                                false,
                                                pp.BackBufferFormat,
                                                DepthFormat.Depth24);

            testplane = Content.Load<Model>("plane");

            //////////////////////////////////
            //buttons
            var button = new Button(new Vector2(0, 0), Content.Load<Texture2D>("addTank"));
            buttons.Add(button);
            // TODO: use this.Content to load your game content here
        }

        #endregion 


        #region Update

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            mouseState = Mouse.GetState();
            keyboardState = Keyboard.GetState();
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            foreach (AITank tank in tanks)
            {
                tank.UpdateAI(dt, tanks);
            }
            foreach (Object obj in objects)
                obj.Update(dt);
                

            // TODO: Add your update logic here
            floor.Update(camera.ViewMatrix);
            camera.Update(dt);
            base.Update(gameTime);
        }

        //////////////////////////////
        //buttons
        
        public void AddTank()
        {
            //add tank
            //add to tanklist
            //add to object list
        }

        public void AddPowerup()
        {
            //add new instance of power up in random location
            //add to list of powerups
            //add to list of objects

        }

        public void SelectTank()
        {
            //show tanks heath and status
            //show button to control tank

        }

        #endregion


        #region Draw

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            DrawObjects();

            GraphicsDevice.SetRenderTarget(null);

            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(0, BlendState.Opaque);
            spriteBatch.Draw(renderTarget, Vector2.Zero, Color.White);

            DrawGUI();

            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Draw models in a 3D space
        /// </summary>
        private void DrawObjects()
        {
            floor.Draw(graphics.GraphicsDevice);

            foreach (Object obj in objects)
                obj.Draw(camera);
        }

        /// <summary>
        /// Draw 2D interface objects
        /// </summary>
        private void DrawGUI()
        {
            foreach (Button button in buttons)
                button.Draw(spriteBatch);
        }

        #endregion


    }
}
