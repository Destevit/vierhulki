using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VierhulkiCore
{
    public class BinaryTreeChecker<TVertex, TEdge> where TEdge : IEdge, new() where TVertex : IVertex
    {
        public event EventHandler<VertexCheckingEventArgs> VertexChecking;
        public event EventHandler<EdgeCheckingEventArgs> EdgeChecking;
        public event EventHandler<CheckingFailedEventArgs> CheckingFailed;

        public bool Check(IVertex root, IEnumerable<IEdge> edges, bool directed)
        {
            IVertex current = root;
            Queue<IVertex> queue = new Queue<IVertex>();
            Dictionary<IVertex, IVertex> visited = new Dictionary<IVertex, IVertex>();

            visited.Add(current, null);
            queue.Enqueue(current);

            do
            {
                current = queue.Dequeue();
                OnVertexChecking(current);
                List<IEdge> currentEdges = (from foundEdge in edges
                                            where foundEdge.From == current
                                            select foundEdge).ToList();

                bool correctNumberOfNeighbours = false;
                if (directed)
                    correctNumberOfNeighbours = currentEdges.Count <= 2;
                else
                    correctNumberOfNeighbours = currentEdges.Count <= 3;

                if (correctNumberOfNeighbours)
                {
                    bool parentFound = false;
                    foreach (IEdge currentEdge in currentEdges)
                    {
                        OnEdgeChecking(currentEdge);
                        if (!directed && visited[current] == currentEdge.To)
                        {
                            if (parentFound)
                            {
                                OnCheckingEdgeFailed(currentEdge, "Podwójna krawędź do rodzica.");
                                return false;
                            }
                            else
                            {
                                parentFound = true;
                            }
                        }
                        else
                        {
                            if (visited.Keys.Contains(currentEdge.To))
                            {
                                OnCheckingEdgeFailed(currentEdge, "Nieprawidłowy powrót do odwiedzonego wierzchołka.");
                                return false;
                            }
                            else
                            {
                                visited.Add(currentEdge.To, current);
                                queue.Enqueue(currentEdge.To);
                            }
                        }
                    }
                    if (!directed && current != root && !parentFound)
                    {
                        OnCheckingVertexFailed(current, "Nie znaleziono referencji do rodzica.");
                        return false;
                    }
                }
                else
                {
                    OnCheckingVertexFailed(current, "Nieprawidłowa liczba krawędzi z wierzchołka.");
                    return false;
                }
            } while (queue.Count > 0);

            return true;
        }

        protected virtual void OnCheckingVertexFailed(IVertex current, string message)
        {
            CheckingFailed?.Invoke(this,
                new CheckingFailedEventArgs { FailedVertex = current, Message = message });
        }

        protected virtual void OnCheckingEdgeFailed(IEdge edge, string message)
        {
            CheckingFailed?.Invoke(this,
                new CheckingFailedEventArgs
                {
                    FailedVertex = edge.From,
                    FailedEdge = edge,
                    Message = message
                }
                );
        }

        protected virtual void OnEdgeChecking(IEdge edge)
        {
            EdgeChecking?.Invoke(this,
                new EdgeCheckingEventArgs { EdgeChecked = edge }
                );
        }

        protected virtual void OnVertexChecking(IVertex vertex)
        {
            VertexChecking?.Invoke(this,
                new VertexCheckingEventArgs { VertexChecked = vertex }
                );
        }
    }
    public class VertexCheckingEventArgs : EventArgs
    {
        public IVertex VertexChecked { get; set; }
    }
    public class EdgeCheckingEventArgs : EventArgs
    {
        public IEdge EdgeChecked { get; set; }
    }
    public class CheckingFailedEventArgs : EventArgs
    {
        public IEdge FailedEdge { get; set; }
        public IVertex FailedVertex { get; set; }
        public string Message { get; set; }
    }
}
