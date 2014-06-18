using System;

namespace u3dExtensions.IOC
{
	public class BindingName: IBindingName
	{

		public BindingName (object name)
		{
			Name = name;
		}

		public override bool Equals (object obj)
		{
			IBindingName other = obj as IBindingName;
			if(other == null)
			{
				return false;
			}



			return other.Name.Equals(Name);
		}

		public override string ToString ()
		{
			return string.Format ("[BindingName: Name={0}]", Name);
		}
	
		public override int GetHashCode ()
		{
			return Name.GetHashCode ();
		}
		#region IBindingName implementation

		public object Name {
			get;
			private set;
		}

		#endregion
	}
}

