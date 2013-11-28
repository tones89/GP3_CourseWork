using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GP3_Coursework
{
    class Player
    {
        private int i_PlayerHealth;
        private int i_Ammo;
        public Matrix[] transforms;
        public Matrix playerWorld;
        public Model model;
        public KeyboardState keyBoardState = Keyboard.GetState();
        public KeyboardState previousKeyBoardState = Keyboard.GetState();
        Camera camera = new Camera();

        /// <summary>
        /// The constructor for the creation of a new player
        /// </summary>
        /// <param name="Health"> The players starting health, used to determine if the player is alive or not </param>
        /// <param name="modelName"> The name of the model used to represent the player</param>
        public Player(int Health)
        {
            i_PlayerHealth = Health;
            i_Ammo = 4;
            playerWorld = Matrix.Identity;
        }

        public int getHealth()
        {
            return i_PlayerHealth;
        }

        public int getAmmo()
        {
            return i_Ammo; 
        }

        /// <summary>
        /// Called to add an amount to the players health
        /// </summary>
        /// <param name="health">The amount of health to be added </param>
        /// <returns>Returns the players health</returns>
        public int AddHealth(int health) 
        {
            i_PlayerHealth += health;
            return i_PlayerHealth;
        }

        /// <summary>
        /// Deducts the amount from the players health
        /// </summary>
        /// <param name="health"> The amount of health to be deducted</param>
        /// <returns> the players health</returns>
        public int DeductHealth(int health,int enemyPower)
        {
            i_PlayerHealth -= health*enemyPower;
            return i_PlayerHealth;
        }


        public int DeductHealth(int health)
        {
            i_PlayerHealth -= health ;
            return i_PlayerHealth;
        }

        /// <summary>
        /// Reduce the players ammo by 1
        /// When ammo depletes a timer counts to 5 seconds, when the time reaches 5, the players
        /// ammo is reloaded. 
        /// </summary>
        public void ReduceAmmo()
        {
            i_Ammo -= 1;
            float i = 0.0f;
            if (i_Ammo <= 0)
            {
                i+= 0.1f;

                if (i >= 5.0f)
                {
                    i_Ammo = 4; 
                }
            }
        }

        public void Update()
        {
            HandleInput();
            camera.Update(playerWorld);
          
        }

        private void  HandleInput()
        {

            if (keyBoardState.IsKeyDown(Keys.Enter) && previousKeyBoardState.IsKeyUp(Keys.Enter))
            {
                //camera.SwitchCameraMode();
            }
            if (keyBoardState.IsKeyDown(Keys.Space) && previousKeyBoardState.IsKeyUp(Keys.Space))
            {
                //FireCannon();
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
            if (keyBoardState.IsKeyDown(Keys.Left))
            {
                playerWorld *= Matrix.CreateTranslation(-playerWorld.Right);
            }
            if (keyBoardState.IsKeyDown(Keys.Right))
            {
                playerWorld *= Matrix.CreateTranslation(playerWorld.Right);
            }
            previousKeyBoardState = keyBoardState;
        }
        /// <summary>
        /// This method deals with any animations the player may have, 
        /// and makes a call to the animation class to play the given animation
        /// </summary>
        /// <param name="animationName"> The name of the animation to be activated</param>
        /// <param name="frame"> The frame to start the animation from</param>
        /// <param name="duration"> The amount of time to play the animation</param>
        public void PlayAnimation(string animationName,int frame, float duration)
        {

        }


        /// <summary>
        /// Stops the given animation from playing
        /// </summary>
        /// <param name="animationName"> The name of the animation to be stopped.</param>
        public void StopAnimation(string animationName)
        { 
        }

        /// <summary>
        /// Play a sound attributed to the player, will make a call to the audio class
        /// </summary>
        /// <param name="soundName"> The name of the sound to be played</param>
        /// <param name="duration"> the amount of time the sound has to be played for</param>
        public void PlaySound(string soundName, int duration)
        {
        }

        /// <summary>
        /// Play the given sound indefinately for the player,
        /// once again makes a call to the audio class
        /// </summary>
        /// <param name="soundName"> The name of the sound to be played</param>
        public void PlaySound(string soundName)
        {
        }

        /// <summary>
        /// Stops the given sound from being played
        /// by making a call to the audio class
        /// </summary>
        /// <param name="soundName">The name of the sound to be stopped</param>
        public void StopSound(string soundName)
        {
        }

        
    }


}
