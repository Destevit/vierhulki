using GraphX.PCL.Common.Models;
using System.Collections.Generic;
using VierhulkiCore;
using QuickGraph;
using GraphX.PCL.Logic.Models;

namespace VierhulkiGui
{
    public class GuiVertex : VertexBase, IVertex
    {

        long IVertex.ID
        {
            get
            {
                return base.ID;
            }
        }

        public override string ToString()
        {
            return base.ID.ToString();
        }
    }
    public class GuiEdge : EdgeBase<GuiVertex>, IEdge    
    {
        public GuiEdge()
            :base(null,null)
        {

        }
        public GuiEdge(GuiVertex source, GuiVertex target, double weight = 1) : base(source, target, weight)
        {
        }

        public IVertex From
        {
            get
            {
                return Source;
            }

            set
            {
                Source = value as GuiVertex;
            }
        }

        public IVertex To
        {
            get
            {
                return Target;
            }

            set
            {
                Target = value as GuiVertex;
            }
        }
    }
    public class GuiGraph : BidirectionalGraph<GuiVertex, GuiEdge>
    { }

    public class GuiGXLogicCore : GXLogicCore<GuiVertex, GuiEdge, BidirectionalGraph<GuiVertex, GuiEdge>>
    { }
}
