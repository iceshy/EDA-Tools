﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Elicon.Config;
using Elicon.Domain.GateLevel;
using Elicon.Domain.GateLevel.BuildData;
using Elicon.Domain.GateLevel.Contracts.DataAccess;
using Elicon.Domain.GateLevel.Manipulations.RemoveBuffer;
using Elicon.Domain.GateLevel.Manipulations.ReplaceLibraryGate;
using Elicon.Domain.GateLevel.Manipulations.UpperCaseLibraryGatesPorts;
using Elicon.Domain.GateLevel.Reports.CountLibraryGates;
using Elicon.Domain.GateLevel.Reports.ListLibraryGates;
using Elicon.Domain.GateLevel.Reports.ListPhysicalPaths;

namespace EdaTools.Utility
{

    public class ToolRunnerEventArgs : EventArgs
    {
        public ToolRunnerEventArgs():this(false, "")
        {
        }

        public ToolRunnerEventArgs(bool error, string errorMessage)
        {
            Error = error;
            ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; set; }
        public bool Error { get; set; }
    }

    public class ToolRunner
    {
        public event EventHandler<ToolRunnerEventArgs> TaskRunningFinished;
        private ToolRunnerEventArgs _toolRunnerEventArgs;

        public ToolRunner(EventHandler<ToolRunnerEventArgs> taskRunningFinished)
        {
            TaskRunning = false;
            TaskRunningFinished += taskRunningFinished;
        }

        public bool TaskRunning { get; private set; }


        public async void ListLibraryGates(string netlist, string targetSaveFile)
        {
            InitRunner();
            var report = Bootstrapper.Get<IListLibraryGatesReport>();
            await Task.Run(() =>
            {
                try
                {
                    report.GenerateReport(netlist, targetSaveFile);
                }
                catch (Exception ex)
                {
                    SetErrorData(ex);
                }
            });
            OnTaskRunningFinished(_toolRunnerEventArgs);
        }

        public async void CountLibraryGatesInstancesCommand(string netlist, string rootModule, string targetSaveFile)
        {
            InitRunner();
            var report = Bootstrapper.Get<ICountLibraryGatesReport>();        
            await Task.Run(() =>
            {
                try
                {
                    report.GenerateReport(netlist, rootModule, targetSaveFile);
                }
                catch (Exception ex)
                {
                    SetErrorData(ex);
                }
            });
            OnTaskRunningFinished(_toolRunnerEventArgs);
        }

        public async void ListPhysicalPathsCommand(string netlist, string rootModule, string moduleNames, string targetSaveFile)
        {
            InitRunner();
            var modules = moduleNames.CommaSeparatedStringToList();
            var report = Bootstrapper.Get<IListPhysicalPathsReport>();
            await Task.Run(() =>
            {
                try
                {
                    report.GenerateReport(netlist, rootModule, modules, targetSaveFile);
                }
                catch (Exception ex)
                {
                    SetErrorData(ex);
                }
            });
            OnTaskRunningFinished(_toolRunnerEventArgs);
        }

        public async void ReplaceLibraryGateCommand(LibraryGateReplaceRequest replaceRequest)
        {
            InitRunner();
            var action = Bootstrapper.Get<ILibraryGateReplaceManipulation>();
            await Task.Run(() =>
            {
                try
                {
                    action.Replace(replaceRequest);
                }
                catch (Exception ex)
                {
                    SetErrorData(ex);
                }
            });
            OnTaskRunningFinished(_toolRunnerEventArgs);
        }

        public async void RemoveBuffersCommand(RemoveBufferRequest removeBufferRequest)
        {
            InitRunner();
            var action = Bootstrapper.Get<IRemoveBufferManipulation>();
            await Task.Run(() =>
            {
                try
                {
                    action.Remove(removeBufferRequest);
                }
                catch (Exception ex)
                {
                    SetErrorData(ex);
                }
            });
            OnTaskRunningFinished(_toolRunnerEventArgs);
        }

        public async void UpperCaseLibraryGatesPortsCommand(string netlist, string targetSaveFile)
        {
            InitRunner();
            var action = Bootstrapper.Get<IUpperCaseLibraryGatesPortsManipulation>();
            await Task.Run(() =>
            {
                try
                {
                    action.PortsToUpper(new UpperCasePortsRequest {SourceNetlist = netlist, TargetNetlist = targetSaveFile});
                }
                catch (Exception ex)
                {
                    SetErrorData(ex);
                }
            });
            OnTaskRunningFinished(_toolRunnerEventArgs);
        }

        public async void ReadNetlist(string netlistReadFilePath)
        {
            InitRunner();
            var action = Bootstrapper.Get<INetlistDataBuilder>();
            await Task.Run(() =>
            {
                try
                {
                    action.Build(netlistReadFilePath);
                }
                catch (Exception ex)
                {
                    SetErrorData(ex);
                }
            });
            OnTaskRunningFinished(_toolRunnerEventArgs);
        }

        public IEnumerable<Netlist> GetNetlists()
        {
            var repository = Bootstrapper.Get<INetlistRepository>();
            return repository.GetAll();
        }

        private void InitRunner()
        {
            TaskRunning = true;
            _toolRunnerEventArgs = new ToolRunnerEventArgs(false, "test");
        }

        private void SetErrorData(Exception ex)
        {
            _toolRunnerEventArgs.Error = true;
            _toolRunnerEventArgs.ErrorMessage = ex.GetType() == typeof (OutOfMemoryException) ? 
                $"ERROR - Unexpected failure (probably due to your system configuration).{Environment.NewLine}Let us help and fix this issue ! Please send us (HW, OS, Input files) at http://www.elicon.biz/community." : 
                ex.FormatMessage();
        }

        private void OnTaskRunningFinished(ToolRunnerEventArgs e)
        {
            TaskRunning = false;
            var handler = TaskRunningFinished;
            handler?.Invoke(this, e);
        }
    }
}
