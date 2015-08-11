﻿using System;

namespace u3dExtensions
{
	/*public interface IFuture<T> : IDisposable
	{
		IFuture<Unit> Map(System.Action<T> mapFunc);

		IFuture<K> Map<K>(System.Func<T,K> mapFunc);

		IFuture<T> Recover(Action<System.Exception> recoverFunc);
		IFuture<T> Recover<K> (Action<K> recoverFunc);

		bool IsSet{get;}

		object Error{get;}

		T Value{get;}
	}*/

	public interface IFuture<T> : IDisposable
	{
		//IFuture<Unit> Map(System.Action<T> mapFunc);
		//IFuture<K> Map<K>(System.Func<T,K> mapFunc);

		//IFuture<T> Recover(Action<System.Exception> recoverFunc);
		//IFuture<T> Recover<K> (Action<K> recoverFunc);
		//T Value{get;}

		void Cancel();

		bool IsSet{get;}

		object Error{get;}

		InnerFuture InnerFuture{get;}

	}
		


		
}

