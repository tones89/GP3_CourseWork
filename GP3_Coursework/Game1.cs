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
using System.IO;

namespace GP3_Coursework
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        ModelManager mdl_Manager;

        #region User Defined Variables
        //------------------------------------------
        // Added for use with fonts
        //------------------------------------------
        SpriteFont fontToUse;

        //--------------------------------------------------
        // Added for use with playing Audio via Media player
        //--------------------------------------------------
        Song bkgMusic;
        String songInfo;
        //--------------------------------------------------
        //Set the sound effects to use
        //--------------------------------------------------
        SoundEffectInstance tardisSoundInstance;
        SoundEffect tardisSound;

        // Set the 3D model to draw.
        private Model mdlTardis;

        // The aspect ratio determines how to scale 3d to 2d projection.
        private float aspectRatio;

        // Set the position of the model in world space, and set the rotation.
        private Vector3 mdlPosition = Vector3.Zero;
        private float mdlRotation = 0.0f;
        private Vector3 mdlVelocity = Vector3.Zero;

        // Set the position of the camera in world space, for our view matrix.
        private Vector3 cameraPosition = new Vector3(0.0f, 5.0f, 15.0f);

        private void MoveModel()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            // Create some velocity if the right trigger is down.
            Vector3 mdlVelocityAdd = Vector3.Zero;

            // Find out what direction we should be thrusting, using rotation.
            mdlVelocityAdd.X = -(float)Math.Sin(mdlRotation);
            mdlVelocityAdd.Z = -(float)Math.Cos(mdlRotation);

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                // Rotate left.
                mdlRotation -= -1.0f * 0.10f;
            }

            if (keyboardState.IsKeyDown(Keys.Right))
            {
                // Rotate right.
                mdlRotation -= 1.0f * 0.10f;
            }

            if (keyboardState.IsKeyDown(Keys.Up))
            {
                // Rotate left.
                // Create some velocity if the right trigger is down.
                // Now scale our direction by how hard the trigger is down.
                mdlVelocityAdd *= 0.05f;
                mdlVelocity += mdlVelocityAdd;
            }

            if (keyboardState.IsKeyDown(Keys.Down))
            {
                // Rotate left.
                // Now scale our direction by how hard the trigger is down.
                mdlVelocityAdd *= -0.05f;
                mdlVelocity += mdlVelocityAdd;
            }

            if (keyboardState.IsKeyDown(Keys.R))
            {
                mdlVelocity = Vector3.Zero;
                mdlPosition = Vector3.Zero;
                mdlRotation = 0.0f;
                tardisSoundInstance.Play();
            }
        }

        private void writeText(string msg, Vector2 msgPos, Color msgColour)
        {
            spriteBatch.Begin();
            string output = msg;
            // Find the center of the string
            Vector2 FontOrigin = fontToUse.MeasureString(output) / 2;
            Vector2 FontPos = msgPos;
            // Draw the string
            spriteBatch.DrawString(fontToUse, output, FontPos, msgColour);
            spriteBatch.End();
        }

        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
            mdl_Manager = new ModelManager();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.IsMouseVisible = true;
            Window.Title = "Lab 5 - Models";

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //-------------------------------------------------------------
            // added to load font
            //-------------------------------------------------------------
            fontToUse = Content.Load<SpriteFont>(".\\Fonts\\DrWho");
            //-------------------------------------------------------------
            // added to load Song
            //-------------------------------------------------------------
            bkgMusic = Content.Load<Song>(".\\Audio\\DoctorWhotheme11");
            MediaPlayer.Play(bkgMusic);
            MediaPlayer.IsRepeating = true;
            songInfo = "Song: " + bkgMusic.Name + " Song Duration: " + bkgMusic.Duration.Minutes + ":" + bkgMusic.Duration.Seconds;
            //-------------------------------------------------------------
            // added to load Model
            //-------------------------------------------------------------
            mdlTardis = Content.Load<Model>(".\\Models\\tardis");
            mdl_Manager.addModel("Tardis", mdlTardis);

            mdl_Manager.GetModel("Tardis");

            String testString = mdl_Manager.GetModel("Tardis").ToString();
        
            Console.WriteLine(testString);
            
            aspectRatio = graphics.GraphicsDevice.Viewport.AspectRatio;
            //-------------------------------------------------------------
            // added to load SoundFX's
            //-------------------------------------------------------------
            tardisSound = Content.Load<SoundEffect>("Audio\\tardisEdit");
            tardisSoundInstance = tardisSound.CreateInstance();
            tardisSoundInstance.Play();

            // TODO: use this.Content to load your game content here
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

            // TODO: Add your update logic here
            //modelRotation += (float)gameTime.ElapsedGameTime.TotalMilliseconds * MathHelper.ToRadians(0.1f);
            MoveModel();

            // Add velocity to the current position.
            mdlPosition += mdlVelocity;

            // Bleed off velocity over time.
            mdlVelocity *= 0.95f;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            // Copy any parent transforms.
            Matrix[] transforms = new Matrix[mdlTardis.Bones.Count];
            mdlTardis.CopyAbsoluteBoneTransformsTo(transforms);

            // Draw the model. A model can have multiple meshes, so loop.
            foreach (ModelMesh mesh in mdlTardis.Meshes)
            {
                // This is where the mesh orientation is set, as well as our camera and projection.
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = transforms[mesh.ParentBone.Index] * Matrix.CreateRotationY(mdlRotation)
                        * Matrix.CreateTranslation(mdlPosition);
                    effect.View = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);
                    effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f),
                        aspectRatio, 1.0f, 10000.0f);
                    effect.EnableDefaultLighting();
                }
                // Draw the mesh, using the effects set above.
                mesh.Draw();
            }
            writeText("The Tardis", new Vector2(50, 10), Color.Yellow);
            writeText("Instructions\nPress The Arrow keys to move the Model\nR to Reset", new Vector2(50, 50), Color.Black);

            writeText(songInfo, new Vector2(50, 100), Color.AntiqueWhite);
            base.Draw(gameTime);
        }
    }
}
