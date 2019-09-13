using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace slapdash_di
{
	public class ServiceProvider
	{
		private readonly ServiceCollection _registry;
		private readonly ConcurrentDictionary<Type, List<object>> _instantiatedScoped;

		public ServiceProvider(ServiceCollection registry)
		{
			_registry = registry;
			_instantiatedScoped = new ConcurrentDictionary<Type, List<object>>();
		}

		public List<TInterface> GetServices<TInterface>()
		{
			var requestedType = typeof(TInterface);
			if (_instantiatedScoped.TryGetValue(requestedType, out var results))
			{
				return results.Select(x=>(TInterface)x).ToList();
			}


			var services = _registry.GetServices<TInterface>();
			var serviceType = _registry.GetServiceType<TInterface>();

			if (serviceType == ServiceType.Scoped)
			{
				_instantiatedScoped.TryAdd(requestedType, services.Select(x=>(object)x).ToList());
			}

			return services;
		}

		
		public TInterface GetService<TInterface>()
		{
			return GetServices<TInterface>().First();
		}
	}
}