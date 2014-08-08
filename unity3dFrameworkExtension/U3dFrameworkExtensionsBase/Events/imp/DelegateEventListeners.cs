using System;

namespace u3dExtensions.Events
{
	public static class DelegateEventListeners
	{
		public static IWeakEventListener<T> LifeTime<T>(Action<T> callback, T target)
		{
			return new WeakEventListener<T>(target,callback);
		}

		public static IEventListener Ever(Action callback)
		{
			return new DelegateEventListener(callback);
		}
			
		public static IEventListener Once (Action callback)
		{
			bool dead = false;
			return new DelegateEventListener(()=> {dead = true; callback();},()=> dead);
		}

		public static IWeakEventListener<T,K>LifeTime<T,K> (Action<T,K> callback, T target)
		{
			return new WeakEventListener<T,K>(target,callback);
		}

		public static IEventListener<T> Ever<T>(Action<T> callback)
		{
			return new DelegateEventListener<T>(callback);
		}
			
		public static IEventListener<T> Once<T>(Action<T> callback)
		{
			bool dead = false;
			return new DelegateEventListener<T>((arg1)=> {dead = true; callback(arg1);},()=> dead);
		}

		public static IWeakEventListener<T,K,J> LifeTime<T,K,J> (Action<T,K,J> callback, T target)
		{
			return new WeakEventListener<T,K,J>(target,callback);
		}

		public static IEventListener<T,K> Ever<T,K>(Action<T,K> callback)
		{
			return new DelegateEventListener<T,K>(callback);
		}
			
		public static IEventListener<T,K> Once<T,K>(Action<T,K> callback)
		{
			bool dead = false;
			return new DelegateEventListener<T,K>((arg1,arg2)=> {dead = true; callback(arg1,arg2);},()=> dead);
		}

		public static IWeakEventListener<T,K,J,W> LifeTime<T,K,J,W> (Action<T,K,J,W> callback, T target)
		{
			return new WeakEventListener<T,K,J,W>(target,callback);
		}

		public static IEventListener<T,K,J> Ever<T,K,J>(Action<T,K,J> callback)
		{
			return new DelegateEventListener<T,K,J>(callback);
		}
			
		public static IEventListener<T,K,J> Once<T,K,J>(Action<T,K,J> callback)
		{
			bool dead = false;
			return new DelegateEventListener<T,K,J>((arg1,arg2,arg3)=> {dead = true; callback(arg1,arg2,arg3);},()=> dead);
		}
			
		public static IEventListener<T,K,J,W> Ever<T,K,J,W>(Action<T,K,J,W> callback)
		{
			return new DelegateEventListener<T,K,J,W>(callback);
		}

		public static IEventListener<T,K,J,W> Once<T,K,J,W>(Action<T,K,J,W> callback)
		{
			bool dead = false;
			return new DelegateEventListener<T,K,J,W>((arg1,arg2,arg3,arg4)=> {dead = true; callback(arg1,arg2,arg3,arg4);},()=> dead);
		}

	}
}

