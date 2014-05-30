using System;

namespace u3dExtensions.IOC
{
	public interface IBindingContext
	{
		IValueBindingContext<T> Bind<T>();
		IValueBindingContext<T> Bind<T>(object name);

		T Get<T>();
		T Get<T>(object name);

		IUnsafeBindingContext Unsafe{get;}
	}
}

