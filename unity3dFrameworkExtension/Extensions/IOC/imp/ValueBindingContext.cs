using System;
using System.Collections.Generic;

namespace u3dExtensions.IOC
{
	public partial class BindingContext
	{
		public class ValueBindingContext: IValueBindingContext
		{
			IDictionary<object,IBinding> m_bindings = new Dictionary<object,IBinding>(); 

			public IBindingName Name{get;private set;}

			public ValueBindingContext(IBindingName name)
			{
				Name = name;
			}

			#region IValueBindingContext implementation
			public void To<T> (Func<T> func)
			{
				var binding = new Binding(func);
				To(new BindingKey(typeof(T)),binding);
			}
				
			#endregion

			public IValueBindingContext<T> As<T>()
			{
				return new ValueBingindContextAdapter<T>(this);
			}

			public IUnsafeValueBindingContext Unsafe(IBindingKey key)
			{
				return new UnsafeValueBindindContextAdapter(key,this);
			}

			internal void To<T>(IBinding binding)
			{
				To(new BindingKey(typeof(T)),binding);
			}

			internal void To(IBindingKey key,IBinding biding)
			{
				biding.CheckRequiremets(key,Name);
				m_bindings[key] = biding;
			}

			public object Get(object key, IBindingContext currentBindingContext, params object[] extras)
			{
				IBinding ret = null;

				if(m_bindings.TryGetValue(key,out ret))
				{
					return ret.Get(currentBindingContext,extras);
				}

				throw new BindingNotFound("With key: "+key);
			}
				
		}


			
	}
}

