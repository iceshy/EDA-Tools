namespace Elicon.Domain.GateLevel.Contracts.DataAccess
{
    public interface INetlistRepository
    {
        void Add(Netlist netlist);
        Netlist Get(string source);
        void Update(Netlist netlist);
        bool Exists(string source);
    }
}