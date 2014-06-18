using NUnit.Framework;
using System;
using u3dExtensions;

namespace u3dExtensions.Tests.FuturesTests
{
	[TestFixture ()]
	public class Tests
	{
		IPromise<int> m_promise;
		IFuture<int> m_future;

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

			floatFuture.Map((x) => {throw new Exception();}).Recover((e) => called = true);

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

			success.Recover((e) => called = true);

			Assert.That(called);
		}

		[Test ()]
		public void IntFutureMapsToAfloatFutureRecoverPre ()
		{
			bool called = false;

			IFuture<float> floatFuture = Future.Success(1.0f);

			floatFuture.Map((x) => {throw new Exception();}).Recover((e) => called = true);

			Assert.That(called);
		}

		[Test ()]
		public void IntFutureMapsToAfloatFutureRecoverPreChained ()
		{
			bool called = false;

			IFuture<float> floatFuture = m_future.Map(x => 44.0f);

			m_promise.Fulfill(32);

			floatFuture.Map((x) => {throw new Exception();}).Recover((e) => called = true);

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

			success.FlatMap((x) => {throw new Exception(); return Future.Success(99.0f);}).Map((x) => {}).Recover((e) => called = true);

			Assert.That(called);
		}

		[Test ()]
		public void IntFailureThroughPromise ()
		{
			bool called = false;

			m_promise.FulfillError(new Exception());

			m_future.Recover((e) => called = true);

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
	}
}

