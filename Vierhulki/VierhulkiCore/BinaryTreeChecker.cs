using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VierhulkiCore
{
    public class BinaryTreeChecker
    {
        static public bool Check(Vertex v)
        {
            Vertex current = v;
            Queue<Vertex> kolejka = new Queue<Vertex>();
            List<Vertex> visited = new List<Vertex>();
            kolejka.Enqueue(current);
            do
            {
                current = kolejka.Dequeue();
                visited.Add(current);
                
                foreach (var next in current.NextVertexes)
                {
                    if (!visited.Contains(next))
                    {
                        kolejka.Enqueue(next);
                        if (current.NextVertexes.Count > 3)
                        {
                            return false;
                        }

                    }
                    
                }
            } while (kolejka.Count!=0);
            return true;

        }   
    }
}
