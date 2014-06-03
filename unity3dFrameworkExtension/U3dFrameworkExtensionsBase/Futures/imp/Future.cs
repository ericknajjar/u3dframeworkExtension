using System;

namespace u3dExtensions
{
	public class Future<T>: IFuture<T>
	{
		T m_value;
		System.Exception m_error;

		Action<T> m_mapFunc = (t) => {};
		Action<System.Exception> m_recoveryFunc = e=>{};

		internal Future (): this(null)
		{
		
		}

		internal Future (System.Exception error)
		{
			IsSet = false;
			m_error = error;
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

			if(m_error!=null)
			{
				other.FlushErrorRecover(m_error);
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
			m_mapFunc(m_value);
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
			if(m_error!=null)
			{
				recoverFunc(m_error);
			}
			else
			{
				m_recoveryFunc+=recoverFunc;
			}

			return this;
		}
		#endregion

		void FlushErrorRecover(System.Exception error)
		{
			m_error = error;
			m_recoveryFunc(error);
			m_recoveryFunc = (e)=>{};
			m_mapFunc = (x)=>{};
		}

		public void Set(T value) 
		{
			if(IsSet) return;

			m_value = value;
			IsSet = true;
			FlushMapFunc();
		}

						
		public bool IsSet
		{
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

