using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SymStoreAdapter
{
    abstract class SymStoreExecuteBase: HandlerBase
    {
        protected SymStoreExecuteBase(string[] cmdArgs) : base(cmdArgs)
        {
            CheckArguments();
        }

        private void CheckArguments()
        {
            var pdbRepository = CmdParser.Value(CmdArgumentOptions.PdbRepositoryPath);
            if (string.IsNullOrWhiteSpace(pdbRepository))
                throw new ArgumentException($"Not found {CmdArgumentOptions.PdbRepositoryPath} commandline argument");

            var symStorePath = CmdParser.Value(CmdArgumentOptions.SymStorePath);
            if (string.IsNullOrWhiteSpace(symStorePath))
                throw new ArgumentException($"Not found {CmdArgumentOptions.SymStorePath} commandline argument");

            if (!File.Exists(symStorePath))
                throw new ArgumentException($"Not found executable path {CmdArgumentOptions.SymStorePath} in commandline argument");
        }

        protected string SymStoreExecutable()
        {
            return CmdParser.Value(CmdArgumentOptions.SymStorePath) ?? @"symstore.exe";
        }

        protected string SymStoreAllArgs()
        {
            var symStoreArgs = CmdParser.GetSince(CmdArgumentOptions.SymStoreArgs)
                               ?? throw new ArgumentException($"Commandline has no {CmdArgumentOptions.SymStoreArgs}");
            return @"""" + string.Join(@""" """, symStoreArgs) + @"""";
        }

        protected string SymStoreCommandArgs()
        {
            return $"{SymStoreMode()} {SymStoreArgs()}";
        }

        protected abstract string SymStoreMode();
        protected abstract string SymStoreArgs();

        public override int Execute()
        {
            Console.WriteLine($"Starting process '{SymStoreExecutable()}' with args '{SymStoreCommandArgs()}'");
            var startInfo = new System.Diagnostics.ProcessStartInfo()
            {
                FileName = SymStoreExecutable(),
                Arguments = SymStoreCommandArgs(),
                UseShellExecute = false
            };
            var process = System.Diagnostics.Process.Start(startInfo);
            if (process is null)
                throw new Exception($"Can't start process '{SymStoreExecutable()}' with args '{SymStoreCommandArgs()}'");
            using (process)
            {
                process.WaitForExit();
                return process.ExitCode;
            }
        }
    }
}
