using System;
using System.Collections.Generic;
using System.Text;

namespace SymStoreAdapter
{
    class ModeHandlersFactory
    {
        public static IHandler Create(string[] cmdArgs)
        {
            if (cmdArgs.Length < 1)
                throw new ArgumentException("Commandline have too small amount of arguments");
            switch (cmdArgs[0])
            {
                case CmdMode.CleanTransactionsWhileRepositorySizeOverflow:
                    return new CleanTransactionsWhileRepositorySizeOverflowHandler(cmdArgs);
                case CmdMode.RecursiveAddFromDirectoryWithFilter:
                    return new RecursiveAddFromDirectoryWithFilterHandler(cmdArgs);
                case CmdMode.CleanOldTransactions:
                    return new CleanOldTransactionsHandler(cmdArgs);
                default:
                    return new SymStoreExecute(cmdArgs);
            }
        }
    }
}
