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
        private int playerHealth;

        public int PlayerHealth
        {
            get { return playerHealth; }
            set { playerHealth = value; }
        }


        Vector3 playerPosition;

        public Vector3 PlayerPosition
        {
            get { return playerPosition; }
            set { playerPosition = value; }
        }
        Vector3 playerVelocity;

        public Vector3 PlayerVelocity
        {
            get { return playerVelocity; }
            set { playerVelocity = value; }
        }
        float playerRotation;

        public float PlayerRotation
        {
            get { return playerRotation; }
            set { playerRotation = value; }
        }
        private int i_Ammo;

        private Matrix[] transforms;
        public Matrix[] Transforms
        {
            get { return transforms; }
            set { transforms = value; }
        }

        private Matrix transformation;
        public Matrix Transformation
        {
            get { return transformation; }
            set { transformation = value; }
        }
        private Model playerModel;
        public Model PlayerModel
        {
            get { return playerModel; }
            set { playerModel = value; }
        }


        /// <summary>
        /// The constructor for the creation of a new player
        /// </summary>
        /// <param name="Health"> The players starting health, used to determine if the player is alive or not </param>
        /// <param name="modelName"> The name of the model used to represent the player</param>
        public Player(int Health)
        {
            playerHealth = Health;
            i_Ammo = 4;
            // playerWorld = Matrix.Identity;
        }

        public int getHealth()
        {
            return playerHealth;
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
            playerHealth += health;
            return playerHealth;
        }

        /// <summary>
        /// Deducts the amount from the players health
        /// </summary>
        /// <param name="health"> The amount of health to be deducted</param>
        /// <returns> the players health</returns>
        public int DeductHealth(int health, int enemyPower)
        {
            playerHealth -= health * enemyPower;
            return playerHealth;
        }




        public int DeductHealth(int health)
        {
            playerHealth -= health;
            return playerHealth;
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
                i += 0.1f;

                if (i >= 5.0f)
                {
                    i_Ammo = 4;
                }
            }
        }

        public void Update()
        {
            //  HandleInput();
            //  camera.Update(playerWorld);

        }



    }
}
