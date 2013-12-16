using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;


/// <summary>
/// An enumarator to cycle through the various cameras
/// orbit allows camera to rotate freely around a static object
/// free cam allows cam to be controlled by user and fly around the environment
/// chase cam is the third person camera which remains behind the player throughout the game 
/// </summary>
public enum CameraMode
{
    free = 0,
    chase = 1,
}

// THIS CODE WAS SOURCED AND REFACTORED FROM http://www.madgamedev.com/post/2010/09/05/Article-Simple-3D-Camera-in-XNA.aspx 
//[accessed 20/2013]

namespace GP3_Coursework
{
    class Camera
    {

        public CameraMode cameraType = CameraMode.chase; //setting default camera mode to chase camera 

        public Vector3 actualPosition; // the position of the camera in  3d space
        private Vector3 lookat; // where the camera is going to look at.

        public Matrix viewMatrix, projectionMatrix; // the two crucial matrices for a 3d camera. 
        //projection matrix translates 3d to flat imaghe
        // The View matrix transfroms world to view space 

        private float rotY, rotX, rotZ;     //these three variables control the angles at which the camera will 
        
        private float speed;   //Speed at which the camera will move

        private Matrix cameraRotation;  //The matrix which rotations will be passed to, to make the camera rotate. 

        //the desried target and position  are used when we use 3rd person cams.
        //they are used to calculate the positions and lookats of the chase object, 
        //and are then added to the actual position and lookat to make the
        //camera move to the position of the object and ensure that the camera is looking at the
        //correct place ( based ont he chase object)
        private Vector3 desiredPosition;

        private Vector3 desiredTarget;

        private Vector3 chaseDistance; // The distance to remain behind the player object, used for 3rd person camera

        /// <summary>
        /// The default constructor for the camera,
        /// calls the reset camera function to reset all the matrice calculations
        /// such as movement, rotation etc. 
        /// </summary>
        public Camera()
        {
            ResetCamera();
        }

