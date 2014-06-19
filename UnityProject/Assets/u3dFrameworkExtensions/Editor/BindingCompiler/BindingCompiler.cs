using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using u3dExtensions.Engine.Runtime;
using System.Linq;

namespace u3dExtensions.Engine.Editor
{
	public class BindingCompiler 
	{
		public IDiagnostic Analise (IEnumerable<IBindingProvider> providers)
		{

			int globalErrors = 0;
			HashSet<IBindingProvider> allProviders = new HashSet<IBindingProvider>(providers);

			foreach(var provider in providers)
			{
				int errors = provider.Dependencies.Count;

				foreach(var dependency in provider.Dependencies)
				{
					if(!dependency.Equals(provider) && allProviders.Contains(dependency))
					{
						--errors;
					}
				}

				globalErrors+= errors;
			}

			return new Diagnostic(globalErrors);
		}

		class Diagnostic: IDiagnostic
		{
			public Diagnostic(int errors)
			{
				ErrorCount = errors;
			}

			#region IDiagnostic implementation

			public int ErrorCount {
				get;
				private set;
			}

			#endregion


		}
	}
}