using NUnit.Framework;
using System;
using u3dExtensions.IOC;
using u3dExtensions.IOC.extesions;

namespace u3dExtensions.Tests.BindingContextTets
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

		[Test ()]
		public void NamedBindingIntError()
		{
			IBindingContext context = TestsFactory.BindingContext();

			Assert.Throws<BindingNotFound>(() => context.Get<int>("coisa"));
		}

		[Test ()]
		public void NamedBindingDiferentTypeError()
		{
			IBindingContext context = TestsFactory.BindingContext();

			context.Bind<string>("coisa").To(()=> "string");

			Assert.Throws<BindingNotFound>(() => context.Get<int>("coisa"));
		}

		[Test ()]
		public void NamedBindingDiferentNameError()
		{
			IBindingContext context = TestsFactory.BindingContext();

			context.Bind<int>("coisa").To(()=> 45);

			Assert.Throws<BindingNotFound>(() => context.Get<int>("coisa2"));
		}

		[Test ()]
		public void OneArgumentBinding()
		{
			IBindingContext context = TestsFactory.BindingContext();

			context.Bind<int>().With<string>().To((value)=> 45);

			context.Bind<string>().To(()=> "uhul");

			int ret = context.Get<int>();

			Assert.AreEqual(45,ret);
		}

		[Test ()]
		public void OneArgumentBindingError()
		{
			//Requires a string to get the int binging

			IBindingContext context = TestsFactory.BindingContext();

			context.Bind<int>().With<string>().To((value)=> 45);

			Assert.Throws<BindingNotFound>(() =>context.Get<int>());
		}

		[Test ()]
		public void OneArgumentNamedBinding()
		{
			IBindingContext context = TestsFactory.BindingContext();

			context.Bind<int>().With<string>("MyText").To((value)=> 45);
			//context.Bind<int>().With<string>("MyText").To((value)=> 45);

			context.Bind<string>("MyText").To(()=> "uhul");

			int ret = context.Get<int>();

			Assert.AreEqual(45,ret);
		}

		[Test ()]
		public void OneCorrectArgumentNamedBinding()
		{
			IBindingContext context = TestsFactory.BindingContext();

			string parameter = "";

			context.Bind<int>().With<string>("MyText").To((value)=>{ parameter = value; return 45;});

			context.Bind<string>().To(()=> "uhul");

			context.Bind<string>("MyText").To(()=> "uhul2");

			context.Get<int>();

			Assert.AreEqual("uhul2",parameter);
		}

		[Test ()]
		public void RightArgumentArgumentBinding()
		{
			IBindingContext context = TestsFactory.BindingContext();

			string parameter = "";

			context.Bind<int>().With<string>().To((value)=>{ parameter = value; return 45;});

			context.Bind<string>().To(()=> "uhul");

			context.Get<int>();

			Assert.AreEqual("uhul",parameter);
		}

		[Test ()]
		public void RequireIteselfError()
		{
			IBindingContext context = TestsFactory.BindingContext();

			Assert.Throws<BindingSelfRequirement>( () => context.Bind<int>().With<int>().To((value)=> 45) );

		}

		[Test ()]
		public void RequireIteselWithSameNameError()
		{
			IBindingContext context = TestsFactory.BindingContext();

			Assert.Throws<BindingSelfRequirement>( () => context.Bind<int>("name").With<int>("name").To((value)=> 45));
		
		}

		[Test ()]
		public void RequireIteselWithDifferenNameNoError()
		{
			IBindingContext context = TestsFactory.BindingContext();

			Assert.DoesNotThrow( () => context.Bind<int>("name").With<int>("differentName").To((value)=> 45));
		}

		[Test ()]
		public void TwoArgumentBinding()
		{
			IBindingContext context = TestsFactory.BindingContext();

			context.Bind<int>().With<string>().With<float>().To((str,flt)=> 45);

			context.Bind<string>().To(()=> "uhul");

			context.Bind<float>().To(()=> 3.0f);

			int ret = context.Get<int>();

			Assert.AreEqual(45,ret);
		}

		[Test ()]
		public void TwoRequireIteselWithSameNameError()
		{
			IBindingContext context = TestsFactory.BindingContext();

			Assert.Throws<BindingSelfRequirement>( () => context.Bind<int>().With<string>().With<int>().To((str,flt)=> 45));


		}

		[Test ()]
		public void TowRequireIteselWithDifferenNameNoError()
		{
			IBindingContext context = TestsFactory.BindingContext();

			Assert.DoesNotThrow( () => context.Bind<int>("name").With<string>().With<int>("diferentName").To((str,flt)=> 45));
		}

		[Test ()]
		public void TowRequireIteselWithDifferenNameNoError2()
		{
			IBindingContext context = TestsFactory.BindingContext();

			Assert.DoesNotThrow( () => context.Bind<int>("name").With<int>().With<int>().To((str,flt)=> 45));
		}

		[Test ()]
		public void UnsafeSimpleBiding()
		{
			IBindingContext context = TestsFactory.BindingContext();

			System.Func<int> func = () => 45;
			IBinding binding = new Binding(func);

			context.Unsafe.Bind(typeof(int)).To(binding);

			Assert.AreEqual(45,context.Get<int>());
		}

		[Test ()]
		public void UnsafeNamedBiding()
		{
			IBindingContext context = TestsFactory.BindingContext();

			System.Func<int> func = () => 45;
			IBinding binding = new Binding(func);

			context.Unsafe.Bind("name",typeof(int)).To(binding);

			Assert.AreEqual(45,context.Get<int>("name"));
		}


		[Test ()]
		public void UnsafeSelfRequiremetBidingError()
		{
			IBindingContext context = TestsFactory.BindingContext();
			IBindingRequirement requirement = BindingRequirements.Instance.With<int>();

			System.Func<int> func = () => 45;

			IBinding binding = new Binding(func,requirement);

			Assert.Throws<BindingSelfRequirement>(() => context.Unsafe.Bind(typeof(int)).To(binding));
		}
	}

}

