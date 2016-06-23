
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VierhulkiCore
{
    public class Vertex : IVertex
    {
        private static long _ID = 0;
        public Vertex()
        {
            ID = _ID++;
        }
        public long ID { get; private set; }
        public override string ToString()
        {
            return ID.ToString();
        }
    }
}