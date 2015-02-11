using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace u3dExtensions
{
	public class Future<T>: IDisposable
	{
	
		Action<T> m_mapFunc = (t) => {};

		Queue<RecoverPair> m_recoverPairs = new Queue<RecoverPair>();
	
		class RecoverPair
		{
			public Type ArgType;
			public Action<object> RecoveryFunc;

			public RecoverPair (Type argType, Action<object> recoveryFunc)
			{
				this.ArgType = argType;
				this.RecoveryFunc = recoveryFunc;
			}
			
		}

		internal Future (): this(null)
		{
		
		}

		internal Future (System.Exception error)
		{
			IsSet = false;
			Error = error;
		}

		#region IFuture implementationr

		public Future<Unit> Map(Action<T> mapFunc)
		{

			return Map((x) =>{
				mapFunc(x);
				return Unit.Unit;
			});
		}

		public Future<K> Map<K> (Func<T, K> mapFunc)
		{
			if(IsDisposed)
				throw new FutureContentDisposed();

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

			Recover((object e) => other.FlushErrorRecover(e));

			return other;
		}

		void FlushMapFunc()
		{
			var localMap = m_mapFunc;
			m_mapFunc = (x) =>{};

			if(IsDisposed)
			{
				var disposable = (Value as IDisposable);
				if(disposable != null)
				{
					disposable.Dispose();				
				}

				FlushErrorRecover(new FutureContentDisposed());
			}
			else
			{
				localMap(Value);
				m_recoverPairs.Clear ();
			}
		}

		public Future<T> Recover(Action<System.Exception> recoverFunc)
		{
			return Recover<System.Exception> (recoverFunc);
		}

		public Future<T> Recover<K> (Action<K> recoverFunc)
		{
			if(IsDisposed)
				throw new FutureContentDisposed();

			if(Error!=null)
			{
				if(Error is K)
					recoverFunc((K)(object)Error);
			}
			else
			{
				var recoverPair = new RecoverPair (typeof(K), ((obj) => recoverFunc ((K)obj)));
				m_recoverPairs.Enqueue (recoverPair);
			}

			return this;
		}
		#endregion

		public void FlushErrorRecover(object error)
		{
			try
			{
				Error = error;

				foreach (var pair in m_recoverPairs) 
				{
					if (pair.ArgType.Equals(error.GetType()) || error.GetType().IsSubclassOf(pair.ArgType))
					{
						pair.RecoveryFunc (Error);
						//break;
					}
				}
					
			}
			finally
			{
				m_recoverPairs.Clear ();
				m_mapFunc = (x)=>{};
			}
		}

		internal void Set(T value) 
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

		public object Error
		{
			get;
			private set;
		}

		public T Value {
			get;
			private set;
		}

	
		private bool IsDisposed
		{
			get;
			set;
		}

		public void Dispose ()
		{
			if(!IsDisposed)
			{

				if(IsSet)
				{
					var disposable = (Value as IDisposable);
					if(disposable != null)
					{
						disposable.Dispose();				
					}
				}
					
				IsDisposed = true;
			}

		}
	}

	public static class Future
	{
		public static Future<T> Success<T>(T value)
		{
			Future<T> future = new Future<T>();
			future.Set(value);
			return future;
		}

		public static Future<T> Failure<T>(Exception e)
		{
			Future<T> future = new Future<T>(e);
		
			return future;
		}

		public static Future<K> Recover<T,K>(this Future<T> me, Func<System.Exception, K> recoverFunc) where T:K
		{
			return  me.Recover<T,K,Exception>(recoverFunc);
		}

		public static Future<K> Recover<T,K,W>(this Future<T> me, Func<W, K> recoverFunc) where T:K
		{
			Future<K> other = new Future<K>();

			me.Recover((W e) =>{ other.Set(recoverFunc(e));});
			me.Map((val) =>{ other.Set(val);});

			return  other;
		}

		public static Future<K> FlatRecover<T,K>(this Future<T> me, Func<System.Exception, Future<K>> recoverFunc) where T:K
		{

			return  me.FlatRecover<T,K,Exception>(recoverFunc);
		}

		public static Future<K> FlatRecover<T,K,W>(this Future<T> me, Func<W, Future<K>> recoverFunc) where T:K
		{
			Future<K> other = new Future<K>();

			me.Recover((W e) =>
			{ 
				var future = recoverFunc(e);
				future.Map((x) => other.Set(x));
				future.Recover((e2) => other.FlushErrorRecover(e2));
			});

			me.Recover ((object e) => {
				if(!(e is W))
					other.FlushErrorRecover(e);
			});
		
			me.Map((val) =>{ other.Set(val);});

			return  other;
		}

		public static Future<T> Complete<T> (this Future<T> me,System.Action completeFunc)
		{
			me.Map((t) => completeFunc());
			me.Recover((object o) => completeFunc());

			return me;
		}

		public static Future<K> FlatMap<T,K> (this Future<T> me,System.Func<T,Future<K>> flatMapFunc)
		{
			Future<K> other = new Future<K>();

			me.Map((x) =>{

				var map1 = flatMapFunc(x).Map((k) => other.Set(k));
				return map1.Recover((e) => other.FlushErrorRecover(e));

			}).Recover((e)=> other.FlushErrorRecover(e));

			return other;
		}
	}
}

