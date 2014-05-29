using NUnit.Framework;
using System;
using u3dExtensions.IOC;

namespace IOCExtensionsTests.BindingContextTets
{
	[TestFixture ()]
	public class Tests
	{
		[Test ()]
		public void BindingSimpleInt ()
		{
			IBindingContext context = TestsFactory.BindingContext();

			context.Bind<int>().To(()=> 45);

			int ret = context.Get<int>();

			Assert.AreEqual(45,ret);
		}

		[Test ()]
		public void BindingSimpleIntError ()
		{
			IBindingContext context = TestsFactory.BindingContext();
			Assert.Throws<BindingNotFound>(() => context.Get<int>());
		}

		[Test ()]
		public void NamedBindingInt ()
		{
			IBindingContext context = TestsFactory.BindingContext();

			context.Bind<int>("coisa").To(()=> 45);

			int ret = context.Get<int>("coisa");

			Assert.AreEqual(45,ret);
		}
	}
}

