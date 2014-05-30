using System;
using System.Collections.Generic;

namespace u3dExtensions.IOC
{
	public class Binding: IBinding
	{
		Delegate m_factory;
		IBindingRequirement[] m_requirements;

		public Binding(Delegate factory, params IBindingRequirement[] requirement)
		{
			m_requirements = requirement;
			m_factory = factory;
		}

		public object Get(IBindingContext bindingContext)
		{
			List<object> parameters = new List<object>(m_requirements.Length);

			foreach(var requirement in m_requirements)
			{
				var par = requirement.Get(bindingContext);
				parameters.Add(par);
			}

			if(parameters.Count == 0)
			{
				return m_factory.DynamicInvoke(null);
			}
			else
			{
				return m_factory.DynamicInvoke(parameters.ToArray());
			}
		}
	}
}

