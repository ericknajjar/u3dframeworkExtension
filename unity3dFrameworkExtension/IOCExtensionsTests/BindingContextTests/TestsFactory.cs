using System;
using u3dExtensions.IOC;

namespace IOCExtensionsTests.BindingContextTets
{
	public static class TestsFactory
	{
		public static IBindingContextFactory ContextFactory ()
		{
			IBindingContextFactory factory = BindingContextFactory.Create();

			return factory;
		}
	}
}

