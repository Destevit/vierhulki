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
            kolejka.Enqueue(current);
            do
            {
                current = kolejka.Dequeue();
                foreach (var next in current.NextVertexes)
                {
                    kolejka.Enqueue(next);
                    if (current.NextVertexes.Count > 3)
                    {
                        return false;
                    }
                }
            } while (kolejka.Count!=0);
            return true;

        }   
    }
}
