namespace Elicon.Domain.Netlist.BuildData
{
    public class BuildState
    {
        public string NetlistSource { get; set; }
        public string CurrentModuleName { get; set; }
        public string CurrentStatement { get; set; }
    }
}