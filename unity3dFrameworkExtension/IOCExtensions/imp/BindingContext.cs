using System;
using System.Collections.Generic;

namespace u3dExtensions.IOC
{
	public partial class BindingContext: IBindingContext
	{
		Dictionary<object,ValueBindingContext> m_namedBindings;

		public BindingContext()
		{
			m_namedBindings = new Dictionary<object,ValueBindingContext>();
			//Creating empty binding
			GetBinding(InnerBindingNames.Empty,true);
		}

		#region IBindingContext implementation

		IValueBindingContext<T> IBindingContext.Bind<T> ()
		{
			return GetBinding(InnerBindingNames.Empty,true).As<T>();
		}

		IValueBindingContext<T> IBindingContext.Bind<T> (object name)
		{
			return GetBinding(name,true).As<T>();
		}

		T IBindingContext.Get<T> ()
		{
			object key = typeof(T);
			return (T)Get(InnerBindingNames.Empty,key);
		}

		T IBindingContext.Get<T> (object name)
		{
			object key = typeof(T);
			return (T)Get(name,key);
		}
	

		public IUnsafeBindingContext Unsafe {
			get 
			{
				return new UnsafeBindingContextAdapter(this);
			}
		}
		#endregion

		public object Get(object name,object key)
		{
			ValueBindingContext ret = GetBinding(name);
			return ret.Get(key,this);
		}

		ValueBindingContext GetBinding(object name, bool create)
		{
			ValueBindingContext ret = null;

			if(m_namedBindings.TryGetValue(name,out ret))
			{
				return ret;
			}

			if(create)
			{
				ret = new ValueBindingContext(name);
				m_namedBindings[name] = ret;
				return ret;
			}

			throw new BindingNotFound();
		}

		ValueBindingContext GetBinding(object name)
		{
			return GetBinding(name,false);
		}

	}
}

