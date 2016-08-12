namespace Elicon.Domain.Netlist.DataQuery.Traversal
{
    public class TraverseState
    {
        public TraverseState(string currentModuleName)
        {
            CurrentModuleName = currentModuleName;
            InstancesPath = "";
        }

        public TraverseState(TraverseState prevState, Instance currentModule)
        {
            CurrentModuleName = currentModule.CellName;
            InstancesPath = prevState.InstancesPath + ", " + currentModule.InstanceName;
        }

        public string CurrentModuleName { get; set; }
        public string InstancesPath { get; set; }
    }
}