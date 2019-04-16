using System;

namespace SymStoreAdapter
{
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                var handler = ModeHandlersFactory.Create(args);
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
