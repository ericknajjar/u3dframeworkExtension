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

		public object Get (object name, object key)
		{
			return m_adaptee.Get(name,key);
		}

		#endregion
	}
}

