using System;

namespace u3dExtensions
{
	public class Future<T>: IFuture<T>
	{
	
		Action<T> m_mapFunc = (t) => {};
		Action<System.Exception> m_recoveryFunc = e=>{};

		internal Future (): this(null)
		{
		
		}

		internal Future (System.Exception error)
		{
			IsSet = false;
			Error = error;
		}

		#region IFuture implementation

		public IFuture<Unit> Map(Action<T> mapFunc)
		{
			return Map((x) =>{

				mapFunc(x);
				return Unit.Unit;
			});
		}

		public IFuture<K> Map<K> (Func<T, K> mapFunc)
		{
			Future<K> other = new Future<K>();

			if(Error!=null)
			{
				other.FlushErrorRecover(Error);
				return other;
			}
	
			m_mapFunc += (x) =>
			{
				try
				{
					other.Set(mapFunc(x));
				}
				catch(System.Exception e)
				{
					other.FlushErrorRecover(e);
				}
			};
				
			if(IsSet == true)
			{
				FlushMapFunc();
			}

			return other;
		}

		void FlushMapFunc()
		{
			m_mapFunc(Value);
			m_mapFunc = (x) =>{};
			m_recoveryFunc = (e)=>{};
		}

		public IFuture<K> FlatMap<K> (Func<T, IFuture<K>> flatMapFunc)
		{
			Future<K> other = new Future<K>();

			Map((x) => flatMapFunc(x).Map((k) => other.Set(k))).Recover((e)=> other.FlushErrorRecover(e));

			return other;
		}

		public IFuture<T> Recover(Action<System.Exception> recoverFunc)
		{
			if(Error!=null)
			{
				recoverFunc(Error);
			}
			else
			{
				m_recoveryFunc+=recoverFunc;
			}

			return this;
		}
		#endregion

		public void FlushErrorRecover(System.Exception error)
		{
			Error = error;
			m_recoveryFunc(error);
			m_recoveryFunc = (e)=>{};
			m_mapFunc = (x)=>{};
		}

		public void Set(T value) 
		{
			if(IsSet) return;

			Value = value;
			IsSet = true;
			FlushMapFunc();
		}

						
		public bool IsSet
		{
			get;
			private set;
		}

		public Exception Error
		{
			get;
			private set;
		}

		public T Value {
			get;
			private set;
		}
	}

	public static class Future
	{
		public static IFuture<T> Success<T>(T value)
		{
			Future<T> future = new Future<T>();
			future.Set(value);
			return future;
		}

		public static IFuture<T> Failure<T>(Exception e)
		{
			Future<T> future = new Future<T>(e);
		
			return future;
		}
	}
}

