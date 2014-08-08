using System;

namespace u3dExtensions.Events
{
	public class WeakEventListener<T>: IWeakEventListener<T>
	{
		DelegateEventListener<T> m_eventListener;
		System.WeakReference m_target;

		public WeakEventListener(T target, System.Action<T> callback)
		{
			m_target = new WeakReference(target);
			m_eventListener = new DelegateEventListener<T>(callback,()=> !m_target.IsAlive || m_target.Target == null);
		}

		#region IEventListener implementation

		public void Call ()
		{
			T target = (T)m_target.Target;
			m_eventListener.Call(target);
		}

		public bool IsDead ()
		{
			return m_eventListener.IsDead();
		}

		#endregion
	}

	public class WeakEventListener<T,K>: IWeakEventListener<T,K>
	{
		DelegateEventListener<T,K> m_eventListener;
		System.WeakReference m_target;

		public WeakEventListener(T target, System.Action<T,K> callback)
		{
			m_target = new WeakReference(target);
			m_eventListener = new DelegateEventListener<T,K>(callback,()=> !m_target.IsAlive || m_target.Target == null);
		}

		#region IEventListener implementation

		public void Call (K arg1)
		{
			T target = (T)m_target.Target;
			m_eventListener.Call(target,arg1);
		}

		public bool IsDead ()
		{
			return m_eventListener.IsDead();
		}

		#endregion
	}

	public class WeakEventListener<T,K,J>: IWeakEventListener<T,K,J>
	{
		DelegateEventListener<T,K,J> m_eventListener;
		System.WeakReference m_target;

		public WeakEventListener(T target, System.Action<T,K,J> callback)
		{
			m_target = new WeakReference(target);
			m_eventListener = new DelegateEventListener<T,K,J>(callback,()=> !m_target.IsAlive || m_target.Target == null);
		}

		#region IEventListener implementation

		public void Call (K arg1,J arg2)
		{
			T target = (T)m_target.Target;
			m_eventListener.Call(target,arg1,arg2);
		}

		public bool IsDead ()
		{
			return m_eventListener.IsDead();
		}

		#endregion
	}

	public class WeakEventListener<T,K,J,W>: IWeakEventListener<T,K,J,W>
	{
		DelegateEventListener<T,K,J,W> m_eventListener;
		System.WeakReference m_target;

		public WeakEventListener(T target, System.Action<T,K,J,W> callback)
		{
			m_target = new WeakReference(target);
			m_eventListener = new DelegateEventListener<T,K,J,W>(callback,()=> !m_target.IsAlive || m_target.Target == null);
		}

		#region IEventListener implementation

		public void Call (K arg1,J arg2,W arg3)
		{
			T target = (T)m_target.Target;
			m_eventListener.Call(target,arg1,arg2,arg3);
		}

		public bool IsDead ()
		{
			return m_eventListener.IsDead();
		}

		#endregion
	}
}

