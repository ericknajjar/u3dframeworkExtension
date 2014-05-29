using System;

namespace u3dExtensions.IOC
{
	public interface IValueBindingContext
	{
		void To<T>(System.Func<T> func);
	}
}

