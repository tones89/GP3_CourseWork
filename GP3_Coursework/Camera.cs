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
        
        public CameraMode currentCameraMode = CameraMode.chase; //setting default camera mode to chase camera 

        private Vector3 position; // the position of the camera in  3d space
        private Vector3 target; // where the camera is going to look at.

        public Matrix viewMatrix, projectionMatrix; // the two crucial matrices for a 3d camera. 
        //Position matrix is used to determine aspect ratios, field of views and near and far clip planes. 
        // The View matrix transfroms world to view space ( vertices) to represent where the cam is looking, 
        //the normal vector and its lookat. 

        private float yaw, pitch, roll;     //these three variables control the angles at which the camera will 
        
        //roll Angle at which we  Rotate around  the z axis
        //pitch Angle at which we Rotate around  the x axis 
        //yaw   Angle at which we Rotate around  the y axis

        private float speed;   //Speed at which the camera will move

        private Matrix cameraRotation;  //The matrix which rotations will be passed to, to make the camera rotate. 

        //the desried target and position  are used when we use 3rd person cams.
        //they are used to calculate the positions and lookats of the chase object, 
        //and are then added to the actual position and lookat to make the
        //camera move to the position of the object and ensure that the camera is looking at the
        //correct place ( based ont he chase object)
        private Vector3 desiredPosition;

        private Vector3 desiredTarget;

        private Vector3 offsetDistance; // The distance to remain behind the player object, used for 3rd person camera

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
            position = new Vector3(0, 0, 50);   //default position of the camera is 50 on the z axis, when the camera is created ti
            //will be 50 units from the centre of the world. 

            target = new Vector3();
            viewMatrix = Matrix.Identity; //This initiates the matrix to a default matrix.
           
            //set The rotation variables to default and set the speed of the camera. 
            yaw = 0.0f;
            pitch = 0.0f;
            roll = 0.0f;
            speed = .3f;

            //These two vars will be used to ensure that the camera
            // will move smoothly when trying to keep up with the object.

            //These
            desiredPosition = position;


            desiredTarget = target;



            offsetDistance = new Vector3(2, 10, 10); // set the distance the camera should be from the object,
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

            currentCameraMode++;    //Use the integer from the enum, and simply increase by one. hence we are 
            //switching between them sequentially

            if ((int)currentCameraMode > 1) //if it is greater than 2 ( the last number in enum) set back to 0 the first,
            {
                currentCameraMode = 0;
            }
        }

        /// <summary>
        /// The input method whch will be called from game1 to update the camera each frame.
        /// It handles the user input to move the camera, and also manages the cameras view matrix
        /// to ensure it keeps up to date with its movement.
        /// </summary>
        /// <param name="chaseObjectsWorld"> The object that we want to follow's world matrix,</param>
        public void Update(Matrix chaseObjectsWorld)
        { 
            HandleInput();
            UpdateViewMatrix(chaseObjectsWorld); //pass the view matrix the objects world matrix to allow tracking.
        }


        /// <summary>
        /// This method updates the cameras view matrix.
        /// the view matrix is calculated as a standalone method,and only once. We use a swithch case to 
        /// switch between differernt camera modes. Within each of the different camera modes, 
        /// the target position and rotation vars are manipulated and changed; and these variables are then passed
        /// to the view matrix calculation which will change the camera accordingly.
        /// </summary>
        /// <param name="chasedObjectsWorld"> Used for the third person camera, this ius the world matrix of the world
        /// we wish to follow.</param>
        private void UpdateViewMatrix(Matrix chasedObjectsWorld)
        {
            //The switch case which switches between the three types of cameras.
            switch (currentCameraMode)
            {
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
                cameraRotation *= Matrix.CreateFromAxisAngle(cameraRotation.Right, pitch);  //This will rotate around the x axis

                cameraRotation *= Matrix.CreateFromAxisAngle(cameraRotation.Up, yaw);   // This will rotate around the y axis 

                cameraRotation *= Matrix.CreateFromAxisAngle(cameraRotation.Forward, roll); //this will rotate around the z axis

                yaw = 0.0f;
                pitch = 0.0f;
                roll = 0.0f;

                // Update the cameras lookat to keep it next to the camera and
                //to keep it facing forward when rotating. And also ensure the
                    //camera faces forard regardless of how it is rotated.
                target = position + cameraRotation.Forward;

                    break;

                case CameraMode.chase:
                    
                    cameraRotation.Forward.Normalize();
                    chasedObjectsWorld.Right.Normalize();
                    chasedObjectsWorld.Up.Normalize();
 
                    cameraRotation = Matrix.CreateFromAxisAngle(cameraRotation.Forward, roll);


                    //set the desired target to the object we are chasings position/
                    desiredTarget = chasedObjectsWorld.Translation;

                    //Set the actual target to the desired target and allow a little pitch and yaw.
                    //This ensures that the position of the camera stays smoothly behind the desired object at 
                    //all times
                    target = desiredTarget;
                    target += chasedObjectsWorld.Right * yaw;
                    target += chasedObjectsWorld.Up * pitch;

                    //set the desired position of the camera, could  just be the 
                    //offest vector. But to take rotation of chase object into account, we
                    //must transfom the camera to the world matrix and offset distance 
                    desiredPosition = Vector3.Transform(offsetDistance, chasedObjectsWorld);

                    // The smoothstep functions ensure that the camera moves and rotates
                    // smoothly to its necissary position. 
                    //It also sets the cameras rotation back to zero
                    position = Vector3.SmoothStep(position, desiredPosition, .15f);

                    yaw = MathHelper.SmoothStep(yaw, 0f, .1f);
                    pitch = MathHelper.SmoothStep(pitch, 0f, .1f);
                    roll = MathHelper.SmoothStep(roll, 0f, .2f);
                    break;

                
            }

            

            viewMatrix = Matrix.CreateLookAt(position, target, cameraRotation.Up);
           
        }

        /// <summary>
        /// Translate the camera to the given coordinates at the given speed
        /// </summary>
        /// <param name="addedVector">The position which to move the camera to. </param>
        private void MoveCamera(Vector3 addedVector)
        {
            position += speed * addedVector;
        }



        /// <summary>
        /// Method to handle the movement of the camera based on the 
        /// input from the user. 
        /// </summary>
        private void HandleInput()
        {
            KeyboardState keyboardState = Keyboard.GetState();

            //We can Rotate the camera ( roll pitch and yaw) it
            //throughout any of the camera modes.
            if (keyboardState.IsKeyDown(Keys.J))
            {
                yaw += .02f;
            }
            if (keyboardState.IsKeyDown(Keys.L))
            {
                yaw += -.02f;
            }
            if (keyboardState.IsKeyDown(Keys.I))
            {
                pitch += -.06f;
            }
            if (keyboardState.IsKeyDown(Keys.K))
            {
                pitch += .06f;
            }
            if (keyboardState.IsKeyDown(Keys.U))
            {
                roll += -.02f;
            }
            if (keyboardState.IsKeyDown(Keys.O))
            {
                roll += .02f;
            }


            //only allow the camera to be freely moved when the 
            //camera mode is free. 
            if (currentCameraMode == CameraMode.free)
            {
             
                //Camera’s Movement code
                if (keyboardState.IsKeyDown(Keys.W))
                {
                    MoveCamera(cameraRotation.Forward);
                }
                if (keyboardState.IsKeyDown(Keys.S))
                {
                    MoveCamera(-cameraRotation.Forward);
                }
                if (keyboardState.IsKeyDown(Keys.A))
                {
                    MoveCamera(-cameraRotation.Right);
                }
                if (keyboardState.IsKeyDown(Keys.D))
                {
                    MoveCamera(cameraRotation.Right);
                }
                if (keyboardState.IsKeyDown(Keys.E))
                {
                    MoveCamera(cameraRotation.Up);
                }
                if (keyboardState.IsKeyDown(Keys.Q))
                {
                    MoveCamera(-cameraRotation.Up);
                }
            }


           

            
        }
      
          
    }
}
