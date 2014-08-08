using NUnit.Framework;
using System;
using System.Collections.Generic;
using u3dExtensions.Events;

namespace u3dExtensions.Tests.EventTests
{
	[TestFixture ()]
	public class Tests
	{
		[Test ()]
		public void EventCallOrder()
		{
			var slot = new EventSlot();
			List<int> ints = new List<int>();

			slot.Register(() => ints.Add(0));
			slot.Register(() => ints.Add(1));
			slot.Trigger();

			Assert.That(ints[0].Equals(0) && ints[1].Equals(1));
		}

		[Test ()]
		public void NotCalledIfNotTrigger()
		{
			var slot = new EventSlot();
			bool called = false;

			slot.Register(() => called = true);
		
			Assert.That(!called);
		}

		[Test ()]
		public void CalledWithDelegateEventListener()
		{
			var slot = new EventSlot();

			bool called = false;

			IEventListener listener = new DelegateEventListener(() => called = true);

			slot.Register(listener);
			slot.Trigger();

			Assert.That(called);
		}

		[Test ()]
		public void DeadListenerNotCalled()
		{
			var slot = new EventSlot();

			bool called = false;

			IEventListener listener = new DelegateEventListener(() => called = true,()=> true);

			slot.Register(listener);
			slot.Trigger();

			Assert.That(!called);
		}

		[Test ()]
		public void DeadListenerRemoved()
		{
			var slot = new EventSlot();

			bool called = false;
			bool dead = true;

			IEventListener listener = new DelegateEventListener(() => called = true,()=> dead);

			slot.Register(listener);
			slot.Trigger();
			dead = false;
			slot.Trigger();

			Assert.That(!called);
		}

		[Test ()]
		public void ListenerAddedDuringTriggerNotCalled()
		{
			var slot = new EventSlot();

			bool called = false;

			IEventListener listener2 = new DelegateEventListener(()=> called=true);

			IEventListener listener = new DelegateEventListener(() => slot.Register(listener2));

			slot.Register(listener);
			slot.Trigger();

			Assert.That(!called);
		}

		[Test ()]
		public void ExceptionsDuringTriggerDontEraseSlot()
		{
			var slot = new EventSlot();

			bool called = false;

			IEventListener listener2 = new DelegateEventListener(()=> called=true);

			bool dead = false;
			IEventListener listener = new DelegateEventListener(() => {dead = true; throw new Exception();},()=> dead);


			slot.Register(listener);

			slot.Register(listener2);

			try
			{
				slot.Trigger();
			}
			catch(Exception){}

			slot.Trigger();

			Assert.That(called);
		}

		[Test ()]
		public void LifeTimeEventListenerDontHoldGarbage()
		{
			var slot = new EventSlot();
			List<int> list = new List<int>();

			{
				TestGarbage garbage = new TestGarbage(list);
				var listener = DelegateEventListeners.LifeTime((t)=> t.Callback(),garbage);
				slot.Register(listener);
			}
		
			GC.Collect();

			slot.Trigger();

			Assert.AreEqual(0,list.Count);
		}

		[Test ()]
		public void LifeTimeEventListenerStillAlive()
		{
			var slot = new EventSlot();
			List<int> list = new List<int>();


			TestGarbage garbage = new TestGarbage(list);

			var listener = DelegateEventListeners.LifeTime((t)=> t.Callback(),garbage);
			slot.Register(listener);


			GC.Collect();

			slot.Trigger();
			garbage.Nothing = 2;
			Assert.AreEqual(1,list.Count);
		}

		[Test ()]
		public void WeakEventListenerDead()
		{
			List<int> list = new List<int>();

			WeakEventListener<TestGarbage> listener;
			{
				 var garbage = new TestGarbage(list);

				listener = new WeakEventListener<TestGarbage>(garbage,(target) =>{} );
			}

			GC.Collect();
			Assert.That(listener.IsDead());
		}

		[Test ()]
		public void WeakEventListenerDead1()
		{
			List<int> list = new List<int>();

			WeakEventListener<TestGarbage,int> listener;
			{
				var garbage = new TestGarbage(list);

				listener = new WeakEventListener<TestGarbage,int>(garbage,(target,i) =>{} );
			}

			GC.Collect();
			Assert.That(listener.IsDead());
		}

		[Test ()]
		public void WeakEventListenerDead2()
		{
			List<int> list = new List<int>();

			WeakEventListener<TestGarbage,int,int> listener;
			{
				var garbage = new TestGarbage(list);

				listener = new WeakEventListener<TestGarbage,int,int>(garbage,(target,i,i2) =>{} );
			}

			GC.Collect();
			Assert.That(listener.IsDead());
		}

		[Test ()]
		public void WeakEventListenerDead3()
		{
			List<int> list = new List<int>();

			WeakEventListener<TestGarbage,int,int,int> listener;
			{
				var garbage = new TestGarbage(list);

				listener = new WeakEventListener<TestGarbage,int,int,int>(garbage,(target,i,i2,i3) =>{} );
			}

			GC.Collect();
			Assert.That(listener.IsDead());
		}

		[Test ()]
		public void DeadListenersRemovedRightAway()
		{
			var slot = new EventSlot();

			bool called = false;
			bool dead = false;
			int count = 0;

			IEventListener listener = new DelegateEventListener(() =>{ if(count==0)dead = true;else called = true; ++count;},()=> dead);

			slot.Register(listener);

			slot.Trigger();
			dead = false;

			slot.Trigger();

			Assert.That(!called);
		}

		[Test ()]
		public void Once()
		{
			var slot = new EventSlot();

			int count = 0;

			IEventListener listener = DelegateEventListeners.Once(() =>{  ++count;});

			slot.Register(listener);

			slot.Trigger();
			slot.Trigger();

			Assert.AreEqual(1,count);
		}

		class TestGarbage
		{
			List<int> m_list;
			public TestGarbage(List<int> list)
			{
				m_list = list;
			}

			public int Nothing{get; set;}
			public void Callback()
			{
				m_list.Add(1);
			}
		}
	}
}

