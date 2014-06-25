using NUnit.Framework;
using System;
using u3dExtensions;
using System.Collections.Generic;

namespace u3dExtensions.Tests.FuturesTests
{
	[TestFixture ()]
	public class Tests
	{
		IPromise<int> m_promise;
		IFuture<int> m_future;

		class A
		{

		}

		class B: A
		{

		}

		[SetUp]
		public void Setup()
		{
			m_promise = new Promise<int>();
			m_future = m_promise.Future;
		}

		[Test ()]
		public void IntFutureMapPre ()
		{
			int ret = -1;

			m_future.Map((x) => ret = x);
			m_promise.Fulfill(32);

			Assert.AreEqual(32,ret);
		}

		[Test ()]
		public void IntFutureMapPos ()
		{
	
			int ret = -1;
			m_promise.Fulfill(32);
			m_future.Map((x) => ret = x);


			Assert.AreEqual(32,ret);
		}

		[Test ()]
		public void IntFutureNotSetNotCalled ()
		{
			int ret = -1;
			m_future.Map((x) => ret = x);

			Assert.AreEqual(-1,ret);
		}

		[Test ()]
		public void IntFutureNoReset ()
		{
			m_promise.Fulfill(32);
			Assert.Throws<PromiseResetException>(() => m_promise.Fulfill(32));
		}

		[Test ()]
		public void IntFutureMapsToAfloatFuture ()
		{
			float ret = -1;
			IFuture<float> floatFuture = m_future.Map((x)=> x+1.5f);

			floatFuture.Map((x) => ret = x);

			m_promise.Fulfill(32);

			Assert.Greater(ret,32.0f);
		}

		[Test ()]
		public void IntFutureMapsToAfloatFutureRecover ()
		{
			bool called = false;

			IFuture<float> floatFuture = m_future.Map((x)=> x+1.5f);

			floatFuture.Map((x) => {throw new Exception();}).Recover((e) =>{ called = true;});

			m_promise.Fulfill(32);

			Assert.That(called);
		}
			
		[Test ()]
		public void IntFutureSucessfull ()
		{
			int ret = -1;

			IFuture<int> success = Future.Success(32);

			success.Map((x) => ret = x);

			Assert.AreEqual(32,ret);
		}

		[Test ()]
		public void IntFutureFailuer ()
		{
			bool called = false;

			IFuture<int> success = Future.Failure<int>(new Exception());

			success.Recover((e) =>{ called = true;});

			Assert.That(called);
		}

		[Test ()]
		public void IntFutureMapsToAfloatFutureRecoverPre ()
		{
			bool called = false;

			IFuture<float> floatFuture = Future.Success(1.0f);

			floatFuture.Map((x) => {throw new Exception();}).Recover((e) =>{ called = true;});

			Assert.That(called);
		}

		[Test ()]
		public void IntFutureMapsToAfloatFutureRecoverPreChained ()
		{
			bool called = false;

			IFuture<float> floatFuture = m_future.Map(x => 44.0f);

			m_promise.Fulfill(32);

			floatFuture.Map((x) => {throw new Exception();}).Recover((e) =>{ called = true;});

			Assert.That(called);
		}

		[Test ()]
		public void IntFutureToFloatFlatMap ()
		{
			float ret = -1;

			IFuture<int> success = Future.Success(32);

			success.FlatMap((x) => Future.Success(99.0f)).Map((x) => ret = x);

			Assert.AreEqual(99.0f,ret,0.1f);
		}

		[Test ()]
		public void IntFutureToUnitFlatMapRecover ()
		{
			bool called = false;

			IFuture<int> success = Future.Success(32);

			success.FlatMap((x) => {throw new Exception(); return Future.Success(99.0f);}).Map((x) => {}).Recover((e) =>{ called = true;});

			Assert.That(called);
		}

		[Test ()]
		public void IntFailureThroughPromise ()
		{
			bool called = false;

			m_promise.FulfillError(new Exception());

			m_future.Recover((e) =>{ called = true;});

			Assert.That(called);
		}

		[Test ()]
		public void IntFailureThroughPromiseFlatMap ()
		{
			bool called = false;

			m_promise.FulfillError(new Exception());

			m_future =	m_future.FlatMap((i) => Future.Success(i));
			m_future.Recover((e) => {called = true;});

			Assert.That(called);
		}

		[Test ()]
		public void IntFailureThroughPromiseCompound ()
		{
			bool called = false;

			IPromise<int> promisse2 = new Promise<int>();

			m_future.Map((i) => promisse2.FulfillError(new Exception()));

			m_promise.Fulfill(32);

			promisse2.Future.Recover((e) => {called = true;});

			Assert.That(called);
		}

