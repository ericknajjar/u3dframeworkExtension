using System;

namespace u3dExtensions.Events
{
	public interface IEventTrigger
	{
		void Trigger();
	}

	public interface IEventTrigger<T>
	{
		void Trigger(T arg1);
	}

	public interface IEventTrigger<T,K>
	{
		void Trigger(T arg1,K arg2);
	}

	public interface IEventTrigger<T,K,J>
	{
		void Trigger(T arg1,K arg2, J arg3);
	}

	public interface IEventTrigger<T,K,J,W>
	{
		void Trigger(T arg1,K arg2, J arg3,W arg4);
	}
}

