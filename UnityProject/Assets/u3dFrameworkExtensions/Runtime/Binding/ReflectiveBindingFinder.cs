
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace u3dExtensions.Engine.Runtime
{
	public class ReflectiveBindingFinder : IEnumerable<IBindingProvider>
	{
		Assembly[] m_assemblies;

		public ReflectiveBindingFinder(params Assembly[] assemblies)
		{
			m_assemblies = assemblies;
		}

		#region IEnumerable implementation

		public IEnumerator<IBindingProvider> GetEnumerator ()
		{
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
							yield return new ReflectionBindingProvider(method,(BindingProviderAttribute)attributes[0]);
						}
					}
				}

			}
		}

		#endregion

		#region IEnumerable implementation

		IEnumerator IEnumerable.GetEnumerator ()
		{
			throw new System.NotImplementedException ();
		}

		#endregion

		class ReflectionBindingProvider: IBindingProvider
		{
			BindingProviderAttribute m_attribute;
			public ReflectionBindingProvider(MethodInfo factory,BindingProviderAttribute attribute)
			{
				Dependencies = new List<IBindingProvider>();
			}

			#region IBindingProvider implementation

			public IList<IBindingProvider> Dependencies 
			{
				get;
				private set;
			}

			#endregion
		}
	}
}
