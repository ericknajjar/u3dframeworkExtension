using UnityEngine;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;

namespace u3dExtensions.Engine.Runtime
{
	public class ReflectiveBinding  
	{
		public BindingPair Root{get; private set;}
		public MethodInfo Factory{get; private set;}
		public IList<BindingPair> Dependencies{get; private set;}

		public ReflectiveBinding (BindingPair root, MethodInfo factory, IList<BindingPair> dependencies)
		{
			this.Root = root;
			this.Factory = factory;
			this.Dependencies = dependencies;
		}
		
	}
}

