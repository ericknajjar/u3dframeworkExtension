using System;

namespace u3dExtensions.IOC
{
	internal class ObjectBindingRequirement: IBindingRequirement
	{
		IBindingName m_name;
		IBindingKey m_key;

		public ObjectBindingRequirement(IBindingName name,IBindingKey key)
		{
			m_name = name;
			m_key = key;
		}

		#region IBindingRequirement implementation

		object IBindingRequirement.Get (IBindingContext bindingContext)
		{
			return bindingContext.Unsafe.Get(m_name,m_key);
		}

		#endregion

		public override bool Equals (object obj)
		{
			ObjectBindingRequirement other = obj as ObjectBindingRequirement;
			if(other == null) return false;
		

			return other.m_name.Equals(m_name) && other.m_key.Equals(m_key);
		}

		public override int GetHashCode ()
		{
			unchecked
			{
				int hash = 17;

				hash = hash * 31 + m_name.GetHashCode();
				hash = hash * 31 + m_key.GetHashCode();

				return hash.GetHashCode ();
			}
		}

		public override string ToString ()
		{
			return string.Format ("[ObjectBindingRequirement: m_name={0}, m_key={1}]", m_name, m_key);
		}
		
	}

	public class BindingRequirements
	{
		static BindingRequirements s_instance;
		BindingRequirements()
		{

		}

		public static BindingRequirements Instance
		{
			get
			{
				if(s_instance == null)
				{
					s_instance = new BindingRequirements();
				}

				return s_instance;
			}

		}

		public IBindingRequirement With<T>()
		{
			return new ObjectBindingRequirement(new BindingName(InnerBindingNames.Empty), new BindingKey(typeof(T)));
		}

		public IBindingRequirement With<T>(IBindingName name)
		{
			return new ObjectBindingRequirement(name,new BindingKey(typeof(T)));
		}

		public IBindingRequirement With(IBindingName name,IBindingKey key)
		{
			return new ObjectBindingRequirement(name,key);
		}
	}
}

