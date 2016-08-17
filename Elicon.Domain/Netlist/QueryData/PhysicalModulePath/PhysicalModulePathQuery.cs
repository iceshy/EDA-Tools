﻿using System.Collections.Generic;
using Elicon.Domain.Netlist.QueryData.Traversal;

namespace Elicon.Domain.Netlist.QueryData.PhysicalModulePath
{
    public interface IPhysicalModulePathQuery
    {
        IDictionary<string, IList<string>> GetPhysicalPaths(string netlist, string rootModule, IList<string> moduleNames);
    }
        
    public class PhysicalModulePathQuery : IPhysicalModulePathQuery
    {
        private readonly INetlistTraverser _netlistTraverser;

        public PhysicalModulePathQuery(INetlistTraverser netlistTraverser)
        {
            _netlistTraverser = netlistTraverser;
        }

        public IDictionary<string, IList<string>> GetPhysicalPaths(string netlist, string rootModule, IList<string> moduleNames)
        {
            var aggregator = new PhysicalModulePathAggregator();
            aggregator.SetModulesToTrack(moduleNames);
            
            foreach (var traversalState in _netlistTraverser.Traverse(netlist, rootModule))
                aggregator.Collect(traversalState);

            return aggregator.Result();
        }
    }
}
