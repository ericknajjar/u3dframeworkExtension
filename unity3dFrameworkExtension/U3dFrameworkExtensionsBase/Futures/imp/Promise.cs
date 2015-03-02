using System;

namespace u3dExtensions
{
	public class Promise<T>: IPromise<T>
	{
		InnerFuture m_future = new InnerFuture();
	
		public Promise ()
		{
			Future = new FutureWrapper<T>(m_future);
		}

		#region IPromise implementation

		public IFuture<T> Future 
		{
			get;
			private set;
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

		public void FulfillError (object e)
		{
			ThrowIfCantFulfill ();

			m_future.FlushErrorRecover(e);
		}
	}
}

