using System;

namespace u3dExtensions.IOC
{
	public interface IUnsafeBindingContext
	{
		object Get(object name,object key);
	}
}

