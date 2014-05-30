using System;

namespace u3dExtensions.IOC
{
	public interface IBindingContextFactory
	{
		IValueBindingContext<T> Bind<T>();
		IValueBindingContext<T> Bind<T>(IBindingName name);

		IBindingContext GetContext();

		IBindingContextFactory Create(IBindingContextFactory parent);

		IUnsafeBindingContextFactory Unsafe{get;}
	}
}

