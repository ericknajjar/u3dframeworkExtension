using NUnit.Framework;
using System;
using u3dExtensions.IOC;
using u3dExtensions.IOC.extesions;

namespace IOCExtensionsTests.BindingContextTets
{
	[TestFixture ()]
	public class Tests
	{
		[Test ()]
		public void BindingSimpleInt ()
		{
			IBindingContextFactory factory = TestsFactory.ContextFactory();

			factory.Bind<int>().To(()=> 45);

			var context = factory.GetContext();

			int ret = context.Get<int>();

			Assert.AreEqual(45,ret);
		}

		[Test ()]
		public void BindingSimpleIntError ()
		{
			var factory = TestsFactory.ContextFactory();
			var context = factory.GetContext();
			Assert.Throws<BindingNotFound>(() => context.Get<int>());
		}

		[Test ()]
		public void NamedBindingInt ()
		{
			var factory = TestsFactory.ContextFactory();

			factory.Bind<int>("coisa").To(()=> 45);

			var context = factory.GetContext();

			int ret = context.Get<int>("coisa");

			Assert.AreEqual(45,ret);
		}

		[Test ()]
		public void NamedBindingIntError()
		{
			var factory = TestsFactory.ContextFactory();
			var context = factory.GetContext();;

			Assert.Throws<BindingNotFound>(() => context.Get<int>("coisa"));
		}

		[Test ()]
		public void NamedBindingDiferentTypeError()
		{
			var factory = TestsFactory.ContextFactory();
		
			factory.Bind<string>("coisa").To(()=> "string");
			var context = factory.GetContext();

			Assert.Throws<BindingNotFound>(() => context.Get<int>("coisa"));
		}

		[Test ()]
		public void NamedBindingDiferentNameError()
		{
			var factory = TestsFactory.ContextFactory();
		
			factory.Bind<int>("coisa").To(()=> 45);

			var context = factory.GetContext();

			Assert.Throws<BindingNotFound>(() => context.Get<int>("coisa2"));
		}

		[Test ()]
		public void OneArgumentBinding()
		{
			var factory = TestsFactory.ContextFactory();

			factory.Bind<int>().With<string>().To((value)=> 45);

			factory.Bind<string>().To(()=> "uhul");

			var context = factory.GetContext();

			int ret = context.Get<int>();

			Assert.AreEqual(45,ret);
		}

		[Test ()]
		public void OneArgumentBindingError()
		{
			//Requires a string to get the int binging
			var factory = TestsFactory.ContextFactory();

			factory.Bind<int>().With<string>().To((value)=> 45);

			var context = factory.GetContext();

			Assert.Throws<BindingNotFound>(() =>context.Get<int>());
		}

		[Test ()]
		public void OneArgumentNamedBinding()
		{
			var factory = TestsFactory.ContextFactory();

			factory.Bind<int>().With<string>("MyText").To((value)=> 45);
			//context.Bind<int>().With<string>("MyText").To((value)=> 45);

			factory.Bind<string>("MyText").To(()=> "uhul");

			var context = factory.GetContext();

			int ret = context.Get<int>();

			Assert.AreEqual(45,ret);
		}

		[Test ()]
		public void OneCorrectArgumentNamedBinding()
		{
			var factory = TestsFactory.ContextFactory();

			string parameter = "";

			factory.Bind<int>().With<string>("MyText").To((value)=>{ parameter = value; return 45;});

			factory.Bind<string>().To(()=> "uhul");

			factory.Bind<string>("MyText").To(()=> "uhul2");

			var context = factory.GetContext();

			context.Get<int>();

			Assert.AreEqual("uhul2",parameter);
		}

		[Test ()]
		public void RightArgumentArgumentBinding()
		{
			var factory = TestsFactory.ContextFactory();

			string parameter = "";

			factory.Bind<int>().With<string>().To((value)=>{ parameter = value; return 45;});

			factory.Bind<string>().To(()=> "uhul");

			var context = factory.GetContext();

			context.Get<int>();

			Assert.AreEqual("uhul",parameter);
		}

		[Test ()]
		public void RequireIteselfError()
		{
			var factory = TestsFactory.ContextFactory();

			Assert.Throws<BindingSelfRequirement>( () => factory.Bind<int>().With<int>().To((value)=> 45) );

		}

		[Test ()]
		public void RequireIteselWithSameNameError()
		{
			var factory = TestsFactory.ContextFactory();

			Assert.Throws<BindingSelfRequirement>( () => factory.Bind<int>("name").With<int>("name").To((value)=> 45));
		
		}

		[Test ()]
		public void RequireIteselWithDifferenNameNoError()
		{
			var factory = TestsFactory.ContextFactory();

			Assert.DoesNotThrow( () => factory.Bind<int>("name").With<int>("differentName").To((value)=> 45));
		}

		[Test ()]
		public void TwoArgumentBinding()
		{
			var factory = TestsFactory.ContextFactory();


			factory.Bind<int>().With<string>().With<float>().To((str,flt)=> 45);

			factory.Bind<string>().To(()=> "uhul");

			factory.Bind<float>().To(()=> 3.0f);
			var context = factory.GetContext();

			int ret = context.Get<int>();

			Assert.AreEqual(45,ret);
		}

		[Test ()]
		public void TwoRequireIteselWithSameNameError()
		{
			var factory = TestsFactory.ContextFactory();

			Assert.Throws<BindingSelfRequirement>( () => factory.Bind<int>().With<string>().With<int>().To((str,flt)=> 45));


		}

		[Test ()]
		public void TowRequireIteselWithDifferenNameNoError()
		{
			var factory = TestsFactory.ContextFactory();

			Assert.DoesNotThrow( () => factory.Bind<int>("name").With<string>().With<int>("diferentName").To((str,flt)=> 45));
		}

		[Test ()]
		public void TowRequireIteselWithDifferenNameNoError2()
		{
			var factory = TestsFactory.ContextFactory();

			Assert.DoesNotThrow( () => factory.Bind<int>("name").With<int>().With<int>().To((str,flt)=> 45));
		}

		[Test ()]
		public void UnsafeSimpleBiding()
		{
			var factory = TestsFactory.ContextFactory();

			System.Func<int> func = () => 45;
			IBinding binding = new Binding(func);

			factory.Unsafe.Bind(typeof(int)).To(binding);
			var context = factory.GetContext();

			Assert.AreEqual(45,context.Get<int>());
		}

		[Test ()]
		public void UnsafeNamedBiding()
		{
			var factory = TestsFactory.ContextFactory();

			System.Func<int> func = () => 45;
			IBinding binding = new Binding(func);

			factory.Unsafe.Bind("name",typeof(int)).To(binding);
			var context = factory.GetContext();

			Assert.AreEqual(45,context.Get<int>("name"));
		}


		[Test ()]
		public void UnsafeSelfRequiremetBidingError()
		{
			var factory = TestsFactory.ContextFactory();
			IBindingRequirement requirement = BindingRequirements.Instance.With<int>();

			System.Func<int> func = () => 45;

			IBinding binding = new Binding(func,requirement);

			Assert.Throws<BindingSelfRequirement>(() => factory.Unsafe.Bind(typeof(int)).To(binding));
		}

		[Test ()]
		public void GetItself()
		{
			var factory = TestsFactory.ContextFactory();
			var context = factory.GetContext();


			IBindingContext meAgain = context.Get<IBindingContext>(InnerBindingNames.CurrentBindingContext);

			Assert.AreEqual(context,meAgain);

		}
	}

}

