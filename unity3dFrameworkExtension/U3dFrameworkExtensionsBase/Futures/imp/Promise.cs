using System;

namespace u3dExtensions
{
	public class Promise<T>: IPromise<T>
	{
		Future<T> m_future = new Future<T>();

		public Promise ()
		{

		}

		#region IPromise implementation

		public IFuture<T> Future 
		{
			get 
			{
				return m_future;
			}
		}

		#endregion

		void ThrowIfCantFulfill ()
		{
			if (m_future.Error != null || m_future.IsSet)
				throw new PromiseResetException ();
		}

		public void Fulfill (T val)
		{
			ThrowIfCantFulfill ();

			m_future.Set(val);
		}

		public void FulfillError<K> (K e)
		{
			ThrowIfCantFulfill ();

			m_future.FlushErrorRecover(e);
		}
	}
}

