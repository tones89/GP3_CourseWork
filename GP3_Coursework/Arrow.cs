using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GP3_Coursework
{
    struct Arrow
    {
        public Vector3 position;
        public Vector3 direction;
        public float speed;
        public bool firing;
        public bool isActive;

        public void Update(float delta)
        {
            position += direction * speed * delta;
        }

 
    }
}
