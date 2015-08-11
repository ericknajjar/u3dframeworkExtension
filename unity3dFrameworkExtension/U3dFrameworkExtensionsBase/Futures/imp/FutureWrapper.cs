using System;

namespace u3dExtensions
{
	internal class FutureWrapper<T>: IFuture<T>
	{
		InnerFuture m_innerFuture;
		bool m_canceled = false;

		public FutureWrapper(InnerFuture innerFuture)
		{
			m_innerFuture = innerFuture;
		}

		#region IDisposable implementation

		public void Dispose ()
		{
			m_innerFuture.Dispose();
		}

		#endregion

		#region IFuture implementation

		public bool IsSet {
			get {
				return m_innerFuture.IsSet;
			}
		}

		public object Error {
			get {
				return m_innerFuture.Error;
			}
		}

		public InnerFuture InnerFuture {
			get {
				return m_innerFuture;
			}
		}

		public void Cancel ()
		{
			m_innerFuture.Cancel();
		}
		#endregion


	}
}

