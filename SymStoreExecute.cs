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
            return SymStoreAllArgs();
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
