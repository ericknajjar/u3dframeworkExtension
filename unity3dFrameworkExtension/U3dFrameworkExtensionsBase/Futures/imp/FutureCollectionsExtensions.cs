using System;
using System.Collections.Generic;

namespace u3dExtensions
{
	public static class FutureCollectionsExtensions
	{
		public static Future<IList<T>> FlatMap<T>(this IEnumerable<Future<T>> enumerable)
		{
			Promise<IList<T>> promise = new Promise<IList<T>> ();

			IList<T> ret = new List<T> ();

			var secured = new List<Future<T>> (enumerable);
		
			var enumerator = secured.GetEnumerator ();

			Map(enumerator,ret,promise);

			return promise.Future;
		}

		static void Map<T>(IEnumerator<Future<T>> enumerator, IList<T> toAdd, Promise<IList<T>> promise)
		{
		
			if (enumerator.MoveNext ()) 
			{
				enumerator.Current.Map((T t) =>{
					toAdd.Add(t);
					Map(enumerator,toAdd,promise);

				}).Recover((object e) => Map(enumerator,toAdd,promise));

			}
			else
			{
				promise.Fulfill(toAdd);
				enumerator.Dispose();
			}

		}
	}
}

