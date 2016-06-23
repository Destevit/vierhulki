using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VierhulkiCore
{
    public class Edge : IEdge
    {
        public Edge()
        {
        }
        public Edge(IVertex from, IVertex to)
        {
            From = from;
            To = to;
        }
        public IVertex From
        {
            get; set;
        }

        public IVertex To
        {
            get; set;
        }
    }
}
