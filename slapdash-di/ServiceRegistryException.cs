using System;

namespace slapdash_di
{
	public class ServiceRegistryException : Exception
	{
		public ServiceRegistryException(string message):base(message){}
	}
}