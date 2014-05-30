using System;

namespace u3dExtensions.IOC
{
	public interface IBindingContext
	{
		T Get<T>();
		T Get<T>(IBindingName name);

		IUnsafeBindingContext Unsafe{get;}
	}
}

