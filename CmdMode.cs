using System;
using System.Collections.Generic;
using System.Text;

namespace SymStoreAdapter
{
    static class CmdMode
    {
        public const string CleanOldTransactions = "CleanOldTransactions";
        public const string CleanTransactionsWhileRepositorySizeOverflow = "CleanTransactionsWhileRepositorySizeOverflow";
        public const string RecursiveAddFromDirectoryWithFilter = "RecursiveAddFromDirectoryWithFilter";
    }
}
