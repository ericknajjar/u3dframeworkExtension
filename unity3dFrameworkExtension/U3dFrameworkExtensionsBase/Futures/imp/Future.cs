using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace u3dExtensions
{
	public class InnerFuture
	{
		Action<object> m_mapFunc = (t) => {};

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

		internal InnerFuture (): this(null)
		{

		}

		internal InnerFuture (System.Exception error)
		{
			IsSet = false;
			Error = error;
		}

		#region IFuture implementation

		public InnerFuture Map(Action<object> mapFunc)
		{

			return Map((x) =>{
				mapFunc(x);
				return Unit.Unit;
			});
		}

		public InnerFuture Map (Func<object, object> mapFunc)
		{
			if(IsDisposed)
				throw new FutureContentDisposed();

			InnerFuture other = new InnerFuture();

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

			Recover((object e) => other.FlushErrorRecover(e),typeof(object));

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
			
		public InnerFuture Recover (Action<object> recoverFunc, Type t)
		{
			if(IsDisposed)
				throw new FutureContentDisposed();

			if(Error!=null)
			{
				recoverFunc(Error);
			}
			else
			{
				var recoverPair = new RecoverPair (t, ((obj) => recoverFunc (obj)));
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

		public void Set(object value) 
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

		public object Value {
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

	internal class FutureWrapper<T>: IFuture<T>
	{
		InnerFuture m_innerFuture;
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

		#endregion
	}

	public static class Future
	{
		public static IFuture<T> Success<T>(T value)
		{
			InnerFuture inner  = new InnerFuture();
			inner.Set(value);

			return new FutureWrapper<T>(inner);
		}

		public static IFuture<T> Failure<T>(Exception e)
		{
			InnerFuture inner  = new InnerFuture(e);
			return new FutureWrapper<T>(inner);
		}

		public static IFuture<Unit> Map<T>(this IFuture<T> me,System.Action<T> mapFunc)
		{
			InnerFuture inner = me.InnerFuture;

			var ret = inner.Map((obj) => mapFunc((T)obj));

			return new FutureWrapper<Unit>(ret);
		}

		public static IFuture<K> Map<T,K>(this IFuture<T> me,System.Func<T,K> mapFunc)
		{
			InnerFuture inner = me.InnerFuture;

			var ret = inner.Map((obj) => mapFunc((T)obj));

			return new FutureWrapper<K>(ret);
		}	

		public static IFuture<T> Recover<T>(this IFuture<T> me,Action<System.Exception> recoverFunc)
		{
			InnerFuture inner = me.InnerFuture;

			var ret = inner.Recover((obj) => {

				if(obj is Exception)
					recoverFunc((Exception)obj);
			
			},typeof(Exception));

			return new FutureWrapper<T>(ret);
		}

		public static T GetValue<T>(this IFuture<T> me)
		{
			return (T)me.InnerFuture.Value;
		}

		public static IFuture<T> Recover<T,K> (this IFuture<T> me,Action<K> recoverFunc)
		{

			InnerFuture inner = me.InnerFuture;

			var ret = inner.Recover((obj) => 
				{

					if(obj is K)
						recoverFunc((K)obj);
				},typeof(K));

			return new FutureWrapper<T>(ret);
		}

		public static IFuture<T> Complete<T> (this IFuture<T> me,System.Action completeFunc)
		{

			me.InnerFuture.Map((t) => completeFunc());
			me.InnerFuture.Recover((object o) =>{ completeFunc();},typeof(object));

			return me;
		}

		public static IFuture<K> FlatMap<T,K> (this IFuture<T> me,System.Func<T,IFuture<K>> flatMapFunc)
		{
			InnerFuture other = new InnerFuture();

			me.Map((x) =>{

				var map1 = flatMapFunc(x).Map((k) => other.Set(k));
				return map1.Recover((e) => other.FlushErrorRecover(e));

			}).Recover((e)=> other.FlushErrorRecover(e));

			return new FutureWrapper<K>(other);
		}

		public static IFuture<K> Recover<T,K>(this IFuture<T> me, Func<System.Exception, K> recoverFunc) //where T:K
		{
			return  me.Recover<T,K,Exception>(recoverFunc);
		}

		public static IFuture<K> Recover<T,K,W>(this IFuture<T> me, Func<W, K> recoverFunc)// where T:K
		{
			InnerFuture other = new InnerFuture();

			me.Recover((W e) =>{ other.Set(recoverFunc(e));});
			me.Map((val) =>{ other.Set(val);});

			return new FutureWrapper<K>(other);
		}

	
		public static IFuture<K> FlatRecover<T,K>(this IFuture<T> me, Func<System.Exception, IFuture<K>> recoverFunc) where T:K
		{

			return  me.FlatRecover<T,K,Exception>(recoverFunc);
		}

		public static IFuture<K> FlatRecover<T,K,W>(this IFuture<T> me, Func<W, IFuture<K>> recoverFunc) where T:K
		{
			InnerFuture other = new InnerFuture();

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

			return  new FutureWrapper<K>(other);
		}
	}
}

