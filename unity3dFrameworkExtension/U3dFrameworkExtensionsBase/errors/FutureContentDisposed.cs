using System;

namespace u3dExtensions
{
	public class FutureContentDisposed: U3dFrameworkExtensionException
	{
		public FutureContentDisposed (): base("The future on an upper level was already disposed.")
		{

		}
	}
}

