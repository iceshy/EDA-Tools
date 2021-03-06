﻿namespace Elicon.Domain.GateLevel.Parse
{
    public class PortsTokenizeRequest
    {
        public PortsTokenizeRequest(string trimmedStatement)
        {
            TrimmedStatement = trimmedStatement;
            MaxLen = trimmedStatement.Length;
        }

        public string TrimmedStatement { get; private set; }
        public bool InEscape { get; set; }
        public bool InBus { get; set; }
        public bool PortDone { get; set; }
        public int EscapePos { get; set; }
        public int StartPos { get; set; }
        public int StringPosition { get; set; }
        public int MaxLen { get; private set; }
        public bool IsPortList { get; set; }
    }
}