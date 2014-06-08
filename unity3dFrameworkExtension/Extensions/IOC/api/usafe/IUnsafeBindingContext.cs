using System;

namespace u3dExtensions.IOC
{
	public interface IUnsafeBindingContext
	{
		object Get(IBindingName name,IBindingKey key);

		IUnsafeValueBindingContext Bind (IBindingKey key);
		IUnsafeValueBindingContext Bind (IBindingName name,IBindingKey key);
	}
}

