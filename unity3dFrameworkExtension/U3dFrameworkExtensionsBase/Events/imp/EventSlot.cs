using System;
using System.Collections.Generic;
using System.Linq;

namespace u3dFramework.Events
{
	public class EventSlot: IEventRegister,IEventTrigger
	{
		EventSlot<int,int,int,int> m_slot = new EventSlot<int,int,int,int>();

		public EventSlot ()
		{

		}

		#region IEventRegister implementation

		public void Register (IEventListener listener)
		{
			m_slot.Register(listener);
		}

		#endregion

		public void Trigger ()
		{
			m_slot.Trigger(0,0,0,0);
		}
	}

	public class EventSlot<T>: IEventRegister<T>,IEventTrigger<T>
	{
		EventSlot<T,int,int,int> m_slot = new EventSlot<T,int,int,int>();

		public EventSlot ()
		{

		}

		#region IEventRegister implementation

		public void Register (IEventListener listener)
		{
			m_slot.Register(listener);
		}

		public void Register (IEventListener<T> listener)
		{
			var list = new DelegateEventListener<T,int,int,int>((arg1,arg2,arg3,arg4) => listener.Call(arg1), listener.IsDead);
			m_slot.Register(list);
		}

		#endregion

		public void Trigger (T arg1)
		{
			m_slot.Trigger(arg1,0,0,0);
		}
	}

	public class EventSlot<T,K>: IEventRegister<T,K>,IEventTrigger<T,K>
	{
		EventSlot<T,K,int,int> m_slot = new EventSlot<T,K,int,int>();

		public EventSlot ()
		{

		}

		#region IEventRegister implementation

		public void Register (IEventListener listener)
		{
			m_slot.Register(listener);
		}

		public void Register (IEventListener<T,K> listener)
		{
			var list = new DelegateEventListener<T,K,int,int>((arg1,arg2,arg3,arg4) => listener.Call(arg1,arg2), listener.IsDead);
			m_slot.Register(list);
		}

		#endregion

		public void Trigger (T arg1,K arg2)
		{
			m_slot.Trigger(arg1,arg2,0,0);
		}
	}


	public class EventSlot<T,K,J>: IEventRegister<T,K,J>,IEventTrigger<T,K,J>
	{
		EventSlot<T,K,J,int> m_slot = new EventSlot<T,K,J,int>();

		public EventSlot ()
		{

		}

		#region IEventRegister implementation

		public void Register (IEventListener listener)
		{
			m_slot.Register(listener);
		}

		public void Register (IEventListener<T,K,J> listener)
		{
			var list = new DelegateEventListener<T,K,J,int>((arg1,arg2,arg3,arg4) => listener.Call(arg1,arg2,arg3), listener.IsDead);
			m_slot.Register(list);
		}

		#endregion

		public void Trigger (T arg1,K arg2,J arg3)
		{
			m_slot.Trigger(arg1,arg2,arg3,0);
		}
	}

	public class EventSlot<T,K,J,W>: IEventRegister<T,K,J,W>,IEventTrigger<T,K,J,W>
	{
		LinkedList<IEventListener<T,K,J,W>> m_listeners = new LinkedList<IEventListener<T,K,J,W>>();

		public EventSlot ()
		{

		}

		#region IEventRegister implementation

		public void Register (IEventListener listener)
		{
			var list = new DelegateEventListener<T,K,J,W>((arg1,arg2,arg3,arg4) => listener.Call(), listener.IsDead);

			m_listeners.AddLast(list);
		}

		public void Register (IEventListener<T,K,J,W> listener)
		{
			throw new NotImplementedException ();
		}

		#endregion

		public void Trigger (T arg1,K arg2,J arg3,W arg4)
		{
			var next = m_listeners.First;

			LinkedList<IEventListener<T,K,J,W>> tempList = new LinkedList<IEventListener<T,K,J,W>>();
			var oldList = m_listeners;
			m_listeners = tempList;

			try
			{
				while(next!=null)
				{
					if(next.Value.IsDead())
					{
						var newNext = next.Next;
						oldList.Remove(next);
						next = newNext;
					}
					else
					{
						next.Value.Call(arg1,arg2,arg3,arg4);
						if(next.Value.IsDead())
						{
							var newNext = next.Next;
							oldList.Remove(next);
							next = newNext;
						}
						else
							next = next.Next;
					}
				}
			}
			finally
			{
				foreach(var node in m_listeners)
					oldList.AddLast(node);

				m_listeners = oldList;
			}

		}
	}
}

