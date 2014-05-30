using System;
using System.Collections.Generic;

namespace u3dExtensions.IOC
{
	public partial class BindingContext
	{
		public class ValueBindingContext: IValueBindingContext
		{
			Dictionary<object,IBinding> m_bindings = new Dictionary<object,IBinding>(); 
			public object Name{get;private set;}

			public ValueBindingContext(object name)
			{
				Name = name;
			}

			#region IValueBindingContext implementation
			public void To<T> (Func<T> func)
			{
				m_bindings[typeof(T)] = new Binding(func);
			}
			#endregion

			public IValueBindingContext<T> As<T>()
			{
				return new ValueBingindContextAdapter<T>(this);
			}

			internal void To<T>(IBinding biding)
			{
				m_bindings[typeof(T)] = biding;
			}

			public object Get(object key, IBindingContext currentBindingContext)
			{
				IBinding ret = null;

				if(m_bindings.TryGetValue(key,out ret))
				{
					return ret.Get(currentBindingContext);
				}

				throw new BindingNotFound("With key: "+key);

			}
				
		}


			
	}
}

