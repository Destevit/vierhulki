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
            Vertex parent = v;
            List<Vertex> visited = new List<Vertex>();
            bool isbinary = true;
            visited.Add(v);
            foreach (var item in v.NextVertexes)
            {

                if (v.NextVertexes.Count > 3)
                {
                    isbinary = false;
                }
                if (visited.Contains(item))
                {
                    if (!(item == parent))
                    {
                        isbinary = false;
                    }

                }
                BinaryTreeChecker.Check(item);

            }
            return isbinary;
        }
    }
}
