using System;

namespace u3dExtensions.IOC
{
	public interface IBinding
	{
		object Get(IBindingContext bindingContext);
		void CheckRequiremets(IBindingKey key, IBindingName name);
	}
}

