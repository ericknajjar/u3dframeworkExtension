using System;

namespace u3dExtensions.IOC
{
	internal class UnsafeBindingContextAdapter: IUnsafeBindingContext
	{
		BindingContext m_adaptee;

		public UnsafeBindingContextAdapter (BindingContext adaptee)
		{
			m_adaptee = adaptee;
		}


	
		#region IUnsafeBindingContext implementation

		public IUnsafeValueBindingContext Bind (IBindingKey key)
		{
			return m_adaptee.Bind( new BindingName(InnerBindingNames.Empty),key);
		}

		public IUnsafeValueBindingContext Bind (IBindingName name, IBindingKey key)
		{
			return m_adaptee.Bind(name,key);
		}

		public object Get (IBindingName name, IBindingKey key)
		{
			return m_adaptee.Get(name,key);
		}

		#endregion
	}
}

