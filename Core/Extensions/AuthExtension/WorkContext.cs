using Core.Extentions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Core.Extensions.AuthExtension
{
    public abstract class WorkContext : IDisposable
    {
        private readonly ConcurrentDictionary<string, object> _stateResolvers;
        private IEnumerable<IWorkContextStateProvider> _workContextStateProviders;
        private bool _disposed;

        public WorkContext()
        {
            _stateResolvers = new ConcurrentDictionary<string, object>();
        }
        protected abstract IEnumerable<IWorkContextStateProvider> GetStateProviders();

        private IEnumerable<IWorkContextStateProvider> Providers
        {
            get { return _workContextStateProviders ?? (_workContextStateProviders = this.GetStateProviders() ?? Enumerable.Empty<IWorkContextStateProvider>()); }
        }
        /// <summary>
        /// 获取或设置当前用户。
        /// </summary>
        public UserSession CurrentUser
        {
            get { return GetState<UserSession>("CurrentUser"); }
            set { SetState("CurrentUser", value); }
        }
        public void SetState<T>(string name, T value)
        {
            _stateResolvers[name] = value;
        }
        public virtual T GetState<T>(string name)
        {
            var factory = this.FindResolverForState<T>(name);
            var state = _stateResolvers.GetOrAdd(name, k => factory());
            return (T)state;
        }
        private Func<object> FindResolverForState<T>(string name)
        {
            var resolver = this.Providers.Select(wcsp => wcsp.Get(name)).FirstOrDefault(value => value != null);

            if (resolver == null)
            {
                return () => default(T);
            }
            return () => resolver(this);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~WorkContext()
        {
            //必须为false
            Dispose(false);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            _disposed = true;
            if (disposing)
            {
                var providers = _stateResolvers.Values.OfType<IDisposable>().ToArray();
                foreach (var p in providers)
                {
                    p.Dispose();
                }
            }
        }
    }

}
