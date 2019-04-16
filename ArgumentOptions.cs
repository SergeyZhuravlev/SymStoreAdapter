using System;
using System.Collections.Generic;
using System.Text;

namespace SymStoreAdapter
{
    static class CmdArgumentOptions
    {
        public const string PdbRepositoryPath = @"/s";

        public const string SymStorePath = @"/SymStorePath";
        public const string SymStoreArgs = @"/SymStoreArgs";

        //public const string Help = @"/?";
        public const string CleanOldTransactionsAfter = @"/After";
    }
}
