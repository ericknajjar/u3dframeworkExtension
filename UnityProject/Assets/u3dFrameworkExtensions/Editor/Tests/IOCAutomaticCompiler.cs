using UnityEngine;
using System.Collections;
using NUnit.Framework;
using Moq;
using u3dExtensions.Engine.Editor;
using u3dExtensions.Engine.Runtime;

using System.Collections.Generic;

namespace u3dExtensions.Engine.Tests
{
	[TestFixture]
	public class IOCAutomaticCompiler 
	{
		[Test]
		public void NoProvidersNoError()
		{
			List<IBindingProvider> providers = new List<IBindingProvider>();

			var compiler = new BindingCompiler();

			var diagnostic = compiler.Analise(providers);

			Assert.AreEqual(0,diagnostic.ErrorCount);
		}

		[Test]
		public void ErrorSelfDependencie()
		{
			var moq1 = new Mock<IBindingProvider>();

			List<IBindingProvider> providers = new List<IBindingProvider>();

			moq1.SetupGet((x) => x.Dependencies).Returns(providers);
			providers.Add(moq1.Object);
			
			var compiler = new BindingCompiler();
			
			var diagnostic = compiler.Analise(providers);
			
			Assert.AreEqual(1,diagnostic.ErrorCount);
		}

		[Test]
		public void NoErrosNoDependencies()
		{
			var moq1 = new Mock<IBindingProvider>();
			
			List<IBindingProvider> providers = new List<IBindingProvider>();
			
			moq1.SetupGet((x) => x.Dependencies).Returns(new List<IBindingProvider>());
			providers.Add(moq1.Object);
			
			var compiler = new BindingCompiler();
			
			var diagnostic = compiler.Analise(providers);
			
			Assert.AreEqual(0,diagnostic.ErrorCount);
		}

		[Test]
		public void NoErrosIfFulfilledDependencies()
		{
			var moq1 = new Mock<IBindingProvider>();
			var moq2 = new Mock<IBindingProvider>();

			List<IBindingProvider> providers = new List<IBindingProvider>();
			
			moq1.SetupGet((x) => x.Dependencies).Returns(new List<IBindingProvider>());
			moq2.SetupGet((x) => x.Dependencies).Returns(new List<IBindingProvider>{moq1.Object});

			providers.Add(moq1.Object);
			providers.Add(moq2.Object);

			var compiler = new BindingCompiler();
			
			var diagnostic = compiler.Analise(providers);
			
			Assert.AreEqual(0,diagnostic.ErrorCount);
		}

	}
}
