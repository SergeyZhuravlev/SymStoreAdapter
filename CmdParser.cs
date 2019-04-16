using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SymStoreAdapter
{
    class CmdParser
    {
        public CmdParser(string[] cmdArgs)
        {
            CmdArgs = cmdArgs.ToList();
        }

        public bool IsKey(string name)
        {
            return CmdArgs.Any(_ => _ == name);
        }

        public string Value(string name)
        {
            if (!IsKey(name))
                return null;
            var value = GetSince(name)
                .FirstOrDefault();
            if (string.IsNullOrEmpty(value) || new [] {'/', '-'}.Contains(value[0]))
                return null;
            return value;
        }

        public IEnumerable<string> GetSince(string name)
        {
            return CmdArgs
                .SkipWhile(_ => _ != name)
                .Skip(1)
                .ToArray();
        }

        public IReadOnlyCollection<string> CmdArgs { get; }

        public bool IsArgumentBefore(string checkedArgument, string beforeArgument)
        {
            return CmdArgs
                .Reverse()
                .SkipWhile(_ => _ != beforeArgument)
                .Any(_ => _ == checkedArgument)
                && 
                CmdArgs
                .SkipWhile(_ => _ != beforeArgument)
                .All(_ => _ != checkedArgument);
        }
    }
}
