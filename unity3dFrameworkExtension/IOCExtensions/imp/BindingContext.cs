using System;
using System.Collections.Generic;

namespace u3dExtensions.IOC
{
	internal partial class BindingContext: IBindingContext
	{
		Dictionary<IBindingName,ValueBindingContext> m_namedBindings;

		public BindingContext(Dictionary<IBindingName,ValueBindingContext> namedBiginds)
		{
			m_namedBindings = namedBiginds;
		}
			
		#region IBindingContext implementation

		public IUnsafeBindingContext Unsafe {
			get 
			{
				return new UnsafeBindingContextAdapter(this);
			}
		}


		T IBindingContext.Get<T> ()
		{
			IBindingKey key = new BindingKey(typeof(T));
			return (T)Get(new BindingName(InnerBindingNames.Empty),key);
		}

		T IBindingContext.Get<T> (IBindingName name)
		{
			IBindingKey key = new BindingKey(typeof(T));
			return (T)Get(name,key);
		}
		#endregion

		public object Get(IBindingName name,IBindingKey key)
		{
			ValueBindingContext ret = GetBinding(name);
			return ret.Get(key,this);
		}
			
		ValueBindingContext GetBinding(IBindingName name)
		{
			ValueBindingContext ret = null;

			if(m_namedBindings.TryGetValue(name,out ret))
			{
				return ret;
			}

			throw new BindingNotFound();
		}

	}
}

