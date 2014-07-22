using System;

namespace u3dFramework.Events
{
	public static class DelegateEventListeners
	{
		public static IEventListener LifeTime (Action callback, object target)
		{
			System.WeakReference w = new WeakReference(target);

			return new DelegateEventListener(callback,() => {

				return w.IsAlive || w.Target == null;
			});
		}

		public static IEventListener Ever(Action callback, object target)
		{
			return new DelegateEventListener(callback);
		}

		public static IEventListener Auto(Action callback, object target)
		{
			return LifeTime(callback,callback.Target);
		}

		public static IEventListener Once (Action callback)
		{
			bool dead = false;
			return new DelegateEventListener(()=> {dead = true; callback();},()=> dead);
		}

		public static IEventListener<T> LifeTime<T> (Action<T> callback, object target)
		{
			System.WeakReference w = new WeakReference(target);

			return new DelegateEventListener<T>(callback,() => {

				return w.IsAlive || w.Target == null;
			});
		}

		public static IEventListener<T> Ever<T>(Action<T> callback, object target)
		{
			return new DelegateEventListener<T>(callback);
		}

		public static IEventListener<T> Auto<T>(Action<T> callback, object target)
		{
			return LifeTime(callback,callback.Target);
		}

		public static IEventListener<T> Once<T>(Action<T> callback)
		{
			bool dead = false;
			return new DelegateEventListener<T>((arg1)=> {dead = true; callback(arg1);},()=> dead);
		}

		public static IEventListener<T,K> LifeTime<T,K> (Action<T,K> callback, object target)
		{
			System.WeakReference w = new WeakReference(target);

			return new DelegateEventListener<T,K>(callback,() => {

				return w.IsAlive || w.Target == null;
			});
		}

		public static IEventListener<T,K> Ever<T,K>(Action<T,K> callback, object target)
		{
			return new DelegateEventListener<T,K>(callback);
		}

		public static IEventListener<T,K> Auto<T,K>(Action<T,K> callback, object target)
		{
			return LifeTime(callback,callback.Target);
		}

		public static IEventListener<T,K> Once<T,K>(Action<T,K> callback)
		{
			bool dead = false;
			return new DelegateEventListener<T,K>((arg1,arg2)=> {dead = true; callback(arg1,arg2);},()=> dead);
		}

		public static IEventListener<T,K,J> LifeTime<T,K,J> (Action<T,K,J> callback, object target)
		{
			System.WeakReference w = new WeakReference(target);

			return new DelegateEventListener<T,K,J>(callback,() => {

				return w.IsAlive || w.Target == null;
			});
		}

		public static IEventListener<T,K,J> Ever<T,K,J>(Action<T,K,J> callback, object target)
		{
			return new DelegateEventListener<T,K,J>(callback);
		}

		public static IEventListener<T,K,J> Auto<T,K,J>(Action<T,K,J> callback, object target)
		{
			return LifeTime(callback,callback.Target);
		}

		public static IEventListener<T,K,J> Once<T,K,J>(Action<T,K,J> callback)
		{
			bool dead = false;
			return new DelegateEventListener<T,K,J>((arg1,arg2,arg3)=> {dead = true; callback(arg1,arg2,arg3);},()=> dead);
		}

		public static IEventListener<T,K,J,W> LifeTime<T,K,J,W> (Action<T,K,J,W> callback, object target)
		{
			System.WeakReference w = new WeakReference(target);

			return new DelegateEventListener<T,K,J,W>(callback,() => {

				return w.IsAlive || w.Target == null;
			});
		}

		public static IEventListener<T,K,J,W> Ever<T,K,J,W>(Action<T,K,J,W> callback, object target)
		{
			return new DelegateEventListener<T,K,J,W>(callback);
		}

		public static IEventListener<T,K,J,W> Auto<T,K,J,W>(Action<T,K,J,W> callback, object target)
		{
			return LifeTime(callback,callback.Target);
		}

		public static IEventListener<T,K,J,W> Once<T,K,J,W>(Action<T,K,J,W> callback)
		{
			bool dead = false;
			return new DelegateEventListener<T,K,J,W>((arg1,arg2,arg3,arg4)=> {dead = true; callback(arg1,arg2,arg3,arg4);},()=> dead);
		}
	}
}

