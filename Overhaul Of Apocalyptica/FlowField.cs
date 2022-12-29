using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Design;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Overhaul_Of_Apocalyptica.Entities
{
    class FlowField
    {
       Vector2[,] field = new Vector2[200, 120]; //Every 4 pixels there is a field. 
       
       public int Rows { get { return 200; } }
       public int Cols { get { return 120; }  } 

        /// <summary>
        /// Generates random vectors for movement in each position of the 2D array
        /// </summary>
        public void GenerateFlowFeild()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {

                }
            }
        }
        /// <summary>
        /// Generates random vectors for movement that point towards a position on the flow field
        /// </summary>
        /// <param name="positionX">Points to a X position in the array</param>
        /// <param name="positionY">Points to a Y position in the array</param>
        public void GenerateFlowField(Vector2 positionX, Vector2 positionY)
        {
            
        }
        //public float[] PerlinNoise(List<float> seed)
        //{

        //    float noise = 0.0f;
        //    List<List<float>> octaves = new List<List<float>>();

        //    List<float> primary = new List<float>();

        //    //todo implement Perlinn Noise
        //    for (int i = 0; i < seed.Count - 1; i++)
        //    {
        //        int sample = i * noise 
        //    }

        //}
    }
   
}
