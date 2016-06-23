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
            Vertex[] vertices = new Vertex[5];
            List<Edge> edges = new List<Edge>();

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = new Vertex();
            }
            edges.Add(new Edge(vertices[0], vertices[1]));
            edges.Add(new Edge(vertices[0], vertices[2]));
            edges.Add(new Edge(vertices[1], vertices[3]));
            edges.Add(new Edge(vertices[1], vertices[4]));
            edges.Add(new Edge(vertices[1], vertices[0]));
            edges.Add(new Edge(vertices[2], vertices[0]));
            edges.Add(new Edge(vertices[3], vertices[1]));
            edges.Add(new Edge(vertices[4], vertices[1]));

            BinaryTreeChecker<Vertex, Edge> btc = new BinaryTreeChecker<Vertex, Edge>();

            btc.VertexChecking += Btc_VertexChecking;
            btc.EdgeChecking += Btc_EdgeChecking;
            btc.CheckingFailed += Btc_CheckingFailed;

            Console.WriteLine(btc.Check(vertices[0], edges, false));
            
            Console.ReadKey();
        }

        private static void Btc_CheckingFailed(object sender, CheckingFailedEventArgs e)
        {
            if (e != null)
            {
                if (e.FailedVertex != null)
                {
                    Console.WriteLine("Zepsuty wierzchołek: " + e.FailedVertex.ID);
                }
                if (e.FailedEdge != null)
                {
                    Console.WriteLine("Zepsuta krawędź {0} -> {1}", e.FailedEdge.From.ID, e.FailedEdge.To.ID);
                }
                Console.WriteLine("Z powodu: " + e.Message);
            }
        }

        private static void Btc_EdgeChecking(object sender, EdgeCheckingEventArgs e)
        {
            if (e != null)
            {
                Console.WriteLine("Sprawdzanie krawędzi od {0} do {1}", e.EdgeChecked.From.ID, e.EdgeChecked.To.ID);
            }
        }

        private static void Btc_VertexChecking(object sender, VertexCheckingEventArgs e)
        {
            Console.WriteLine("Sprawdzanie wierzchołka {0}", e.VertexChecked.ID);
        }
    }
}
