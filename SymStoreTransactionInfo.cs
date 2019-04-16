using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace SymStoreAdapter
{
    class SymStoreTransactionInfo
    {
        public SymStoreTransactionInfo(string transactionLine)
        {
            _splited = transactionLine.Split(',');
        }

        public bool IsValid => _splited?.Length >= 5;

        public string TransactionId => _splited[0];
        public string Operation => _splited[1];
        public string Object => _splited[2];
        public DateTime Date => DateTime.ParseExact(_splited[3] + " " + _splited[4], "MM/dd/yyyy HH:mm:ss",
            CultureInfo.InvariantCulture);

        private readonly string[] _splited;
    }
}
