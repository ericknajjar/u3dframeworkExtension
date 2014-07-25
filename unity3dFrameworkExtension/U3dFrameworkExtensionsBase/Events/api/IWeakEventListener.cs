using System;

namespace u3dExtensions.Events
{
	public interface IWeakEventListener<T>: IEventListener
	{

	}

	public interface IWeakEventListener<T,K>: IEventListener<K>
	{

	}

	public interface IWeakEventListener<T,K,J>: IEventListener<K,J>
	{

	}

	public interface IWeakEventListener<T,K,J,W>: IEventListener<K,J,W>
	{

	}
}

