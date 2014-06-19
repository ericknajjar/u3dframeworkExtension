using UnityEngine;
using System.Collections;
using System;

namespace u3dExtensions.Engine.Runtime
{
	public class BindingPair
	{
		public object name;
		public System.Type BindingType;
		
		public BindingPair (object name, Type bindingType)
		{
			this.name = name;
			this.BindingType = bindingType;
		}
		
		public override bool Equals (object obj)
		{
			if (obj == null)
				return false;
			if (ReferenceEquals (this, obj))
				return true;
			if (obj.GetType () != typeof(BindingPair))
				return false;
			BindingPair other = (BindingPair)obj;
			return name == other.name && BindingType == other.BindingType;
		}
		
		
		public override int GetHashCode ()
		{
			unchecked {
				return (name != null ? name.GetHashCode () : 0) ^ (BindingType != null ? BindingType.GetHashCode () : 0);
			}
		}
		
		
	}

}

