using System;

namespace u3dExtensions.IOC
{
	public class BindingKey: IBindingKey
	{
		public BindingKey (object key)
		{
			Key = key;
		}

		public override bool Equals (object obj)
		{
			IBindingKey other = obj as IBindingKey;

			if(other == null) return false;

			return other.Key.Equals(Key);
		}

		public override int GetHashCode ()
		{
			return Key.GetHashCode();
		}

		public override string ToString ()
		{
			return string.Format ("[BindingKey: Key={0}]", Key);
		}
		
		#region IBindingKey implementation

		public object Key 
		{
			get;
			private set;
		}

		#endregion
	}
}

