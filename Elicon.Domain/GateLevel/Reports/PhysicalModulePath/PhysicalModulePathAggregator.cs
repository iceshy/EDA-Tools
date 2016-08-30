using System.Collections.Generic;
using Elicon.Domain.GateLevel.Traversal.PhysicalTraversal;

namespace Elicon.Domain.GateLevel.Reports.PhysicalModulePath
{
    public class PhysicalModulePathAggregator
    {
        private readonly Dictionary<string, IList<string>> _result  = new Dictionary<string, IList<string>>();

        public PhysicalModulePathAggregator(IList<string> moduleNamesToCollect)
        {
            foreach (var moduleName in moduleNamesToCollect)
                _result.Add(moduleName, new List<string>());
        }

        public void Collect(TraversalState traversalState)
        {
            if (IsModuleToTrack(traversalState.CurrentInstance))
                AddPath(traversalState);
        }

        public IDictionary<string, IList<string>> Result()
        {
            return _result;
        }

        private void AddPath(TraversalState traversalState)
        {
            _result[traversalState.CurrentInstance.ModuleName].Add(traversalState.InstancesPathTracker.ToString());
        }
        
        private bool IsModuleToTrack(Instance instance)
        {
            return _result.ContainsKey(instance.ModuleName);
        }
    }
}