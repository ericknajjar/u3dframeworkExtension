using System;

namespace u3dExtensions
{
	public interface IPromise<T>
	{
		IFuture<T> Future{get;}
		void Fulfill(T val);
	}
}

