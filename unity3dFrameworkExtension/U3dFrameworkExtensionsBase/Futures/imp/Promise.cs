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

		public void Fulfill (T val)
		{
			if(m_future.IsSet) throw new PromiseResetException();

			m_future.Set(val);
		}
	}
}

