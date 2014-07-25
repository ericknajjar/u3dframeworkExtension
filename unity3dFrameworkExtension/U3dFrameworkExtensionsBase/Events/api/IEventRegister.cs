using System;

namespace u3dExtensions.Events
{
	public interface IEventRegister
	{
		void Register(IEventListener listener);
	}

	public interface IEventRegister<T>: IEventRegister
	{
		void Register(IEventListener<T> listener);
	}

	public interface IEventRegister<T,K>: IEventRegister
	{
		void Register(IEventListener<T,K> listener);
	}

	public interface IEventRegister<T,K,J>: IEventRegister
	{
		void Register(IEventListener<T,K,J> listener);
	}

	public interface IEventRegister<T,K,J,W>: IEventRegister
	{
		void Register(IEventListener<T,K,J,W> listener);
	}

	public static class IEventRegisterExtensions
	{
		public static void Register(this IEventRegister me, System.Action callback)
		{
			me.Register(new DelegateEventListener(callback));
		}

		public static void Register<T>(this IEventRegister<T> me, System.Action<T> callback)
		{
			me.Register(new DelegateEventListener<T>(callback));
		}

		public static void Register<T,J>(this IEventRegister<T,J> me, System.Action<T,J> callback)
		{
			me.Register(new DelegateEventListener<T,J>(callback));
		}

		public static void Register<T,J,K>(this IEventRegister<T,J,K> me, System.Action<T,J,K> callback)
		{
			me.Register(new DelegateEventListener<T,J,K>(callback));
		}

		public static void Register<T,J,K,W>(this IEventRegister<T,J,K,W> me, System.Action<T,J,K,W> callback)
		{
			me.Register(new DelegateEventListener<T,J,K,W>(callback));
		}
	}
		
}

