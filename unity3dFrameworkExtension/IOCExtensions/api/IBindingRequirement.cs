using System;

namespace u3dExtensions.IOC
{
	public interface IBindingRequirement
	{
		object Get(IBindingContext bindingContext);
	}

}

