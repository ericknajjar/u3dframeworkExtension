using System;
using System.Collections.Generic;

namespace u3dExtensions.IOC
{
	public partial class BindingContext: IBindingContext
	{
		IDictionary<IBindingName,ValueBindingContext> m_namedBindings;

		BindingContext()
		{
			m_namedBindings = new Dictionary<IBindingName,ValueBindingContext>();
			//Creating empty binding
			GetBinding(new BindingName(InnerBindingNames.Empty),true);

		}

		static public IBindingContext Create()
		{
			IBindingContext ret = new BindingContext();
			ret.Bind<IBindingContext>(new BindingName(InnerBindingNames.CurrentBindingContext)).To(()=>ret);
			return ret;
		}

		#region IBindingContext implementation

		IValueBindingContext<T> IBindingContext.Bind<T> ()
		{
			return GetBinding(new BindingName(InnerBindingNames.Empty),true).As<T>();
		}

		IValueBindingContext<T> IBindingContext.Bind<T> (IBindingName name)
		{
			return GetBinding(name,true).As<T>();
		}

		public IUnsafeValueBindingContext Bind(IBindingName name,IBindingKey key)
		{
			return GetBinding(name,true).Unsafe(key);
		}

		T IBindingContext.Get<T> (IBindingName name, params object[] extras)
		{
			IBindingKey key = new BindingKey(typeof(T));
			return (T)Get(name,key,extras);
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
	

		public IUnsafeBindingContext Unsafe {
			get 
			{
				return new UnsafeBindingContextAdapter(this);
			}
		}
		#endregion

		public object Get(IBindingName name,IBindingKey key, params object[] extras)
		{
			ValueBindingContext ret = GetBinding(name);
			return ret.Get(key,this,extras);
		}
			

		ValueBindingContext GetBinding(IBindingName name, bool create)
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

		ValueBindingContext GetBinding(IBindingName name)
		{
			return GetBinding(name,false);
		}
	}
}

