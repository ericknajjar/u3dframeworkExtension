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
			ret.Bind<IBindingContext>().To(()=>ret);

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
			
		bool IBindingContext.TryGet<T> (out T t, params object[] extras)
		{
			IBindingKey key = new BindingKey(typeof(T));
			var name = new BindingName(InnerBindingNames.Empty);

			return TryGet(name,key,out t,extras);
		}	

		bool IBindingContext.TryGet<T> (IBindingName name, out T t, params object[] extras)
		{
			IBindingKey key = new BindingKey(typeof(T));

			return TryGet(name,key,out t,extras);
		}

	
		public IUnsafeBindingContext Unsafe {
			get 
			{
				return new UnsafeBindingContextAdapter(this);
			}
		}
		#endregion

		bool TryGet<T> (IBindingName name, IBindingKey key,out T t, object[] extras)
		{
			object ret = TryGetBinding (name, key, extras);

			if (ret != null) {
				t = (T)ret;
				return true;
			}

			t = default(T);
			return false;
		}

		object TryGetBinding (IBindingName name, IBindingKey key, object[] extras)
		{
			ValueBindingContext ret = null;

			if (GetBinding (name, out ret))
			{
				object theValue = null;
				if (ret.Get (key, this, out theValue, extras))
				{
					return theValue;
				}
			}

			return null;
		}

		public object Get(IBindingName name,IBindingKey key, params object[] extras)
		{
			object ret = TryGetBinding (name, key, extras);
			if(ret == null) throw new BindingNotFound(name,key);
			return ret;
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

