using NUnit.Framework;
using System;
using u3dExtensions.IOC;
using u3dExtensions.IOC.extesions;

namespace u3dExtensions.Tests.BindingTests
{
	[TestFixture ()]
	public class Tests
	{
		[Test ()]
		public void BindingRequirementEquality()
		{

			IBindingRequirement requirement = BindingRequirements.Instance.With<int>();
			IBindingRequirement requirementb = BindingRequirements.Instance.With(InnerBindingNames.Empty,typeof(int));

			Assert.AreEqual(requirement,requirementb);
		}

		[Test ()]
		public void BindingCheckForErrors()
		{

			IBindingRequirement requirement = BindingRequirements.Instance.With<int>();
	
			System.Func<int> func = () => 45;

			IBinding binding = new Binding(func,requirement);

			Assert.Throws<BindingSelfRequirement>(() => binding.CheckRequiremets(typeof(int),InnerBindingNames.Empty));
		}

	}
}

