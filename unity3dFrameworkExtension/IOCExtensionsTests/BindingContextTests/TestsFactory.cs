using System;
using u3dExtensions.IOC;

namespace IOCExtensionsTests.BindingContextTets
{
	public static class TestsFactory
	{
		//static BindingContext m_context 
		public static IBindingContext BindingContext ()
		{
			return new BindingContext();
		}
	}
}

