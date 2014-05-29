using System;
using System.Collections.Generic;

namespace u3dExtensions.IOC
{
	public class BindingContext: IBindingContext
	{
		ValueBingindContext m_valueBingindContext = new ValueBingindContext();
		Dictionary<object,ValueBingindContext> m_namedBindings = new Dictionary<object,ValueBingindContext>();

		public BindingContext()
		{

		}

		#region IBindingContext implementation

		IValueBindingContext IBindingContext.Bind<T> ()
		{
			return m_valueBingindContext;
		}

		IValueBindingContext IBindingContext.Bind<T> (object name)
		{
			ValueBingindContext ret = null;
			object key = typeof(T);

			if(!m_namedBindings.TryGetValue(key,out ret))
			{
				ret = new ValueBingindContext();

				m_namedBindings[name] = ret;
			}

			return ret;
		}

		T IBindingContext.Get<T> ()
		{
			return (T)m_valueBingindContext.Get(typeof(T));
		}

		T IBindingContext.Get<T> (object name)
		{
			return (T)m_namedBindings[name].Get(typeof(T));
		}
	
		#endregion

		class ValueBingindContext: IValueBindingContext
		{
			Dictionary<object,Delegate> m_bindings = new Dictionary<object,Delegate>(); 

			#region IValueBindingContext implementation
			void IValueBindingContext.To<T> (Func<T> func)
			{
				m_bindings[typeof(T)] = func;
			}
			#endregion

			public object Get(object key)
			{
				Delegate ret = null;

				if(m_bindings.TryGetValue(key,out ret))
				{
					return ret.DynamicInvoke(null);
				}

				throw new BindingNotFound();

			}
		}
	}
}

