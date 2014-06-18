using System;

namespace u3dExtensions.IOC
{
	public interface IBindingContext
	{
		IValueBindingContext<T> Bind<T>();
		IValueBindingContext<T> Bind<T>(IBindingName name);

		T Get<T>();
		T Get<T>(IBindingName name);
		T Get<T>(IBindingName name,params object[] extras);

		IUnsafeBindingContext Unsafe{get;}
	}
}

