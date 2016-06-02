using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VierhulkiCore
{
    class GraphCreator
    {

        public static void CreateFromMatrix(Vertex[] vertexes, int[,] matrix)
        {
            if (matrix.GetLength(0) != matrix.GetLength(1))
            {
                throw new ArgumentException("Macierz musi być kwadratowa!");
            }

            int length = matrix.GetLength(0);
            
            for (int i = 0; i < length; i++)
            {
                vertexes[i].NextVertexes = new List<Vertex>();
                for (int j = 0; j < length; j++)
                {
                    if (matrix[i, j] == 1)
                    {
                        vertexes[i].NextVertexes.Add(vertexes[j]);
                    }
                }
            }
        }

    }
}
