﻿using System;

namespace Core.Extensions.AuthExtension
{
    public class TransactionStateProvider : IWorkContextStateProvider
    {
        public const string StateName = "TransactionState";

        private TransactionState _state = null;

        public TransactionStateProvider()
        {
            _state = new TransactionState();
        }

        public Func<WorkContext, object> Get(string name)
        {
            if (StateName.Equals(name))
            {
                return (context) => _state ?? (_state = new TransactionState());
            }
            return null;
        }
    }

    internal sealed class TransactionState
    {
        public int ChainCount;
        public int CommiteCount;
        public int RollbackCount;
    }
}
