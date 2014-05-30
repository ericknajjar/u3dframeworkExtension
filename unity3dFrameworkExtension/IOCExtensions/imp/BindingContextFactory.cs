using System;
using System.Collections.Generic;

namespace u3dExtensions.IOC
{
	public class BindingContextFactory: IBindingContextFactory, IUnsafeBindingContextFactory
	{

		Dictionary<IBindingName,ValueBindingContext> m_namedBindings = new Dictionary<IBindingName, ValueBindingContext>();

		BindingContextFactory()
		{

		}
			
		public static IBindingContextFactory Create()
		{
			return new BindingContextFactory();
		}
		#region IBindingContextFactory implementation

		public IValueBindingContext<T> Bind<T> ()
		{
			return GetBinding(m_namedBindings,new BindingName(InnerBindingNames.Empty),true).As<T>();
		}

		public IValueBindingContext<T> Bind<T> (IBindingName name)
		{
			return GetBinding(m_namedBindings,name,true).As<T>();
		}

		ValueBindingContext GetBinding(Dictionary<IBindingName,ValueBindingContext> bindings,IBindingName name, bool create)
		{
			ValueBindingContext ret = null;

			if(bindings.TryGetValue(name,out ret))
			{
				return ret;
			}

			if(create)
			{
				ret = new ValueBindingContext(name);
				bindings[name] = ret;
				return ret;
			}

			throw new BindingNotFound();
		}

		IBindingContext IBindingContextFactory.GetContext ()
		{
			var dict = m_namedBindings;
			m_namedBindings = new Dictionary<IBindingName, ValueBindingContext>(m_namedBindings);

			var ret = new BindingContext(dict);

			GetBinding(dict,new BindingName(InnerBindingNames.Empty),true);
			GetBinding(dict,new BindingName(InnerBindingNames.CurrentBindingContext),true).To(() => (IBindingContext)ret);;

			return ret;
		}

		IBindingContextFactory IBindingContextFactory.Create (IBindingContextFactory parent)
		{
			return new BindingContextFactory();
		}
	
		public IUnsafeBindingContextFactory Unsafe {
			get {
				return this;
			}
		}
		#endregion

		IUnsafeValueBindingContext IUnsafeBindingContextFactory.Bind (IBindingKey key)
		{
			return GetBinding(m_namedBindings,new BindingName(InnerBindingNames.Empty),true).Unsafe(key);
		}
	
		IUnsafeValueBindingContext IUnsafeBindingContextFactory.Bind (IBindingName name, IBindingKey key)
		{
			return GetBinding(m_namedBindings,name,true).Unsafe(key);
		}
	}
}

