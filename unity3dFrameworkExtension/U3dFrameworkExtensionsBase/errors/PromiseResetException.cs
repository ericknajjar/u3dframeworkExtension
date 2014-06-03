using System;

namespace u3dExtensions
{
	public class PromiseResetException: U3dFrameworkExtensionException
	{
		public PromiseResetException (): base("Attempt to fulfill a promise more than once")
		{

		}
	}
}

