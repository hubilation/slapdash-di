using System;

namespace slapdash_di
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");

			var collection = new ServiceCollection();


			collection.RegisterService<InterfaceA, ConcreteA>(ServiceType.Scoped)
				.RegisterService<InterfaceA, ConcreteA>(ServiceType.Scoped)
				.RegisterService<InterfaceB, ConcreteB>(ServiceType.Singleton);


			var provider = collection.GetProvider();

			var interfaceA = provider.GetService<InterfaceA>();
			var again = provider.GetService<InterfaceA>();

			Console.WriteLine(interfaceA.GetId());
			Console.WriteLine(again.GetId());

			var singleton = provider.GetService<InterfaceB>();

			var newProvider = collection.GetProvider();

			var sameSingleton = newProvider.GetService<InterfaceB>();

			Console.WriteLine($"Singletons from different provider instances are the same:");
			Console.WriteLine($"First: {singleton.GetId()}");
			Console.WriteLine($"Second: {sameSingleton.GetId()}");
		}
	}

}
