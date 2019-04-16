using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;

namespace SymStoreAdapter
{
    abstract class HandlerBase : IHandler
    {
        protected HandlerBase(string[] cmdArgs)
        {
            if (cmdArgs.Length < 1)
                throw new ArgumentException("Commandline have too small amount of arguments");
            CmdParser = new CmdParser(cmdArgs);
        }

        public abstract int Execute();

        public IDisposable Lock()
        {
            var pdbRepository = CmdParser.Value(CmdArgumentOptions.PdbRepositoryPath);
            if (string.IsNullOrWhiteSpace(pdbRepository))
                throw new ArgumentException($"Not found {CmdArgumentOptions.PdbRepositoryPath} commandline argument");
            Directory.CreateDirectory(pdbRepository);
            var lockFile = Path.Combine(pdbRepository, "SymStoreAdapter.lock");
            while (true)
            {
                try
                {
                    var file = File.OpenWrite(lockFile);
                    return new RAII(() =>
                    {
                        try {
                            file.Dispose();
                            File.Delete(lockFile);
                        } catch { }
                    });
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Lock waiting with exception: " + ex);
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                }
            }
        }

        public abstract bool AutoHandled();

        protected CmdParser CmdParser { get; }
    }
}
