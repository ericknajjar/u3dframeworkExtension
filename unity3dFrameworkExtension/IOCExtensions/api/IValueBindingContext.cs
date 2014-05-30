using System;

namespace u3dExtensions.IOC
{
	public interface IValueBindingContext
	{
		void To<T>(System.Func<T> func);
	}

	public interface IValueBindingContext<T>
	{
		void To<K>(System.Func<K> func) where K:T;

		IValueBindingContext<T,K> With<K> ();
		IValueBindingContext<T,K> With<K> (object name);
	}

	public interface IValueBindingContext<T, J>  
	{
		void To<K>(System.Func<J,K> func) where K:T;

		IValueBindingContext<T,J,K> With<K> ();
		IValueBindingContext<T,J,K> With<K> (IBindingName name);
		IValueBindingContext<T,J,K> With<K> (object name);
	}

	public interface IValueBindingContext<T, J, K>  
	{
		void To<W>(System.Func<J,K,W> func) where W:T;
	}
}

