using System.Collections.Generic;
using Elicon.Domain.GateLevel.Traversal.PhysicalTraversal;
using Elicon.Framework;

namespace Elicon.Domain.GateLevel.Reports.CountNativeModules
{
    public class NativeModulesCountAggregator
    { 
        private readonly Dictionary<string, long> _result = new Dictionary<string, long>();

        public void Collect(TraversalState traversalState)
        {
            if (traversalState.CurrentInstance.IsModule())
                return;

            _result.UpdateValue(traversalState.CurrentInstance.ModuleName, count => ++count, 0);
        }

        public IDictionary<string, long> Result()
        {
            return _result;
        }
    }
}