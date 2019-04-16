using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SymStoreAdapter
{
    class SymStoreExecute : SymStoreExecuteBase
    {
        public override bool AutoHandled()
        {
            return false;
        }

        protected override string SymStoreArgs()
        {
            var symStoreArgs = CmdParser.GetSince(CmdArgumentOptions.SymStoreArgs)
                               ?? throw new ArgumentException($"Commandline has no {CmdArgumentOptions.SymStoreArgs}");
            return string.Join(" ", symStoreArgs.Select(_ => "\"" + _ + "\"").ToArray());
        }

        protected override string SymStoreMode()
        {
            return CmdParser.CmdArgs.First();
        }

        public SymStoreExecute(string[] cmdArgs) : base(cmdArgs)
        {
        }
    }
}
