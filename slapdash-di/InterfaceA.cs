using System;

namespace slapdash_di
{
	public interface InterfaceA
	{
		Guid GetId();
	}
	
	public interface InterfaceB : InterfaceA
	{

	}

	public class ConcreteA : InterfaceA
	{
		private readonly Guid _id;
		public ConcreteA()
		{
			_id = Guid.NewGuid();
		}

		public Guid GetId()
		{
			return _id;
		}
	}

	public class ConcreteB : InterfaceB
	{
		private readonly Guid _id;
		public ConcreteB()
		{
			_id = Guid.NewGuid();
		}

		public Guid GetId()
		{
			return _id;
		}
	}
}