using System.Collections;
using u3dExtensions;
using u3dExtensions.IOC;
using u3dExtensions.IOC.extesions;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System;

namespace u3dExtensions.Engine.Runtime
{
	public class ReflectiveBindingContextFactory 
	{
		IEnumerable<ReflectiveBinding> m_bindings;

		public ReflectiveBindingContextFactory(IEnumerable<ReflectiveBinding> bindings)
		{
			m_bindings = bindings;
		}

		public IBindingContext CreateContext()
		{
			var context = BindingContext.Create();

			foreach(var binding in m_bindings)
			{
				List<IBindingRequirement> requirements = new List<IBindingRequirement>();

				foreach(var req in binding.Dependencies)
				{
					var realReq = BindingRequirements.Instance.With(req.name,req.BindingType);
					requirements.Add(realReq);
				}

			
				var args = new List<Type>(binding.Factory.GetParameters().Select(p => p.ParameterType));
				args.Add(binding.Factory.ReturnType);

				var delegateType = Expression.GetFuncType(args.ToArray());

				var factory = System.Delegate.CreateDelegate(delegateType,binding.Factory);

				IBinding realBinding = new Binding(factory,requirements.ToArray());

				context.Unsafe.Bind(binding.Root.name,binding.Root.BindingType).To(realBinding);
			}

			return context;
		}
	}
}

