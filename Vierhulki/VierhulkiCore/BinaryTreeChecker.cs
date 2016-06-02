using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VierhulkiCore
{
    public class BinaryTreeChecker
    {
        List<Vertex> visited = new List<Vertex>();

        public bool Check(Vertex w)
        {
            Boolean isbinary = true;
            visited.Add(w);
            foreach (var item in w.NextVertexes)
            {
                if (w.NextVertexes.Count > 3)
                {
                    isbinary = false;
                }
                if (!w.NextVertexes.Contains(item))
                {
                    Check(item);
                }
            }            
          return isbinary;
            Console.WriteLine(isbinary);
        }
    }
}
