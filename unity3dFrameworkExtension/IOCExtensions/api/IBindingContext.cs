using System;

namespace u3dExtensions.IOC
{
	public interface IBindingContext
	{
		IValueBindingContext Bind<T>();
		IValueBindingContext Bind<T>(object name);

		T Get<T>();
		T Get<T>(object name);
	}
}

