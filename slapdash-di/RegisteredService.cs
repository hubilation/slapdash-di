using System;

namespace slapdash_di
{
	public class RegisteredService
	{
		public RegisteredService(Type type, ServiceType serviceType, Func<object> constructor)
		{
			Type = type;
			ServiceType = serviceType;
			Constructor = constructor;
		}

		public RegisteredService(Type type, ServiceType serviceType) : this(type, serviceType, ()=>Activator.CreateInstance(type)){}

		public Type Type { get; set; }
		public Func<object> Constructor { get; set; }
		public ServiceType ServiceType { get; set; }

	}
}