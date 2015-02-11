using System;

namespace u3dExtensions
{
	public interface Promise<T>
	{
		Future<T> Future{get;}
		void Fulfill(T val);
		void FulfillError<K> (K e);
	}
}

