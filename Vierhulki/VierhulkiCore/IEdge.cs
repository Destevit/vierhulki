namespace VierhulkiCore
{
    public interface IEdge
    {
        IVertex From { get; set; }
        IVertex To { get; set; }
    }
}