using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SymStoreAdapter
{
    class CleanOldTransactionsHandler: SymStoreExecuteBase
    {
        public CleanOldTransactionsHandler(string[] cmdArgs) : base(cmdArgs)
        {
        }

        public override int Execute()
        {
            var adminDirectory = Path.Combine(_pdbRepositoryPath, "000Admin");
            if (!Directory.Exists(adminDirectory))
                return 0;
            var transactionsFilePath = Path.Combine(adminDirectory, "server.txt");
            bool anyTransactionValid = false;
            DateTime dropTransactionsOlderThen = DateTime.Now.Subtract(_dieAfterAge);
            using(var mirror = new TempFile(transactionsFilePath))
            foreach (var transactionLine in File.ReadLines(mirror.FileName))
            {
                var transaction = new SymStoreTransactionInfo(transactionLine);
                anyTransactionValid = anyTransactionValid || transaction.IsValid;
                if(!transaction.IsValid)
                    continue;
                if (transaction.Operation == "add" && transaction.Object == "file" &&
                    transaction.Date <= dropTransactionsOlderThen)
                    DropSymStoreTransaction(transaction.TransactionId);
            }
            if(!anyTransactionValid)
                throw new Exception("Repository hasn't valid transactions");
            return 0;
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
            if (CmdParser.Value(CmdArgumentOptions.CleanOldTransactionsAfter) is string dieAfterAgeString
                && TimeSpan.TryParse(dieAfterAgeString, out _dieAfterAge)
                && CmdParser.Value(CmdArgumentOptions.PdbRepositoryPath) is string pdbRepositoryPath)
            {
                _pdbRepositoryPath = pdbRepositoryPath;
                return false;
            }

            Console.Error.WriteLine($"This mode {GetType()} required {CmdArgumentOptions.CleanOldTransactionsAfter} and {CmdArgumentOptions.SymStorePath} and {CmdArgumentOptions.PdbRepositoryPath} commandline arguments.");
            return true;
        }

        private TimeSpan _dieAfterAge;
        private string _transactionId;
        private string _pdbRepositoryPath;
    }
}
