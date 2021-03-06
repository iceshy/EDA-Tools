﻿using System.Collections.Generic;
using Elicon.Domain.GateLevel;
using Elicon.Domain.GateLevel.Manipulations;
using Elicon.Tests.Framework;
using NUnit.Framework;

namespace Elicon.Unit.Tests.Domain.Manipulations
{
    [TestFixture]
    public class InstanceMutatorTests
    {
        private IInstanceMutator _target;

        [SetUp]
        public void SetUp()
        {
            _target = new InstanceMutator();
        }

        [Test]
        public void ReplacePorts_NoMapping_NoReplace()
        {
            var portsMap = new PortsMapping();
            var instances = new List<Instance> { new Instance("netlist","host","name","instName") {
                Net = new List<PortWirePair>() { new PortWirePair("a","w1"), new PortWirePair("b", "w2") }
            }};
            
            _target.Take(instances).ReplacePorts(portsMap);

            Assert.That(instances[0].Net, Has.Count.EqualTo(2));
            Assert.That(instances[0].Net[0].Port, Is.EqualTo("a"));
            Assert.That(instances[0].Net[0].Wire, Is.EqualTo("w1"));
            Assert.That(instances[0].Net[1].Port, Is.EqualTo("b"));
            Assert.That(instances[0].Net[1].Wire, Is.EqualTo("w2"));
        }

        [Test]
        public void ReplacePorts_MappingOfNoExistingPorts_NoReplace()
        {
            var portsMap = new PortsMapping().AddMapping("aa", "apple").AddMapping( "bb", "boom");
            var instances = new List<Instance> {
                new InstanceBuilder("netlist", "host").New("name", "instName")
                .Add("a", "w1")
                .Add("b", "w2").Build()
            };
            
            _target.Take(instances).ReplacePorts(portsMap);

            Assert.That(instances[0].Net, Has.Count.EqualTo(2));
            Assert.That(instances[0].Net[0].Port, Is.EqualTo("a"));
            Assert.That(instances[0].Net[0].Wire, Is.EqualTo("w1"));
            Assert.That(instances[0].Net[1].Port, Is.EqualTo("b"));
            Assert.That(instances[0].Net[1].Wire, Is.EqualTo("w2"));
        }

        [Test]
        public void ReplacePorts_PartialMapping_ReplaceSpecifiedPorts()
        {
            var portsMap = new PortsMapping().AddMapping("a", "apple");
            var instances = new List<Instance> {
                new InstanceBuilder("netlist", "host").New("name", "instName")
                .Add("a", "w1")
                .Add("b", "w2").Build()
            };

            _target.Take(instances).ReplacePorts(portsMap);

            Assert.That(instances[0].Net, Has.Count.EqualTo(2));
            Assert.That(instances[0].Net[0].Port, Is.EqualTo("apple"));
            Assert.That(instances[0].Net[0].Wire, Is.EqualTo("w1"));
            Assert.That(instances[0].Net[1].Port, Is.EqualTo("b"));
            Assert.That(instances[0].Net[1].Wire, Is.EqualTo("w2"));
        }

        [Test]
        public void ReplacePorts_MappingForAllPorts_ReplaceAll()
        {
            var portsMap = new PortsMapping().AddMapping("a", "apple").AddMapping("b", "banana");
            var instances = new List<Instance> {
                new InstanceBuilder("netlist", "host").New("name", "instName")
                .Add("a", "w1")
                .Add("b", "w2").Build()
            };

            _target.Take(instances).ReplacePorts(portsMap);

            Assert.That(instances[0].Net, Has.Count.EqualTo(2));
            Assert.That(instances[0].Net[0].Port, Is.EqualTo("apple"));
            Assert.That(instances[0].Net[0].Wire, Is.EqualTo("w1"));
            Assert.That(instances[0].Net[1].Port, Is.EqualTo("banana"));
            Assert.That(instances[0].Net[1].Wire, Is.EqualTo("w2"));
        }

        [Test]
        public void ReplaceWires_NoPortConnectedToOldWire_NoReplace()
        {
            var instances = new List<Instance> {
                new InstanceBuilder("netlist", "host").New("name", "instName")
                .Add("a", "w1")
                .Add("b", "w2").Build()
            };

            _target.Take(instances).ReplaceWires("w3", "w33");

            Assert.That(instances[0].Net, Has.Count.EqualTo(2));
            Assert.That(instances[0].Net[0].Port, Is.EqualTo("a"));
            Assert.That(instances[0].Net[0].Wire, Is.EqualTo("w1"));
            Assert.That(instances[0].Net[1].Port, Is.EqualTo("b"));
            Assert.That(instances[0].Net[1].Wire, Is.EqualTo("w2"));
        }

        [Test]
        public void ReplaceWires_PortConnectedToOldWire_ReplaceWires()
        {
            var instances = new List<Instance> {
                new InstanceBuilder("netlist", "host").New("name", "instName")
                .Add("a", "w1")
                .Add("b", "w2").Build()
            };

            _target.Take(instances).ReplaceWires("w1", "w11");

            Assert.That(instances[0].Net, Has.Count.EqualTo(2));
            Assert.That(instances[0].Net[0].Port, Is.EqualTo("a"));
            Assert.That(instances[0].Net[0].Wire, Is.EqualTo("w11"));
            Assert.That(instances[0].Net[1].Port, Is.EqualTo("b"));
            Assert.That(instances[0].Net[1].Wire, Is.EqualTo("w2"));
        }

        [Test]
        public void PortsToUpper_OneInstanceTwoPorts_MutateAllPortsToUpper()
        {
            var instances = new List<Instance> {
                new InstanceBuilder("netlist", "host").New("name", "instName")
                .Add("a", "w1")
                .Add("b", "w2").Build()
            };

            _target.Take(instances).PortsToUpper();

            Assert.That(instances[0].Net[0].Port, Is.EqualTo("A"));
            Assert.That(instances[0].Net[1].Port, Is.EqualTo("B"));
        }

        [Test]
        public void PortsToUpper_ManyInstances_MutateAllInstancesPortsToUpper()
        {
            var instances = new List<Instance> {
                new InstanceBuilder("netlist", "host").New("name", "instName1").Add("a", "w1").Build(),
                new InstanceBuilder("netlist", "host").New("name", "instName2").Add("b", "w1").Build(),
                new InstanceBuilder("netlist", "host").New("name", "instName3").Add("c", "w1").Build()
            };
            
            _target.Take(instances).PortsToUpper();

            Assert.That(instances[0].Net[0].Port, Is.EqualTo("A"));
            Assert.That(instances[1].Net[0].Port, Is.EqualTo("B"));
            Assert.That(instances[2].Net[0].Port, Is.EqualTo("C"));
        }

        [Test]
        public void MutateModuleName_OneInstance_MutateModuleName()
        {
            var instances = new List<Instance> {new Instance("netlist", "host", "name", "instName")};

            _target.Take(instances).MutateModuleName("newname");

            Assert.That(instances[0].ModuleName, Is.EqualTo("newname"));
        }

        [Test]
        public void MutateModuleName_ManyInstance_MutateAllInstancesModuleName()
        {
            var instances = new List<Instance> {
                new Instance("netlist", "host", "yada", "instName"),
                new Instance("netlist", "host", "yoda", "instName2"),
                new Instance("netlist", "host", "dojo", "instName3")
            };

            _target.Take(instances).MutateModuleName("newname");

            Assert.That(instances, Has.Exactly(3).Matches<Instance>(i => i.ModuleName == "newname"));
        }
    }
}
