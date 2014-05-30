using System;

namespace u3dExtensions.IOC
{
	internal class ObjectBindingRequirement: IBindingRequirement
	{
		object m_name;
		object m_key;

		public ObjectBindingRequirement(object name,object key)
		{
			m_name = name;
			m_key = key;
		}

		#region IBindingRequirement implementation

		object IBindingRequirement.Get (IBindingContext bindingContext)
		{
			return bindingContext.Unsafe.Get(m_name,m_key);
		}

		#endregion
	}

	public static class BindingRequirements
	{
		static public IBindingRequirement With<T>()
		{
			return new ObjectBindingRequirement(InnerBindingNames.Empty,typeof(T));
		}

		static public IBindingRequirement With<T>(object name)
		{
			return new ObjectBindingRequirement(name,typeof(T));
		}
	}
}

