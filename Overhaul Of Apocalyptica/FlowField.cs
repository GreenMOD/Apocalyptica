using System;
using System.Numerics;

namespace ExtensionTasks
{
    public class FlowField
    {

        private int _rows = 0;

        private int _columns = 0;


        public Vector2[,] Field;


        public FlowField(int rows, int columns, Vector2 target)
        {
            Field = new Vector2[rows, columns];
            _rows = rows;
            _columns = columns;

            GenerateField(target);
        }

        public void GenerateField(Vector2 target)
        {

            Field[(int)target.X, (int)target.Y] = Vector2.Zero;

            for (int y = 0; y < _columns; y++)
            {
                for (int x = 0; x < _rows; x++)
                {

                    if (target != new Vector2(x, y))
                    {
                        Vector2 desired = Vector2.Subtract(target, new Vector2(x, y));
                        desired = Vector2.Normalize(desired);
                        Field[x, y] = desired;
                    }

                }
            }


            DisplayField();
        }
        public void DisplayField()
        {
            for (int y = 0; y < _columns; y++)
            {
                Console.WriteLine("");
                Console.WriteLine("--------------------------------------------------------------------------------------------------------------");
                for (int x = 0; x < _rows; x++)
                {
                    Console.Write(Field[x, y] + "   ");

                }
            }
        }
    }
}
