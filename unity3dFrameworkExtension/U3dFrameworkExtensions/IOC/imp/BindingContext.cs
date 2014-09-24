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
			ValueBindingContext ret = null;
			GetBinding(new BindingName(InnerBindingNames.Empty),true,out ret);

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
			ValueBindingContext ret = null;

			GetBinding(new BindingName(InnerBindingNames.Empty),true,out ret);

			return ret.As<T>();
		}

		IValueBindingContext<T> IBindingContext.Bind<T> (IBindingName name)
		{
			ValueBindingContext ret = null;

			GetBinding(name,true,out ret);
			return ret.As<T>();
		}

		public IUnsafeValueBindingContext Bind(IBindingName name,IBindingKey key)
		{
			ValueBindingContext ret = null;

			GetBinding(name,true,out ret);
			return ret.Unsafe(key);
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
			ValueBindingContext ret = null;

			if(GetBinding(name, out ret))
			{
				object theValue = null;
				if(ret.Get(key,this,out theValue,extras))
				{
					return theValue;
				}
			}
				
			throw new BindingNotFound(name,key);
		}
			

		bool GetBinding(IBindingName name, bool create,out ValueBindingContext valueBindingContext)
		{
			if(m_namedBindings.TryGetValue(name,out valueBindingContext))
			{
				return true;
			}

			if(create)
			{
				valueBindingContext = new ValueBindingContext(name);
				m_namedBindings[name] = valueBindingContext;
				return true;
			}

			valueBindingContext = null;

			return false;
		}

		bool GetBinding(IBindingName name, out ValueBindingContext valueBindingContext)
		{
			return GetBinding(name,false,out valueBindingContext);
		}
	}
}

