using System;

namespace u3dExtensions
{
	public interface Future<T> : IDisposable
	{
		Future<Unit> Map(System.Action<T> mapFunc);

		Future<K> Map<K>(System.Func<T,K> mapFunc);

		Future<T> Recover(Action<System.Exception> recoverFunc);
		Future<T> Recover<K> (Action<K> recoverFunc);

		bool IsSet{get;}

		object Error{get;}

		T Value{get;}
	}
}

