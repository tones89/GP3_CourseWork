using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


//enum enemyState
//{
    //Wander,
  //  Chase
//}
namespace GP3_Coursework
{
    class Enemy
    {
      //  public enemyState EnemyState = enemyState.Wander;
        private int i_Health;
        private int i_Power;
        private int i_speed;

        private Vector3 v3_CurrentPos;

        public Matrix viewMatrix, projectionMatrix; 
      //  private Vector3 v3_WanderDir;
        //The distance from the player before it seeks toward the player

        //In games, lots of calulations are being fired at one time, alot of which going to the AI to perform mulitple calculations 
        //per frame. In order to keep the AI from moving erraticially and unpredictably, we use a threshhold to make
        //the given action sporadically.
        //private float f_RelaxationParam = 15.0f;
        //private float f_RotSpeed = 0.5f;
        

        /// <summary>
        /// Constructor for the enemy that initializes an enemy object
        /// </summary>
        /// <param name="Health"> The enemies health</param>
        /// <param name="Power"> The enemies power. Affects the damaged accrued to the player</param>
        /// <param name="Speed"> The speed that the enemy will move towards the player at</param>
        public Enemy(int Health, int Power,int Speed)
        {
            i_Health = Health;
            i_Power = Power;
            i_speed = Speed;
        }

        #region Getter and setters- needs no explanation. 
        public int getPower()
        {
            return i_Power;
        }

        public int getHealth()
        {
            return i_Health;
        }
        #endregion

        /// <summary>
        /// Deducts the given amount from the enemies health
        /// </summary>
        /// <param name="amount">The amount of health to deduct from the enemies health</param>
        /// <returns> Enemies Health </returns>
        public int DeductHealth(int amountToDeduct)
        {
            i_Health -= amountToDeduct;
            return i_Health;
        }

        public void MoveEnemy(Vector3 enemyPos,Vector3 playerPos,Matrix enemWorld)
        {
            /*
            float deltaX = enemyPos.X-playerPos.X;
            float deltaY = enemyPos.Y-playerPos.Y;
            float deltaZ = enemyPos.Z-playerPos.Z;
            */
         // float chaseDistance = 40f;

            float distance = Vector3.Distance(enemyPos,playerPos);

            enemWorld *= Matrix.CreateTranslation(playerPos);

            enemyPos = enemWorld.Translation;

        }
    }
}
