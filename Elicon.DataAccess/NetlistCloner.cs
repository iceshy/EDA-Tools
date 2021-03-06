using System;
using Elicon.Domain.GateLevel.Contracts.DataAccess;

namespace Elicon.DataAccess
{
    public class NetlistCloner : INetlistCloner
    {
        private readonly INetlistRepository _netlistRepository;
        private readonly IModuleRepository _moduleRepository;
        private readonly IInstanceRepository _instanceRepository;
        private readonly INetlistRemover _netlistRemover;

        public NetlistCloner(INetlistRepository netlistRepository, IModuleRepository moduleRepository, IInstanceRepository instanceRepository, INetlistRemover netlistRemover)
        {
            _netlistRepository = netlistRepository;
            _moduleRepository = moduleRepository;
            _instanceRepository = instanceRepository;
            _netlistRemover = netlistRemover;
        }

        public void Clone(string source, string newSource)
        {
            var netlist = _netlistRepository.Get(source);
            if (netlist == null)
                throw new InvalidOperationException("source netlist does not exists");

            if (source == newSource)
                return;

            _netlistRemover.Remove(newSource);

            netlist.Source = newSource;
            _netlistRepository.Add(netlist);

            foreach (var module in _moduleRepository.GetAll(source))
            {
                module.Netlist = newSource;
                _moduleRepository.Add(module);
            }

            foreach (var instance in _instanceRepository.GetBy(source))
            {
                instance.Netlist = newSource;
                _instanceRepository.Add(instance);
            }
        }
    }
}
