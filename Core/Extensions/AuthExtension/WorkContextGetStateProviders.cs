using System.Collections.Generic;

namespace Core.Extensions.AuthExtension
{
    public class WorkContextGetStateProviders : WorkContext
    {
        private readonly IWorkContextStateProvider _transactionProvider = new TransactionStateProvider();
        protected override IEnumerable<IWorkContextStateProvider> GetStateProviders()
        {
            return new IWorkContextStateProvider[]
            {
                    _transactionProvider
            };
        }
    }
}
