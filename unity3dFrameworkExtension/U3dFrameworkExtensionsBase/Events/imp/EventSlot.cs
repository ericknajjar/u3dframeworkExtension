using System;
using System.Collections.Generic;
using System.Linq;

namespace u3dFramework.Events
{
	public class EventSlot: IEventRegister,IEventTrigger
	{
		LinkedList<IEventListener> m_listeners = new LinkedList<IEventListener>();

		public EventSlot ()
		{

		}

		#region IEventRegister implementation

		public void Register (IEventListener listener)
		{
			m_listeners.AddLast(listener);
		}

		#endregion

		public void Trigger ()
		{
			var next = m_listeners.First;

			LinkedList<IEventListener> tempList = new LinkedList<IEventListener>();
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
						next.Value.Call();
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

