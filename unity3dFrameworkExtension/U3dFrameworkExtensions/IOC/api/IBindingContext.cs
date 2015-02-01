using System;

namespace u3dExtensions.IOC
{
	public interface IBindingContext
	{
		IValueBindingContext<T> Bind<T>();
		IValueBindingContext<T> Bind<T>(IBindingName name);

		T Get<T>();
		T Get<T>(IBindingName name,params object[] extras);

		bool TryGet<T>(out T t,params object[] extras);
		bool TryGet<T>(IBindingName name,out T t,params object[] extras);

		IUnsafeBindingContext Unsafe{get;}
	}
}