		[Test ()]
		public void IntFailureThroughPromiseFlatMapCompound ()
		{
			bool called = false;

			IPromise<int> promisse2 = new Promise<int>();

			m_future.Map((i) => promisse2.FulfillError(new Exception()));

			m_promise.Fulfill(32);

			m_future = m_future.FlatMap((i) => promisse2.Future);
			m_future.Recover((e) => {called = true;});

			Assert.That(called);
		}

		[Test ()]
		public void IntFailureThroughPromiseFlatMapCompoundJustOnce ()
		{
			int callCount = 0;

			IPromise<int> promisse2 = new Promise<int>();

			m_future.Map((i) => promisse2.FulfillError(new Exception()));

			m_promise.Fulfill(32);

			m_future = m_future.FlatMap((i) => promisse2.Future);
			m_future.Recover((e) => ++callCount);

			Assert.AreEqual(1,callCount);
		}


		[Test ()]
		public void IntFailureThroughPromiseMapChain ()
		{
			bool called = false;

			m_future =	m_future.Map((i) => i).Map((i) => i);

			m_promise.FulfillError(new Exception());

			m_future.Recover((e) => {called = true;});

			Assert.That(called);
		}


		[Test ()]
		public void IntFailureThroughPromiseTwice ()
		{
			m_promise.FulfillError(new Exception());

			Assert.Throws<PromiseResetException>(() => m_promise.FulfillError(new Exception()));
		}

		[Test ()]
		public void IntFailureThroughPromiseAlreadyFullfiled ()
		{
			m_promise.Fulfill(32);

			Assert.Throws<PromiseResetException>(() => m_promise.FulfillError(new Exception()));
		}

		[Test ()]
		public void IntFailureThroughPromiseWithError ()
		{
			m_promise.FulfillError(new Exception());

			Assert.Throws<PromiseResetException>(() => m_promise.Fulfill(32));
		}

		[Test ()]
		public void FutureChainMiddleFailuer ()
		{
			var other = m_future.Map((x) =>{ throw new Exception(); return x;});

			bool called = false;
			other.Recover((e) => {called = true;});

			other.Map ((w) => w).Recover((e) => {});

			m_promise.Fulfill(32);
			Assert.That(called);
		}

		[Test ()]
		public void FutureSucessRecoverRightValue ()
		{
			var other = m_future.Map((x) =>{ return x;}).Recover((e) => 33);
			m_promise.Fulfill(32);

			Assert.AreEqual(32,other.Value);
		}

		[Test ()]
		public void FutureFailureRecoverRightValue ()
		{
			var other = m_future.Map((x) =>{ return x;}).Recover((e) => 33);
			m_promise.FulfillError(new Exception());

			Assert.AreEqual(33,other.Value);
		}

		[Test ()]
		public void FutureSucessFlatRecoverRightValue ()
		{
			var other = m_future.Map((x) =>{ return x;}).FlatRecover((e) => Future.Success(33));
			m_promise.Fulfill(32);

			Assert.AreEqual(32,other.Value);
		}

		[Test ()]
		public void FutureFailureFlatRecoverRightValue ()
		{
			var other = m_future.Map((x) =>{ return x;}).FlatRecover((e) => Future.Success(33));
			m_promise.FulfillError(new Exception());

			Assert.AreEqual(33,other.Value);
		}


		[Test ()]
		public void FutureDoubleFailureFlatRecoveCalledWriteError ()
		{
			var error = new Exception();

			var other = m_future.Map((x) =>{ return x;}).FlatRecover((e) => Future.Failure<int>( error));
			m_promise.FulfillError(new Exception());

			bool called = false;
			other.Recover((e) =>{called = error.Equals(e);});

			Assert.That(called);
		}


		[Test ()]
		public void FutureFailureRecoverRightObject ()
		{
			var other = m_future.Map((x) =>{ return x;}).Recover((e) => (object)"33");
			m_promise.FulfillError(new Exception());

			Assert.AreEqual("33",other.Value);
		}

		[Test ()]
		public void FutureSuccesRecoverRightObjectInheritance ()
		{
			var a = new A();
			var b = new B();

			var future = Future.Success(b);

			var other = future.Map((x) => x).Recover((e) => a);


			Assert.AreEqual(b,other.Value);
		}

		[Test ()]
		public void FutureFailureRecoverRightObjectInheritance ()
		{
			var a = new A();
			var b = new B();

			var future = Future.Success(b);

			var other = future.Map((x) =>{throw new Exception(); return x;}).Recover((e) => a);

			Assert.AreEqual(a,other.Value);
		}

