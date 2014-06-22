using System;

namespace u3dExtensions
{
	public interface IFuture<T>
	{
		IFuture<Unit> Map(System.Action<T> mapFunc);

		IFuture<K> Map<K>(System.Func<T,K> mapFunc);

		IFuture<T> Recover(Action<System.Exception> recoverFunc);

		bool IsSet{get;}

		System.Exception Error{get;}

		T Value{get;}
	}
}

