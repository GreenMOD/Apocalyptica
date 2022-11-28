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
       
       public int LengthX { get { return 200; } }
       public int LengthY { get { return 120; }  } 

        /// <summary>
        /// Generates random vectors for movement in each position of the 2D array
        /// </summary>
        public void GenerateFlowField()
        {
            for (int i = 0; i < LengthX; i++)
            {
                for (int j = 0; j < LengthY; j++)
                {
                    field[i, j] = 
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
    }
   
}
