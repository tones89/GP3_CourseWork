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

namespace GP3_Coursework
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Camera camera = new Camera();
        Enemy enemy = new Enemy(5, 1, 2);
        //Player aPlayer = new Player(100);
        KeyboardState keyBoardState = Keyboard.GetState();
        KeyboardState previousKeyBoardState = Keyboard.GetState();

        #region model vars
        Matrix playerWorld;
        Model mdl_Player;
        Vector3 playerPos = Vector3.Zero;
        float playerRot = 0.0f;
        Vector3 playerVelocity = Vector3.Zero;
        Texture2D skyBack;

        Model mdl_Enemy;
        Matrix enemyWorld;
        Vector3 enemyPos;
        Model CannonBall;
        Matrix cannonWorld;
<<<<<<< HEAD
=======
        Vector3 playerPos;
>>>>>>> 504ead635f4add52cb3029999ab0047cde2f696d
        Matrix enemyRot;
        
        #endregion

        public Game1()
        {
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
            
           playerWorld = Matrix.Identity;
            enemyWorld = Matrix.Identity;
            enemyRot = Matrix.Identity;
<<<<<<< HEAD

            playerPos = Vector3.Zero;
        //    playerVelocity = Vector3.Zero;
=======
            
>>>>>>> 504ead635f4add52cb3029999ab0047cde2f696d
            base.Initialize();
            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            
            
            // Create  new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            mdl_Player = Content.Load<Model>(".\\Models\\Pirate Ship");  
            skyBack = Content.Load<Texture2D>(".\\Models\\sky");   
            CannonBall = Content.Load<Model>(".\\Models\\Ball2");
            mdl_Enemy = Content.Load<Model>(".\\Models\\Pirate Ship");
          

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();


            float elapsed = (float)gameTime.ElapsedGameTime.Seconds;


            ArtificialIntellignce();
            ChasePlayer(playerWorld);
            camera.Update(playerWorld);
           // camera.Update(playerWorld);
           MovePlayer();
         //  playerVelocity *= 0.95f; 
           
            //playerPos = playerWorld.Translation;

            

           playerPos = playerWorld.Translation;

>>>>>>> 504ead635f4add52cb3029999ab0047cde2f696d
            base.Update(gameTime);
        }



        void ArtificialIntellignce()
        {

            Vector3 speed = new Vector3(0.1f, 0f, 0f);
            enemyRot.Forward.Normalize();
            enemyRot.Up.Normalize();
            enemyRot.Right.Normalize();
<<<<<<< HEAD
            
            Vector3 desiredPosition;
            //float thing =(float) Math.Atan2(enemyPos.X - playerPos.X, enemyPos.Y - playerPos.Y);
            
            desiredPosition = Vector3.Transform(playerPos, enemyWorld);
            
            enemyPos = Vector3.SmoothStep(enemyPos, desiredPosition, .15f);
            enemyWorld *= Matrix.CreateTranslation(enemyPos);

=======
            enemyWorld.Right.Normalize();
            enemyWorld.Up.Normalize();
            Vector3 desiredPosition;
            float thing =(float) Math.Atan2(enemyPos.X - playerPos.X, enemyPos.Y - playerPos.Y);

            //enemyPos = Vector3.Lerp(enemyWorld.Up, playerWorld.Up, thing);
          //  enemyWorld = Matrix.CreateFromAxisAngle(enemyWorld.Up, thing);
            desiredPosition = Vector3.Transform(speed, enemyWorld);
            enemyPos = Vector3.SmoothStep(enemyPos, desiredPosition, .15f);
            enemyWorld = Matrix.CreateTranslation(enemyPos) * Matrix.CreateFromAxisAngle(enemyWorld.Up, thing) *enemyWorld ;
>>>>>>> 504ead635f4add52cb3029999ab0047cde2f696d

        }

         void ChasePlayer(Matrix objectToChase)
        {
            enemyWorld.Right.Normalize();
            enemyWorld.Up.Normalize();

            Vector3 desiredPosition;
            Vector3 offset = new Vector3(2, 0, 10);

            objectToChase.Right.Normalize();
            objectToChase.Up.Normalize();

<<<<<<< HEAD
            //float distance = Vector3.Distance(enemyPos, playerPos);
=======
            
>>>>>>> 504ead635f4add52cb3029999ab0047cde2f696d
            desiredPosition = Vector3.Transform(offset, objectToChase);

            enemyPos = Vector3.SmoothStep(enemyPos, desiredPosition, .2f);

            enemyWorld = Matrix.CreateTranslation(enemyPos);
        }

        
        
        //Method used to handle input of player and move them around the environment. 
        public void MovePlayer()
        {
            Vector3 velocityToAdd = Vector3.Zero;

            velocityToAdd.X = -(float)Math.Sin(playerRot);
            velocityToAdd.Y -= (float)Math.Sin(playerRot);

            if (keyBoardState.IsKeyDown(Keys.Enter) && previousKeyBoardState.IsKeyUp(Keys.Enter))
            {
                camera.SwitchCameraMode();
            }
            if (keyBoardState.IsKeyDown(Keys.Space) && previousKeyBoardState.IsKeyUp(Keys.Space))
            {
                FireCannon();
            }
 
            //Rotate Cube along its Up Vector
            if (keyBoardState.IsKeyDown(Keys.X))
            {
                playerWorld = Matrix.CreateFromAxisAngle(Vector3.Up, .02f) * playerWorld;
            }
            if (keyBoardState.IsKeyDown(Keys.Z))
            {
                
                playerWorld = Matrix.CreateFromAxisAngle(Vector3.Up, -.02f) * playerWorld;
            }
 
            //Move Cube Forward, Back, Left, and Right
            if (keyBoardState.IsKeyDown(Keys.Up))
            {
                playerWorld *= Matrix.CreateTranslation(playerWorld.Forward);
            }
            if (keyBoardState.IsKeyDown(Keys.Down))
            {
                playerWorld *= Matrix.CreateTranslation(playerWorld.Backward);
            }
            previousKeyBoardState = keyBoardState;
       }
        
        
        private void FireCannon()
        {

            for (int i = 0; i < 100; i++)
            {
                float variant = MathHelper.SmoothStep(10f, 8f, 2f);
                enemyWorld *= Matrix.CreateTranslation(enemyWorld.Left)/variant;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            Draw2D();
            DrawModel(mdl_Player,playerWorld);
            DrawModel(mdl_Enemy, enemyWorld);
            Matrix scaleMat =  Matrix.CreateScale(-100f);
           cannonWorld =  cannonWorld* scaleMat;
            DrawModel(CannonBall, cannonWorld);
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

     

        private void Draw2D()
        {
            spriteBatch.Begin();

            int i_Height = GraphicsDevice.PresentationParameters.BackBufferHeight;
            int i_Width = GraphicsDevice.PresentationParameters.BackBufferWidth;
            Rectangle rec_BackRect = new Rectangle(0, 0, i_Width, i_Height);

            spriteBatch.Draw(skyBack, rec_BackRect, Color.White);

            spriteBatch.End();
    
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            //graphics.GraphicsDevice.DepthStencilState.DepthBufferEnable = true;
             
        }

        private void DrawModel(Model model, Matrix worldMatrix)
        {
            Matrix[] modelTransforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(modelTransforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    
                    effect.EnableDefaultLighting();
                    effect.World = modelTransforms[mesh.ParentBone.Index] * worldMatrix;
                    effect.View = camera.viewMatrix;
                    effect.Projection = camera.projectionMatrix;
                }
                mesh.Draw();

            }
        }
    }
}