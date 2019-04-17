using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SymStoreAdapter
{
    class CleanTransactionsWhileRepositorySizeOverflowHandler : SymStoreExecuteBase
    {
        public CleanTransactionsWhileRepositorySizeOverflowHandler(string[] cmdArgs) : base(cmdArgs)
        {
        }

        public override int Execute()
        {
            var adminDirectory = Path.Combine(_pdbRepositoryPath, "000Admin");
            if (!Directory.Exists(adminDirectory))
                return 0;
            var transactionsFilePath = Path.Combine(adminDirectory, "server.txt");
            bool anyTransactionValid = false;
            long linesAmount = 0;
            using (var mirror = new TempFile(transactionsFilePath))
                foreach (var transactionLine in File.ReadLines(mirror.FileName))
                {
                    ++linesAmount;
                    var transaction = new SymStoreTransactionInfo(transactionLine);
                    anyTransactionValid = anyTransactionValid || transaction.IsValid;
                    if (!transaction.IsValid)
                        continue;
                    if (transaction.Operation == "add" && transaction.Object == "file")
                    {
                        if (DirectorySize(_pdbRepositoryPath) > _maxSizeInBytes)
                            DropSymStoreTransaction(transaction.TransactionId);
                        else
                            break;
                    }
                }
            if (linesAmount > 1 && !anyTransactionValid)
                throw new Exception("Repository hasn't valid transactions");
            return 0;
        }

        private static long DirectorySize(string pdbRepositoryPath)
        {
            IWshRuntimeLibrary.FileSystemObject FSO = new IWshRuntimeLibrary.FileSystemObject();
            using (new RAII(() => Marshal.FinalReleaseComObject(FSO)))
            {
                var directory = FSO.GetFolder(pdbRepositoryPath);
                using (new RAII(() => Marshal.FinalReleaseComObject(directory)))
                {
                    var directorySize = (long)directory.Size;
                    Console.WriteLine($"Directory size for '{pdbRepositoryPath}' = {directorySize} bytes ({directorySize / (1024.0 * 1024 * 1024)} GBs).");
                    return directorySize;
                }
            }
        }

        protected override string SymStoreMode()
        {
            return "del";
        }

        protected override string SymStoreArgs()
        {
            return $"/i \"{_transactionId}\" /o /s \"{_pdbRepositoryPath}\"";
        }

        private void DropSymStoreTransaction(string transactionId)
        {
            _transactionId = transactionId;
            base.Execute();
        }

        public override bool AutoHandled()
        {
            if (CmdParser.Value(CmdArgumentOptions.MaxSizeInGb) is string maxSizeInGbString
                && long.TryParse(maxSizeInGbString, out var maxSizeInGb)
                && CmdParser.Value(CmdArgumentOptions.PdbRepositoryPath) is string pdbRepositoryPath)
            {
                _pdbRepositoryPath = pdbRepositoryPath;
                _maxSizeInBytes = maxSizeInGb * 1024 * 1024 * 1024;
                return false;
            }

            Console.Error.WriteLine($"This mode {GetType()} required {CmdArgumentOptions.MaxSizeInGb} and {CmdArgumentOptions.SymStorePath} and {CmdArgumentOptions.PdbRepositoryPath} commandline arguments.");
            return true;
        }

        private long _maxSizeInBytes;
        private string _transactionId;
        private string _pdbRepositoryPath;
    }
}