		[Test ()]
		public void FuturePolymorphicError ()
		{
			var other = m_future.Map ((x) => {
				return x;
			}).Recover ((System.NullReferenceException e) => (object)"34").Recover((e) => (object)"33");

			m_promise.FulfillError(new NullReferenceException());

			Assert.AreEqual("34",other.Value);
		}

		[Test ()]
		public void FuturePolymorphicErrorCalled ()
		{
			bool called = false;

			var other = m_future.Map ((x) => {
				return x;
			}).Recover ((System.NullReferenceException e) => {called = true;return (object)"34";});

			m_promise.FulfillError(new NullReferenceException());

			Assert.That(called);
		}

		[Test ()]
		public void FuturePolymorphicErrorRightObj ()
		{
			bool called = false;
			var error = new NullReferenceException();

		
			m_promise.FulfillError(error);

			var other = m_future.Map ((x) => {
				return x;
			}).Recover ((System.NullReferenceException e) => {
				called = error.Equals (e);
			});


			Assert.That(called);
		}

		[Test ()]
		public void FuturePolymorphicErrorRightObj2 ()
		{
			bool called = false;
			var error = new NullReferenceException();


			m_promise.FulfillError(error);

			Assert.DoesNotThrow (() => {

				var other = m_future.Map ((x) => {
					return x;
				}).Recover ((System.NotSupportedException e) => {
					called = error.Equals (e);
				});

			});
				
		}

		[Test ()]
		public void FutureChainErrorNotException ()
		{
			bool called = false;

			m_promise.FulfillError(32);

			var other = m_future.Map ((x) => {
				return (object) "23423";

			}).Recover ((int e) => {
				called = true;
			});

			Assert.That(called);
		}

		[Test]
		public void FuturePolymorphicErrorRightObj2Twice ()
		{

			var error = new NotSupportedException();

			var other = m_future.Map ((x) => {
				return x;
			}).Recover ((System.NotSupportedException e) => {

			}).Recover((System.NotImplementedException e)=> {
				return 3;
			});

			Assert.DoesNotThrow (() => {


				m_promise.FulfillError(error);

			});

		}

		/*[Test ()]
		public void FuturePolymorphicErrorOnlyOne ()
		{
			int called = 0;
			var error = new NotSupportedException();

			//m_future
			var other = m_future.Map ((x) => {
				return x;
			}).Recover ((System.NotSupportedException e) => {

				called++;

			}).Recover((System.Exception e)=> {
				called++;
			});

			m_promise.FulfillError(error);

			Assert.AreEqual (1, called);

		}*/

		[Test ()]
		public void FuturePolymorphicErrorNoExceptionError ()
		{
			int called = 0;
			int error = 1;

			m_promise.FulfillError(error);

			var other = m_future.Map ((x) => {
				return x;
			}).Recover ((int e) => {

				called++;

			}).Recover((System.Exception e)=> {
				called++;
			});


			Assert.AreEqual (1, called);

		}

		[Test ()]
		public void FuturePolymorphicErrorNoExceptionError2 ()
		{
			int called = 0;
			List<int> error = new List<int>();


			var other = m_future.Map ((x) => {
				return x;
			}).Recover ((List<int> e) => {

				called++;

			});

			m_promise.FulfillError(error);

			Assert.AreEqual (1, called);

		}

		[Test]
		public void ErrorWhenBothCallsFails()
		{
	
			IPromise<int> promise1 = new Promise<int>();
			IPromise<int> promise2 = new Promise<int>();

			var error = new NotSupportedException();

			var future = promise1.Future.Map((x) => x).FlatRecover((NotSupportedException e) => promise2.Future);

			promise1.FulfillError(error);
			promise2.FulfillError(error);

			Assert.IsNotNull(future.Error);
		}

		[Test]
		public void ErrorWhenBothCallsFailsCalledOrder1()
		{

			IPromise<int> promise1 = new Promise<int>();
			IPromise<int> promise2 = new Promise<int>();

			var error = new NotSupportedException();

			var future = promise1.Future.Map((x) => x).FlatRecover((NotSupportedException e) => {return promise2.Future;});

			bool called = false;

			future.Recover((e) => called = true);


			promise1.FulfillError(error);
			promise2.FulfillError(error);

			Assert.That(called);
		}

		[Test]
		public void ErrorWhenBothCallsFailsCalledOrder2()
		{

			IPromise<int> promise1 = new Promise<int>();
			IPromise<int> promise2 = new Promise<int>();

			var error = new NotSupportedException();

			var future = promise1.Future.Map((x) => x).FlatRecover((NotSupportedException e) => {return promise2.Future;});

			bool called = false;

			future.Recover((e) => called = true);

			promise2.FulfillError(error);
			promise1.FulfillError(error);

			Assert.That(called);
		}
			
	}
}

