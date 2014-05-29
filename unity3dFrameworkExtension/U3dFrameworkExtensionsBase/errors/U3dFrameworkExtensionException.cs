using System;

namespace u3dExtensions
{
	public class U3dFrameworkExtensionException: System.Exception
	{
		public U3dFrameworkExtensionException ()
		{

		}

		public U3dFrameworkExtensionException (object mgs): base(mgs.ToString())
		{

		}
	}
}

