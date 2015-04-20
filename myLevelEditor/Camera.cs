using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace myLevelEditor
{
    class Camera
    {
        float speed = 5;

        public Vector2 Position = Vector2.Zero;
        

        public float Speed 
        {
            get { return speed; }
            set { speed = (float)Math.Max(value, 1f); }
        }
        public Camera() 
        {
            
        }
        public Camera(float x,float y) 
        {
           
        }
        public void Update(float x,float y) 
        {
            
        }
    }
}
