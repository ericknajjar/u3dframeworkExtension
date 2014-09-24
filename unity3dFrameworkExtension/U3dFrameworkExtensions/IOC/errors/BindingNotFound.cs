using System;

namespace u3dExtensions.IOC
{
	public class BindingNotFound: IOCExtensionsException
	{	
		public BindingNotFound (IBindingName bindingName,IBindingKey bindingKey): base(string.Format("With {0}, {1}",bindingName,bindingKey))
		{

		}
	}
}

