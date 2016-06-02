using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VierhulkiCore
{
    public class Vertex
    {
        public List<Vertex> NextVertexes { get; set; } = new List<Vertex>();
    }
}
