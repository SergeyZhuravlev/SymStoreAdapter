using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymStoreAdapter
{
    class RAII: IDisposable
    {
        public RAII(Action disposer)
        {
            _disposer = disposer;
        }

        public void Dispose()
        {
            var disposer = _disposer;
            _disposer = null;
            disposer?.Invoke();
        }

        private Action _disposer;
    }
}
