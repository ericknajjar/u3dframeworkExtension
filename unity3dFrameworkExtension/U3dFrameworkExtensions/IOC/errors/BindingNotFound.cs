using System;

namespace u3dExtensions.IOC
{
	public class BindingNotFound: IOCExtensionsException
	{
		public BindingNotFound ()
		{
		}

		public BindingNotFound (object msg): base(msg)
		{
		}
	}
}

