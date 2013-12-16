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
       // public CameraType cameraMode = CameraType.thirdPerson;
        #region User Defined Variables
        // Set the 3D model to draw.
        
        //variables used to manage the models within the game
        //each model is assigned a transformation matrix array to 
        //monitor the models mesh's position.
        private Model mdlPlayer;
        //private Matrix[] mdlPlayerTransform;
        Model mdl_OuterDome;
        private Matrix[] mdlOuterDomeTrans;
        Model mdl_Island;
        private Matrix[] mdlIslandTransform;
        Model mdl_Castle;
        private Matrix[] mdlCastleTrans;
        Model mdl_Tower;
        private Matrix[] mdlTowerTrans;
        Model mdl_Water;
        private Matrix[] mdlWaterTrans;
        Model mdl_Arrow;
        Matrix[] mdlArrowTrans;
        Arrow[] arrows = new Arrow[50];
     
        //default potiion variables for the static objects
        Vector3 islandPos = Vector3.Up * -3;
        Vector3 domePos = Vector3.Up * -3;
        Vector3 waterPos = Vector3.Up * -3;
        float islandRoation = 0f;

        //an instance of the main camera class
        Camera mainCamera = new Camera();
        // An instance of the audio;
        Audio audio = new Audio();
        
        //the array of cannonball structs, and
        //associated model. used to store an 
        //array of ammunition of the player
        //we alos stor transformations for the 
        //cannonball as it will be a moving object.
        cannonBall[] cannonballs = new cannonBall[4];
        Model mdl_CannonBall;
        private Matrix[] mdlCannonBallTrans;

       //Structs to hold static objects of the world/
        //held in an array to allow them to be destroyed when shot
        Castle[] castles = new Castle[1];
        Tower[] towerArray = new Tower[1];
      
        // used to determine if the background soung is playing,
        //allows music to be turned on and off.
        bool playingSong = false;

        //used to determine if the game is to be restarted. 
        bool restart = false;

        //delcare a font for displaying info to the player
        SpriteFont aFont;

        //an instance of the player, we give them a default health of 100;
        Player playerOne = new Player(100);
        KeyboardState oldKeyState = Keyboard.GetState();
        GamePadState oldPadState = GamePad.GetState(PlayerIndex.One);  
    
        /// <summary>
        /// This method takes input from the keyboard and
        /// performs a variety of tasks which will
        /// be explained at each point
        /// </summary>
        private void HandleInput()
        {

            KeyboardState keyboardState = Keyboard.GetState();
            GamePadState padState = GamePad.GetState(PlayerIndex.One, GamePadDeadZone.Circular);
        

            //delta is the velocity each frame to be applied to the player
            Vector3 deltaVelocity = Vector3.Zero;

           //assertain what direction the model is facing based on the rotation
           deltaVelocity.X = -(float)Math.Sin(playerOne.PlayerRotation);
           deltaVelocity.Z = -(float)Math.Cos(playerOne.PlayerRotation);

            //If the left key is pressed, we add rotation to the player 
            if (keyboardState.IsKeyDown(Keys.Left)||padState.IsButtonDown(Buttons.RightThumbstickLeft))
            {
                // Rotate player left 
                
                playerOne.PlayerRotation += 1.0f * 0.10f;
            }



            //As before, however with the right key.
            if (keyboardState.IsKeyDown(Keys.Right)|padState.IsButtonDown(Buttons.RightThumbstickRight))
            {
                // Rotate player  right.
              
                playerOne.PlayerRotation -= 1.0f * 0.10f;
            }

            //Used to move the player forward.

            if (keyboardState.IsKeyDown(Keys.Up)||padState.IsButtonDown(Buttons.LeftThumbstickUp))
            {
               //Determine the deltaVelocity, bd add tihs to the players velocity, 
                //thus generating movement

              deltaVelocity *= 0.05f;
              //mdlVelocity += mdlVelocityAdd;
              playerOne.PlayerVelocity += deltaVelocity;
             
            }

            if (keyboardState.IsKeyDown(Keys.Down)||padState.IsButtonDown(Buttons.LeftThumbstickDown))
            {
               //as before but this time, backwwards.
                deltaVelocity *= -0.05f;
                playerOne.PlayerVelocity += deltaVelocity;
            }

            //This function allows the player to fire using the left control key.
            if (keyboardState.IsKeyDown(Keys.LeftControl) && oldKeyState.IsKeyUp(Keys.LeftControl) || padState.IsButtonDown(Buttons.RightTrigger)&&(oldPadState.IsButtonUp(Buttons.RightTrigger)))
            {

                //firstly we cycle over the amount of ammuniton(or cannonballs held
                //within the struct. 
                for (int i = 0; i < 3; i++)
                {
                    //if the cannonball struct is not active, then we perform the
                    //logic therein for EACH cannonball
                    if (!cannonballs[i].isActive)
                    {
                        //play the sound
                        audio.PlaySoundEffect("cannonFire");
                      
                        //Allow the cannon to take into account the players rotation by create a rotational matrix based
                        //on the players rotation
                        Matrix cannonTransform = Matrix.CreateRotationY(playerOne.PlayerRotation) * 2;
                        //Only allow the cannon to be fired from the left of the boat, for more realism. 
                        cannonballs[i].direction = cannonTransform.Left;
                        //set the speed, again rather fast to account for realism.
                        cannonballs[i].speed = 30f;
                       
                        //set the cannons position to be the players position, and move off 
                        //in the direction we set beforehand
                        cannonballs[i].position = playerOne.PlayerPosition + cannonballs[i].direction;
                        //set this boolean to true, meaning the cannon is only drawn when we press the key 
                        cannonballs[i].isActive =true;
                        //no need to continue, we therefore break out of the for loop
                        break;
                    }
                }
            }

            //allow the user to exit the application  using the escape key

            if (keyboardState.IsKeyDown(Keys.Escape) && oldKeyState.IsKeyUp(Keys.Escape)||padState.IsButtonDown(Buttons.Start)&&oldPadState.IsButtonUp(Buttons.Start))
            {
                this.Exit();
            }

            // if the user hits the delete key, we allow them to restart the game
            if (keyboardState.IsKeyDown(Keys.Delete) && oldKeyState.IsKeyUp(Keys.Delete)||padState.IsButtonDown(Buttons.B)&&oldPadState.IsButtonUp(Buttons.B))
            {
                //we set restart to true.
                restart = true;
            }

            //This is the key to confirm the user wishes to restar or quit...
            if (keyboardState.IsKeyDown(Keys.Y) && oldKeyState.IsKeyUp(Keys.Y)||padState.IsButtonDown(Buttons.A)&&oldPadState.IsButtonUp(Buttons.A))
            {

                //if we are restarting, we must reset the variables to their defaults.
                //These are the variables pertaining to the user, health- positioning ect. 
                restart = false;
                playerOne.PlayerPosition = new Vector3(0f, 0f, 80f);
                playerOne.PlayerRotation = 0f;
                playingSong = true;
                playerOne.PlayerVelocity = Vector3.Zero;
                playerOne.PlayerHealth = 100;


                //we must also reset the structs of stationary objects, so that they exist,
                //the nest time the player plays the game
                for (int i = 0; i < castles.Length; i++)
                {
                    castles[i].isActive = true;

                }

                for (int i = 0; i < towerArray.Length; i++)
                {
                    towerArray[i].isActive = true;
                }
            }
          
            if (keyboardState.IsKeyDown(Keys.Tab) && oldKeyState.IsKeyUp(Keys.Tab)||padState.IsButtonDown(Buttons.LeftThumbstickDown)&&oldPadState.IsButtonUp(Buttons.LeftThumbstickDown))
            {
                mainCamera.SwitchCameraMode();
            }
/*
            if (keyboardState.IsKeyDown(Keys.M) && oldKeyState.IsKeyUp(Keys.M))
            {
                
                audio.PlaySong("backMusic");
            }
*/

            //The following key is used to start the music
            if (keyboardState.IsKeyDown(Keys.N) && oldKeyState.IsKeyUp(Keys.N)||padState.IsButtonDown(Buttons.DPadUp)&&oldPadState.IsButtonUp(Buttons.DPadUp))
            {
                playingSong = true;
                
            }

            //This key is the key used to stop the music
            if (keyboardState.IsKeyDown(Keys.M) && oldKeyState.IsKeyUp(Keys.M)||padState.IsButtonDown(Buttons.DPadDown)&&oldPadState.IsButtonUp(Buttons.DPadDown))
            {

                playingSong = false;
            }
             

            //maintain an instance of the last key pressed, to test if the user has pressed , and relased a key 
            oldKeyState = keyboardState;
            oldPadState = padState;
        }

    



        /// <summary>
        /// This method is called to setup the basic transformations, and basic effects
        /// for the given model.  Cycles through each of the meshes in the model and returns
        /// the required array of bones. We need this matrix[] later to draw the model. 
        /// </summary>
        /// <param name="myModel">The model wihch we want to set up the meshes for</param>
        /// <returns>It will return  transformation matrix  array which keeps track of all  the bones in the mesh
        /// to allow movement and positioning of each model. </returns>
  
        private Matrix[] GetDefaultTransforms(Model myModel)
        {
            Matrix[] absoluteTransforms = new Matrix[myModel.Bones.Count];
            myModel.CopyAbsoluteBoneTransformsTo(absoluteTransforms);

            foreach (ModelMesh mesh in myModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {

                    //these lines of code allow us to manipulate the colour, and effect
                    //of the players appereance. We wish only to affect the player, so thereforewe search for
                    //just the player mesh.
                    if(mesh.Equals(playerOne.PlayerModel.Meshes[0]))
                    {
                        effect.LightingEnabled = true; // turn on the lighting subsystem.
                        effect.DirectionalLight0.DiffuseColor = Color.Yellow.ToVector3();
                        effect.DirectionalLight0.Direction = new Vector3(1, 0, 0);  // coming along the x-axis
                       // effect.DirectionalLight0.SpecularColor = new Vector3(0, 1, 0); // with green highlights
                        effect.DirectionalLight0.SpecularColor = Color.Gold.ToVector3();

                    }
                    else
                    {
                        effect.EnableDefaultLighting();
                    }
                 
                   // effect.Projection = projectionMatrix;
                  //  effect.View = viewMatrix;
                     

                }
            }
                return absoluteTransforms;
            
        }


        /// <summary>
        /// Although not implemented, this method calculates the distance between two models,
        /// //usinf vector calculations, and would be used for A.I purposes
        /// </summary>
        /// <param name="playerPosition">The vector3 position of the player</param>
        /// <param name="otherObject">The vector3 of the other object</param>
        /// <returns>the distance between the two objects as a float</returns>
        public float calculateDistanceToPlayer(Vector3 playerPosition,Vector3 otherObject)
        {
            float distance = Vector3.Distance(playerPosition, otherObject);
            return distance;
        }
        
        

        /// <summary>
        /// This method cycles through a given modesl meshes, and assigns a projection, and view matrix 
        /// for the model. This method then multiplies the bone transforms, from previously, by the objects,
        /// position and roation matrix to draw the model in the correct orientation and position each frame.
        /// I have also assigned fog to the scene for each object, making it black, to simulate night
        /// </summary>
        /// <param name="model">The model to be drawn</param>
        /// <param name="modelTransform">Matrix holding rotation and movement information about the model</param>
        /// <param name="absoluteBoneTransforms"> The array of bone transforms from the previous method</param>
        public void DrawModel(Model model, Matrix modelTransform, Matrix[] absoluteBoneTransforms)
        {
            //Draw the model, a model can have multiple meshes, so loop
            foreach (ModelMesh mesh in model.Meshes)
            {
                //This is where the mesh orientation is set
                foreach (BasicEffect effect in mesh.Effects)
                {

                    effect.FogEnabled = true;
                    effect.FogColor = Color.Black.ToVector3() ;
                    effect.FogStart = 5.0f;
                    effect.FogEnd = 65.0f;
                    effect.World = absoluteBoneTransforms[mesh.ParentBone.Index] * modelTransform;
                    effect.View = mainCamera.viewMatrix;
                    effect.Projection = mainCamera.projectionMatrix;

                }
                //Draw the mesh, will use the effects set above.
                mesh.Draw();
            }
        }
        


        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.IsMouseVisible = false ;
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
            this.IsMouseVisible = false;
            Window.Title = "Night Raid";
            
          //  InitializeTransform();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

            //Setting the players default positions, rotations etc
            playerOne.PlayerPosition = new Vector3(0f, 0f, 80f);
            playerOne.PlayerRotation = 0f;
            playerOne.PlayerVelocity = Vector3.Zero;


            spriteBatch = new SpriteBatch(GraphicsDevice);
            oldKeyState = Keyboard.GetState() ;


            //==================================================
            //==================Model Content====================
            //==================================================
            //==================================================

            // This is where we load each model, and assign their
            //transform matrix []'s
            mdlPlayer = Content.Load<Model>(".\\Models\\Pirate Ship");
            playerOne.PlayerModel = mdlPlayer;
            playerOne.Transforms = GetDefaultTransforms(playerOne.PlayerModel);
            mdl_OuterDome = Content.Load<Model>(".\\Models\\Dome"); //the skyDome
                mdlOuterDomeTrans = GetDefaultTransforms(mdl_OuterDome);
            mdl_Water = Content.Load<Model>(".\\Models\\Water");    //The water
                mdlWaterTrans = GetDefaultTransforms(mdl_Water);
            mdl_Island = Content.Load<Model>(".\\Models\\Island");  //The central island
                mdlIslandTransform = GetDefaultTransforms(mdl_Island);
            mdl_Tower = Content.Load<Model>(".\\Models\\castleTower");    // The Tower
                mdlTowerTrans = GetDefaultTransforms(mdl_Tower);    
            mdl_Castle = Content.Load<Model>(".\\Models\\Castl");   //The castle
                mdlCastleTrans = GetDefaultTransforms(mdl_Castle);
            mdl_CannonBall = Content.Load<Model>(".\\Models\\CannonBall");  //The cannonball
                mdlCannonBallTrans = GetDefaultTransforms(mdl_CannonBall);
            mdl_Arrow = Content.Load<Model>(".\\Models\\Arrow");    
                mdlArrowTrans = GetDefaultTransforms(mdl_Arrow);
            
            //==================================================
            //==================Font Content====================

            aFont = Content.Load<SpriteFont>(".\\Fonts\\HUD");


            //==================================================
            //==================Audio Content===================
            //==================================================
            SoundEffect islandcollision = Content.Load<SoundEffect>(".\\Audio\\Manisland");
                audio.AddSoundEffect("manIsland", islandcollision);

            SoundEffect cannonFire = Content.Load<SoundEffect>(".\\Audio\\cannonFire");
                audio.AddSoundEffect("cannonFire", cannonFire);

            SoundEffect cannonIsland = Content.Load<SoundEffect>(".\\Audio\\cannonIsland");
                audio.AddSoundEffect("cannonIsland",cannonIsland);

            SoundEffect cannonTower = Content.Load<SoundEffect>(".\\Audio\\explosion");
                audio.AddSoundEffect("cannonTower", cannonTower);
           

            Song background = Content.Load<Song>(".\\Audio\\backMusic");
                MediaPlayer.Play(background);
                playingSong = true;


            //==================================================
            //==================Scenery Content=================
            //==================================================


            //we must set the towers and castles to active on start, otherwise they wont be drawn.
            for (int i = 0; i < 1; i++)
                {
                    if (!towerArray[i].isActive)
                    {
                       
                        towerArray[i].isActive = true;
                    }
                }

                for (int c = 0; c < 1; c++)
                {
                    if (!castles[c].isActive)
                    {
                        castles[c].isActive = true;
               
                    }
                }


            // TODO: use this.Content to load your game content here
        }

     

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void
            Update(GameTime gameTime)
        {



            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
         
            //an instance of the player position and rotation matrix, to allow camera to follow layer
           Matrix transform =  playerOne.Transformation = makeTransform(playerOne.PlayerRotation, playerOne.PlayerPosition);
          
            //update the main camera, passing the player's transformation to allow correct following procedures
            mainCamera.Update(transform);
          
            //A call to handle the input each frame
            HandleInput();

            //Used to manage collisions each frame. 
            ManageCollisions();


            //update cannonballs if they are active
            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            for (int i = 0; i < 3; i++)
            {
                cannonballs[i].Update(timeDelta);
            }

            //instantiate positions and rotations for the towers and castles
            for (int i = 0; i < 1; i++)
            {
                towerArray[i].towerPosition = new Vector3(30f, 1f, 30f);
                towerArray[i].towerRotation = 0f;
            }
              

                for (int c = 0; c < 1; c++)
                {
                    castles[c].position = new Vector3(-30f, 9f, -25f);
                    castles[c].rotation = 600;
                }


            //these variables manage the background music
            //we make a call to the media player to resume 
            //if the boolean to  play is true
            //and to pause the song if it is false.
                if (playingSong == true)
                {
                    MediaPlayer.Resume();

                }
                else if (playingSong == false)
                {
                    MediaPlayer.Pause();
                }

                //update the players velocity each frame
                playerOne.PlayerPosition += playerOne.PlayerVelocity;

            //in doing so, we need to "temper" the velocity to prevent the 
            // model conitually moving
                playerOne.PlayerVelocity *= 0.95f;
        
                base.Update(gameTime);
            
        }


        /// <summary>
        /// This method manages all the collisions that occur throughout the game
        /// </summary>
        void ManageCollisions()
        {


            //Create bounding volumes for each of the objects in the world. 
            //we use the default mesh spehere of the object, an multiply it to 
            //reduce its size to avoid false positive collision. 
            BoundingSphere boatSphere = new BoundingSphere(
                playerOne.PlayerPosition, playerOne.PlayerModel.Meshes[0].BoundingSphere.Radius * 0.025f);
            

            BoundingSphere islandSphere = new BoundingSphere(
                islandPos, mdl_Island.Meshes[0].BoundingSphere.Radius * 1.7f);

            BoundingSphere domeSphere = new BoundingSphere(
                domePos, mdl_OuterDome.Meshes[0].BoundingSphere.Radius * 0.5f);
            BoundingSphere towerSphere;


            //These statements determine whether two objects's spheres have intersected. 

            //If player hits the island
            if (boatSphere.Intersects(islandSphere))
            {
                //play sound to alert player they've hit an object
                audio.PlaySoundEffect("manIsland");
                
                //deduct Hit points 
                playerOne.PlayerHealth -= 10;
                //this command reverses the players current velocity, this making forward thrust backward thrust.
                playerOne.PlayerVelocity = Vector3.Negate(playerOne.PlayerVelocity);

                //and then add this velocity to their poition, thus pushing the player backwards
                playerOne.PlayerPosition += playerOne.PlayerVelocity/2;
                
                //AS we have implemented Health to the player, We must do something when this health drops bellow 0
                if (playerOne.PlayerHealth<=0)
                {
                    //set the health to 0, to prevent it contiuing to count down
                    playerOne.PlayerHealth = 0;

                    //stop the sound by calling the audi class method
                    audio.StopSoundEffect("manIsland");

                    //Make the player stationary
                    playerOne.PlayerVelocity = Vector3.Zero;
                    playerOne.PlayerRotation *= 180;
                    playerOne.PlayerPosition = Vector3.SmoothStep(playerOne.PlayerPosition,new Vector3(0.0f,-5f,0.0f),1f);
                    //set the restart boolean to true- we are restarting. 
                    restart = true;
                }
               
            }

            
            //This nested for loop is used to cycle through every active cannonball on screen.
            // then  cycle through every castle, and every and tower and determine whether a collision has occured
            for (int i = 0; i<cannonballs.Length; i++)
            {
                if (cannonballs[i].isActive)
                {
                    //create a bounding colume for EACH cannoball
                    BoundingSphere CannonSphere = new BoundingSphere(
                        cannonballs[i].position, mdl_CannonBall.Meshes[0].BoundingSphere.Radius * 0.95f);
                   
                    //this modifier makes the island collision spehere half the size,meaninf the cannonball doesnt
                    //dissapear until it hits the centre of the island.
                    float modifier = islandSphere.Radius * 0.5f;
                    islandSphere.Radius = modifier;
                    if(CannonSphere.Intersects(islandSphere))
                    {
                        //play a sound
                        audio.PlaySoundEffect("cannonIsland");  

                        //deavtivate the cannoball( removing it from the screen)
                        cannonballs[i].isActive = false;
                        break;
                    }
                        for (int t = 0; t <towerArray.Length; t++)
                        {
                            if (towerArray[t].isActive)
                            {
                                //repeat the process for the tower. This time, if the cannonball hits the tower, delete both 
                                //the tower and the cannonball. 
                                towerSphere = new BoundingSphere(
                                    towerArray[t].towerPosition, mdl_Tower.Meshes[0].BoundingSphere.Radius * 0.0100f);

                                //The default bounding volume was far too big, reduce it further.
                                     float furtherReduct = towerSphere.Radius * 0.25f;
                                      towerSphere.Radius = furtherReduct;
                                        if (CannonSphere.Intersects(towerSphere))
                                        {
                                            audio.PlaySoundEffect("cannonTower");
                                            towerArray[t].isActive = false;
                                            cannonballs[i].isActive = false;
                                            break;
                                        }
                            }
                        }                       //As before.

                                            for (int c = 0; c < castles.Length; c++)
                                            {
                                                if (castles[c].isActive)
                                                {
                                                    BoundingSphere castleSphere = new BoundingSphere(
                                                        castles[c].position, mdl_Castle.Meshes[0].BoundingSphere.Radius * 0.0010f);
                                                    float furtherReduct = castleSphere.Radius * 0.25f;
                                                    castleSphere.Radius = furtherReduct;
                                                    if (CannonSphere.Intersects(castleSphere))
                                                    {
                                                        audio.PlaySoundEffect("cannonTower");
                                                        castles[c].isActive = false;
                                                        cannonballs[i].isActive = false;
                                                        break;
                                                    }
                                                }
                                            }
                }
            }
             
        }
            

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            //fore each cannonball in the list, if it is active, then we create
            /// a movement matrix. We dont require a rotation in this instance as it is already inheriting the player 
            /// models
            for (int i = 0; i < 3; i++)
            {
                if (cannonballs[i].isActive)
                {
                    Matrix cannonTrans =  Matrix.CreateTranslation(cannonballs[i].position);
                    DrawModel(mdl_CannonBall, cannonTrans, mdlCannonBallTrans);
                }
            }

            //as for the cannonballs
            for (int i = 0; i < 1; i++)
            {
                if (towerArray[i].isActive)
                {
                    Matrix towerTrans = makeTransform(towerArray[i].towerRotation, towerArray[i].towerPosition);
                    DrawModel(mdl_Tower, towerTrans, mdlTowerTrans); 
                }
            }

            //as for the towers/cannonballs
            for (int c = 0; c < 1; c++)
            {
                if (castles[c].isActive)
                {
                    Matrix castleTrans = makeTransform(castles[c].rotation, castles[c].position);
                    DrawModel(mdl_Castle, castleTrans, mdlCastleTrans);
                }
            }

            //not used 
            for (int a = 0; a < arrows.Length; a++)
            {
                if (arrows[a].isActive)
                {
                    Matrix arrowTrans = Matrix.CreateTranslation(arrows[a].position);
                    DrawModel(mdl_Arrow, arrowTrans, mdlArrowTrans);
                }
            }


            //create a movement/rotation matrix from the method, and pass this, and the players transform 
            //array into the draw method, to keep the player moveing onscreen.
            Matrix playerTransform = makeTransform(playerOne.PlayerRotation, playerOne.PlayerPosition);
            DrawModel(playerOne.PlayerModel, playerTransform, playerOne.Transforms);   
           
            //as before, these methods simply make a rotational and position matrix, and call the draw method
            Matrix terrainTrans = makeTransform(0f, domePos);
                DrawModel(mdl_OuterDome, terrainTrans, mdlOuterDomeTrans);
            Matrix waterTrans = makeTransform(0f, waterPos);
                DrawModel(mdl_Water, waterTrans, mdlWaterTrans);
            Matrix islandTrans = makeTransform(islandRoation, islandPos);
                DrawModel(mdl_Island, islandTrans, mdlIslandTransform);

            //a call to perform 2d drawing ( the Heads up Display.) Called after 3D
            // to esnusre it isnt drawn over
            Draw2d();
               

            base.Draw(gameTime);
        }

        
           
        void Draw2d()
        {
            spriteBatch.Begin();
           
          //Display the players energy (health onscreen);
            spriteBatch.DrawString(aFont, "Men Aboard:  " + playerOne.PlayerHealth.ToString(), new Vector2(100, 20), Color.White);


            //When restart Variable is called, draw the prompt to the user, to let them know how to quit the game,
            //or restart. 
            if (restart == true)
            {
               
                spriteBatch.DrawString(aFont, " Do you want to restart? Hit Y to restart: Otherwise hit escape to quit", new Vector2(Window.ClientBounds.Height/2, Window.ClientBounds.Width / 2), Color.GhostWhite);
            }
            spriteBatch.End();
           
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

        }


        /// <summary>
        /// A simple method which is used to create a models transformation matrix
        /// </summary>
        /// <param name="rotation">The models current rotation</param>
        /// <param name="position">The models current position</param>
        /// <returns>A transformation matrix representing the given models position and orientation</returns>
        Matrix makeTransform(float rotation, Vector3 position)
        {
            return Matrix.CreateRotationY(rotation)*Matrix.CreateTranslation(position);
        }


       
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            
            // TODO: Unload any non ContentManager content here
        }


  
    }
}
    
   