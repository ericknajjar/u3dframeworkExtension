using System;

namespace u3dFramework.Events
{
	public interface IEventListener
	{
		void Call();
		bool IsDead();
	}

	public interface IEventListener<T>
	{
		void Call(T arg1);
		bool IsDead();
	}

	public interface IEventListener<T,K>
	{
		void Call(T arg1, K arg2);
		bool IsDead();
	}

	public interface IEventListener<T,K,J>
	{
		void Call(T arg1, K arg2,J arg3);
		bool IsDead();
	}

	public interface IEventListener<T,K,J,W>
	{
		void Call(T arg1, K arg2,J arg3,W arg4);
		bool IsDead();
	}
}

