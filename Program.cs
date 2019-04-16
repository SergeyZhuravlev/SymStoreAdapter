using System;
using System.Linq;

namespace SymStoreAdapter
{
    class Program
    {
        static int Main(string[] cmdArgs)
        {
            try
            {
                Console.WriteLine($@"SymStoreAdapter.exe ""{string.Join(@""" """, cmdArgs)}""");
                var handler = ModeHandlersFactory.Create(cmdArgs);
                if (handler.AutoHandled())
                    return -1;
                using (handler.Lock())
                {
                    return handler.Execute();
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Exception: "+ e);
                return -1;
            }
        }
    }
}
