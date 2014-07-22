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

		public static IEventListener Once (Action callback)
		{
			bool dead = false;
			return new DelegateEventListener(()=> {dead = true; callback();},()=> dead);
		}
	}
}

