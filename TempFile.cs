using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymStoreAdapter
{
    class TempFile: IDisposable
    {
        public TempFile()
        {
            FileName = Path.GetTempFileName();
        }

        public TempFile(string copyFromPath)
            : this()
        {
            File.Copy(copyFromPath, FileName, true);
        }

        public string FileName { get; }

        public void Dispose()
        {
            try {
                File.Delete(FileName);
            } catch { }
        }
    }
}
