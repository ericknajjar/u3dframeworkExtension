using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using u3dExtensions.IOC;

namespace u3dExtensions.Engine.Runtime
{
	public class ReflectiveBindingFinder : IEnumerable<ReflectiveBinding>
	{
		Assembly[] m_assemblies;
		List<ReflectiveBinding> m_providers = new List<ReflectiveBinding>();

		public ReflectiveBindingFinder(params Assembly[] assemblies)
		{
			m_assemblies = assemblies;
			foreach(var assembly in m_assemblies)
			{
				foreach(var type in assembly.GetTypes())
				{
					var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static);
					
					foreach(var method in methods)
					{
						var attributes = method.GetCustomAttributes(typeof(BindingProviderAttribute),false);
						
						if(attributes.Length > 0)
						{
							var toAdd =  CreateReflectiveBinding(method,(BindingProviderAttribute)attributes[0]);
							m_providers.Add(toAdd);
						}
					}
				}
				
			}
		}

		public ReflectiveBinding CreateReflectiveBinding(MethodInfo factory,BindingProviderAttribute attribute)
		{
			var dependencies = new List<BindingPair>();
			
			if(attribute.DependencyCount > 0)
			{
				var parameters = factory.GetParameters();
				
				for(int i=0;i<attribute.DependencyCount;++i)
				{
					var type = parameters[i].ParameterType;
					
					object name = InnerBindingNames.Empty;
					
					if(attribute.DependencieNames.Length > i)
					{
						name = attribute.DependencieNames[i];
					}
					
					var duo = new BindingPair(name, type);
					dependencies.Add(duo);
					
				}
			}

			var root = new BindingPair(attribute.Name,factory.ReturnType);

			return new ReflectiveBinding(root,factory,dependencies);
		}

		#region IEnumerable implementation

		public IEnumerator<ReflectiveBinding> GetEnumerator ()
		{
			return m_providers.GetEnumerator();
		}

		#endregion

		#region IEnumerable implementation

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return m_providers.GetEnumerator();
		}

		#endregion

		class ReflectionBindingProvider: IBindingProvider
		{
			BindingPair m_duo;

			public ReflectionBindingProvider(MethodInfo factory,BindingProviderAttribute attribute): this(new BindingPair(attribute.Name,factory.ReturnType))
			{
				Dependencies = new List<IBindingProvider>();

				if(attribute.DependencyCount > 0)
				{
					var parameters = factory.GetParameters();

					for(int i=0;i<attribute.DependencyCount;++i)
					{
						var type = parameters[i].ParameterType;

						object name = InnerBindingNames.Empty;

						if(attribute.DependencieNames.Length > i)
						{
							name = attribute.DependencieNames[i];
						}

						var duo = new BindingPair(name, type);
						Dependencies.Add( new ReflectionBindingProvider(duo));

					}
				}
			}

			private ReflectionBindingProvider(BindingPair duo)
			{
				m_duo = duo;
				Dependencies = new List<IBindingProvider>();
			}


			#region IBindingProvider implementation

			public IList<IBindingProvider> Dependencies 
			{
				get;
				private set;
			}

			#endregion


			public override bool Equals (object obj)
			{
				if (obj == null)
					return false;
				if (ReferenceEquals (this, obj))
					return true;
				if (obj.GetType () != typeof(ReflectionBindingProvider))
					return false;
				ReflectionBindingProvider other = (ReflectionBindingProvider)obj;

				return m_duo.Equals(other.m_duo);
			}
			

			public override int GetHashCode ()
			{
				unchecked {
					return (m_duo != null ? m_duo.GetHashCode () : 0);
				}
			}
	
		}
	}
}
