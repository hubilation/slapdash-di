using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace slapdash_di
{
	public class ServiceCollection
	{
		private ConcurrentDictionary<Type, List<RegisteredService>> _registeredServices;
		private ConcurrentDictionary<Type, List<object>> _instantiatedSingletons;

		public ServiceCollection()
		{
			_registeredServices = new ConcurrentDictionary<Type, List<RegisteredService>>();
			_instantiatedSingletons = new ConcurrentDictionary<Type, List<object>>();
		}

		public ServiceCollection RegisterService<TInterface, TImplementation>(ServiceType serviceType)
		{
			var registry = new RegisteredService(typeof(TImplementation), serviceType);

			if (_registeredServices.TryGetValue(typeof(TInterface), out var services))
			{
				if (services.Any(x => x.ServiceType != serviceType))
				{
					throw new ServiceRegistryException("When registering multiple services to the same interface, they must be the same ServiceType");
				}

				services.Add(registry);
				return this;
			}

			_registeredServices.TryAdd(typeof(TInterface), new List<RegisteredService>() {registry});
			return this;
		}

		public ServiceType GetServiceType<TInterface>()
		{
			var requestedType = typeof(TInterface);
			if (_registeredServices.TryGetValue(requestedType, out var results))
			{
				return results.First().ServiceType;
			}

			throw new ServiceRegistryException($"Unable to locate registration for {typeof(TInterface)}");
		}

		public List<TInterface> GetServices<TInterface>()
		{
			var requestedType = typeof(TInterface);
			if (_registeredServices.TryGetValue(requestedType, out var results))
			{
				if (results.All(x => x.ServiceType == ServiceType.Singleton))
				{
					if (_instantiatedSingletons.TryGetValue(requestedType, out var constructedSingletons))
					{
						return constructedSingletons.Select(x=>(TInterface)x).ToList();
					}

					var instantiated = results.Select(x => x.Constructor.Invoke()).ToList();
					_instantiatedSingletons.TryAdd(requestedType, instantiated);
					return instantiated.Select(x=>(TInterface)x).ToList();
				}

				return results.Select(x => (TInterface) x.Constructor.Invoke()).ToList();
			}

			throw new ServiceRegistryException($"Unable to locate registration for {typeof(TInterface)}");
		}


		public ServiceProvider GetProvider()
		{
			return new ServiceProvider(this);
		}
	}
}