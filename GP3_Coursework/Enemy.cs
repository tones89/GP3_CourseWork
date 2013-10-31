using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GP3_Coursework
{
    class Enemy
    {
        private int i_Health;

        private int i_Power;

        /// <summary>
        /// Constructor for the enemy that initializes an enemy object
        /// </summary>
        /// <param name="Health"> The enemies health</param>
        /// <param name="Power"> The enemies power. Affects the damaged accrued to the player</param>
        public Enemy(int Health, int Power)
        {
            i_Health = Health;
            i_Power = Power;
        }

        public int getHealth()
        {
            return i_Health;
        }

        public void DeductHealth(int amount)
        {
            i_Health -= amount;
        }
        public void move(int speed)
        { 

        }

    }
}