        /// <summary>
        /// Reset camera method, used to reset camera's values to  their defauts . 
        /// </summary>
        public void ResetCamera()
        {
            actualPosition = new Vector3(0, 0, 50);   //default position of the camera is 50 on the z axis, when the camera is created ti
            //will be 50 units from the centre of the world. 

            lookat = new Vector3();
            viewMatrix = Matrix.Identity; //This initiates the matrix to a default matrix.
           
            //set The rotation variables to default and set the speed of the camera. 
            rotY = 0.0f;
            rotX = 0.0f;
            rotZ = 0.0f;
            speed = .3f;

           //set default position to the actual position, set above
            desiredPosition = actualPosition;


            //and the lookat the same, set it to default
            desiredTarget = lookat;

            chaseDistance = new Vector3(10, 10, 10); // set the distance the camera should be from the object,
           // offsetDistance = new Vector3(-10, 2, 0); // set the distance the camera should be from the object,
            // used for 3rd person camera

            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), 16 / 9, .5f, 500f);   //Creating a basic
            //projection matrix. 
            cameraRotation = Matrix.Identity;
        }

        /// <summary>
        /// Method which we will call when we want to switch between camera modes
        /// </summary>
        public void SwitchCameraMode()
        {
            ResetCamera();  //First Reset the camera.

            cameraType++;    //Use the integer from the enum, and simply increase by one. hence we are 
            //switching between them sequentially

            if ((int)cameraType > 1) //if it is greater than 2 ( the last number in enum) set back to 0 the first,
            {
                cameraType = 0;
            }
        }

        /// <summary>
        /// The input method whch will be called from game1 to update the camera each frame.
        /// It handles the user input to move the camera, and also manages the cameras view matrix
        /// to ensure it keeps up to date with its movement.
        /// </summary>
        /// <param name="playerTransform"> The object that we want to follow's world matrix,</param>
        public void Update(Matrix playerTransform)
        { 
            HandleInput();
            SetupViewMatrix(playerTransform); //pass the view matrix the objects world matrix to allow tracking.
        }


        /// <summary>
        /// This method updates the cameras view matrix.
        /// the view matrix is calculated as a standalone method,and only once. We use a swithch case to 
        /// switch between differernt camera modes. Within each of the different camera modes, 
        /// the target position and rotation vars are manipulated and changed; and these variables are then passed
        /// to the view matrix calculation which will change the camera accordingly.
        /// </summary>
        /// <param name="playerTransform"> Used for the third person camera, this ius the world matrix of the world
        /// we wish to follow.</param>
        private void SetupViewMatrix(Matrix playerTransform)
        {
            //The switch case which switches between the types of cameras.
            switch (cameraType)
            {
                #region FreeCam Mode
                case CameraMode.free:

                   
                //Free-camera code goes here.

                //These Methods normalize the vetors. This is resetting the vectors 
                //positions and making them 1 unit in length and facing same direction as oriignal
                //essentially ensuring camera matrix is reset before rotating
                cameraRotation.Forward.Normalize();
                cameraRotation.Up.Normalize();
                cameraRotation.Right.Normalize();

                //Allowing the camera to rotate. We rotate the matric around itself
                // To ensure that it rotates correctly, regardless of prevoius rotation.
                cameraRotation *= Matrix.CreateFromAxisAngle(cameraRotation.Right, rotX);  //This will rotate around the x axis

                cameraRotation *= Matrix.CreateFromAxisAngle(cameraRotation.Up, rotY);   // This will rotate around the y axis 

                cameraRotation *= Matrix.CreateFromAxisAngle(cameraRotation.Forward, rotZ); //this will rotate around the z axis

                rotY = 0.0f;
                rotX = 0.0f;
                rotZ = 0.0f;

                // Update the cameras lookat to keep it next to the camera and
                //to keep it facing forward when rotating. And also ensure the
                    //camera faces forard regardless of how it is rotated.
                lookat = actualPosition + cameraRotation.Forward;

                break;
                #endregion
                case CameraMode.chase:
                    
                    cameraRotation.Forward.Normalize();
                    playerTransform.Right.Normalize();
                    playerTransform.Up.Normalize();
 
                    cameraRotation = Matrix.CreateFromAxisAngle(cameraRotation.Forward, rotZ);

                    //set the desired target to the object we are chasings position/
                    desiredTarget = playerTransform.Translation;
   
                    //Set actual target pos to be the desired target pos and use pitch and and yaw to ensure
                    //that smooth transitioning occurs throughout
                    lookat = desiredTarget;
                    lookat += playerTransform.Right * rotY;
                    lookat += playerTransform.Up * 0.5f;

                    //set the desired position of the camera, could  just be the 
                    //offest vector. But to take rotation of chase object into account, we
                    //must transfom the camera to the world matrix and offset distance 
                    desiredPosition = Vector3.Transform(chaseDistance, playerTransform);

                    // The smoothstep functions ensure that the camera moves and rotates
                    // smoothly to its necissary position. 
                    //It also sets the cameras rotation back to zero
                    actualPosition = Vector3.SmoothStep(actualPosition, desiredPosition, .15f);

                    rotY = MathHelper.SmoothStep(rotY, 0f, .1f);
                    rotX = MathHelper.SmoothStep(rotX, 0f, .1f);
                    rotZ = MathHelper.SmoothStep(rotZ, 0f, .2f);
                    break;

                
            }

            viewMatrix = Matrix.CreateLookAt(actualPosition, lookat, cameraRotation.Up);


           
        }

        /// <summary>
        /// Translate the camera to the given coordinates at the given speed
        /// </summary>
        /// <param name="delta">The position which to move the camera to. </param>
        private void MoveCamera(Vector3 delta)
        {
            actualPosition += speed * delta;
        }



        /// <summary>
        /// Method to handle the movement of the camera based on the 
        /// input from the user. 
        /// </summary>
        private void HandleInput()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            GamePadState padState = GamePad.GetState(PlayerIndex.One);
            //We can Rotate the camera ( roll pitch and yaw) it
            //throughout any of the camera modes.
            if (keyboardState.IsKeyDown(Keys.J)||padState.IsButtonDown(Buttons.LeftThumbstickRight))
            {
                rotY += .02f;
            }
            if (keyboardState.IsKeyDown(Keys.L)||padState.IsButtonDown(Buttons.LeftThumbstickLeft))
            {
                rotY += -.02f;
            }
            if (keyboardState.IsKeyDown(Keys.I)||padState.IsButtonDown(Buttons.LeftThumbstickUp))
            {
                rotX += -.06f;
            }
            if (keyboardState.IsKeyDown(Keys.K)||padState.IsButtonDown(Buttons.LeftThumbstickDown))
            {
                rotX += .06f;
            }
            if (keyboardState.IsKeyDown(Keys.U))
            {
                rotZ += -.02f;
            }
            if (keyboardState.IsKeyDown(Keys.O))
            {
                rotZ += .02f;
            }


            //only allow the camera to be freely moved when the 
            //camera mode is free. 
            if (cameraType == CameraMode.free)
            {
                GamePadState padStat2  = new GamePadState();
                //Camera’s Movement code
                if (keyboardState.IsKeyDown(Keys.W)||padStat2.IsButtonDown(Buttons.RightThumbstickUp))
                {
                    MoveCamera(cameraRotation.Forward);
                }
                if (keyboardState.IsKeyDown(Keys.S)||padStat2.IsButtonDown(Buttons.RightThumbstickDown))
                {
                    MoveCamera(-cameraRotation.Forward);
                }
                if (keyboardState.IsKeyDown(Keys.A)||padStat2.IsButtonDown(Buttons.RightThumbstickRight))
                {
                    MoveCamera(-cameraRotation.Right);
                }
                if (keyboardState.IsKeyDown(Keys.D)||padStat2.IsButtonDown(Buttons.RightThumbstickLeft))
                {
                    MoveCamera(cameraRotation.Right);
                }
                if (keyboardState.IsKeyDown(Keys.E)||padStat2.IsButtonDown(Buttons.LeftShoulder))
                {
                    MoveCamera(cameraRotation.Up);
                }
                if (keyboardState.IsKeyDown(Keys.Q)||padStat2.IsButtonDown(Buttons.RightShoulder))
                {
                    MoveCamera(-cameraRotation.Up);
                }
            }

            
        }
      
    }
}
