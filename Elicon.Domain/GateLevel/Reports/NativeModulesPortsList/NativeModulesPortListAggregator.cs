using System.Collections.Generic;
using System.Linq;
using Elicon.Framework;

namespace Elicon.Domain.GateLevel.Reports.NativeModulesPortsList
{
    public class NativeModulesPortListAggregator
    {
        private readonly Dictionary<string, Instance> _result = new Dictionary<string, Instance>();
       
        public void Collect(Instance instance)
        {
            if (instance.IsModule())
                return;
            
            _result.UpdateValue(instance.ModuleName, 
                i => instance.Net.Count <= i.Net.Count ? i : instance, 
                instance);           
        }
        
        public IList<NativeModulePorts> Result()
        {
            return _result
                .Select(kvp => new NativeModulePorts(kvp.Key, kvp.Value.Net.Select(pwp => pwp.Port).ToList()))
                .ToList();
        }
    }
}