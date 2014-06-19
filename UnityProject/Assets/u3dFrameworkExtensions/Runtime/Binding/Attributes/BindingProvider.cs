using System.Collections;
using System.Collections.Generic;
using System;
using u3dExtensions.IOC;

namespace u3dExtensions.Engine.Runtime
{
	public class BindingProviderAttribute : System.Attribute
	{
		public object Name = InnerBindingNames.Empty;
		public int DependencyCount = 0;
		public object[] DependencieNames = new object[0];
	}


}
