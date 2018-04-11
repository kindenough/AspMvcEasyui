using System;

namespace Zephyr.Data
{
	public interface IEntityFactory
	{
		object Create(Type type);
	}
}
