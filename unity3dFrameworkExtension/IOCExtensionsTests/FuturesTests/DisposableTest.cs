using System;

namespace u3dExtensions.Tests.FuturesTests
{
	public class DisposableTest: IDisposable
	{
		public int Disposed
		{
			get;
			private set;
		}

		#region IDisposable implementation

		void IDisposable.Dispose ()
		{
			++Disposed;
		}

		#endregion
	}
}

