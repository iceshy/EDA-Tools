using Elicon.Domain.Netlist.DataQuery.Traversal;

namespace Elicon.Domain.Netlist.DataQuery.Visitors
{
    public interface IInstanceVisitor
    {
        void Visit(Instance instance, TraverseState traverseState);
    }
}