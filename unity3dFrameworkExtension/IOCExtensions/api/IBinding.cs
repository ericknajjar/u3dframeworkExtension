using System;

namespace u3dExtensions.IOC
{
	internal interface IBinding
	{
		object Get(IBindingContext bindingContext);
	}
}

