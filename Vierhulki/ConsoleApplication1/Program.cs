using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VierhulkiCore;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Vertex v1 = new Vertex();
            Vertex v2 = new Vertex();
            Vertex v3 = new Vertex();
            Vertex v4 = new Vertex();
            Vertex v5 = new Vertex();
            Vertex v6 = new Vertex();
            Vertex v7 = new Vertex();
            Vertex v8 = new Vertex();

            v1.NextVertexes = new List<Vertex>() { v2, v5 };
            v2.NextVertexes = new List<Vertex>() { v1, v8, v3,v5 };

            bool spr = BinaryTreeChecker.Check(v1);

            Console.WriteLine(spr);
            Console.ReadKey();
        }
        }
    }

