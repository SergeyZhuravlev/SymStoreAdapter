using System;
using System.Collections.Generic;
using System.Text;

namespace SymStoreAdapter
{
    interface IHandler
    {
        int Execute();
        IDisposable Lock();
        bool AutoHandled();
    }
}
