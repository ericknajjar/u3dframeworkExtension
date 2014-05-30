using System;

namespace u3dExtensions.IOC
{
	public interface IUnsafeBindingContextFactory
	{
		IUnsafeValueBindingContext Bind (IBindingKey key);
		IUnsafeValueBindingContext Bind (IBindingName name,IBindingKey key);
	}
}

