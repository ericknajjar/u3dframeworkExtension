using System;
using System.Collections.Generic;

namespace u3dExtensions.IOC
{
	public partial class BindingContext
	{
		class ValueBingindContextAdapter<T>: IValueBindingContext<T>
		{
			ValueBindingContext m_adaptee;

			public ValueBingindContextAdapter(ValueBindingContext adaptee)
			{
				m_adaptee = adaptee;
			}

			#region IValueBindingContext implementation

			void IValueBindingContext<T>.To<K> (Func<K> func)
			{
				m_adaptee.To<K>(func);
			}
		
			IValueBindingContext<T,K> IValueBindingContext<T>.With<K> ()
			{
				return new ValueBingindContextAdapterWith<T,K>(InnerBindingNames.Empty,m_adaptee);
			}

			IValueBindingContext<T, K> IValueBindingContext<T>.With<K> (object name)
			{
				return new ValueBingindContextAdapterWith<T,K>(name,m_adaptee);
			}
			#endregion
		}
	

		class ValueBingindContextAdapterWith<T,J>: IValueBindingContext<T,J>
		{
			ValueBindingContext m_adaptee;
			object m_name;

			public ValueBingindContextAdapterWith(object name,ValueBindingContext adaptee)
			{
				m_adaptee = adaptee;
				m_name = name;

				if(typeof(T) == typeof(J) && m_name.Equals(m_adaptee.Name))
				{
					throw new BindingSelfRequirement();
				}
			}

			#region IValueBindingContext implementation
			void IValueBindingContext<T, J>.To<K> (Func<J, K> func)
			{
				var binding = new Binding(func, Requirements().ToArray());

				m_adaptee.To<K>(binding);
			}

			public List<IBindingRequirement> Requirements()
			{
				var requirement = BindingRequirements.With<J>(m_name);

				return new List<IBindingRequirement>(){requirement};
			}

			IValueBindingContext<T, J, K> IValueBindingContext<T, J>.With<K> ()
			{
				return new ValueBingindContextAdapterWith<T,J,K>(InnerBindingNames.Empty,m_adaptee,this);
			}

			IValueBindingContext<T, J, K> IValueBindingContext<T, J>.With<K> (object name)
			{
				return new ValueBingindContextAdapterWith<T,J,K>(name,m_adaptee,this);
			}

			#endregion
		}

		class ValueBingindContextAdapterWith<T,J,K>: IValueBindingContext<T,J,K>
		{
			ValueBindingContext m_adaptee;
			ValueBingindContextAdapterWith<T,J> m_parent;
			ValueBingindContextAdapterWith<T,K> m_me;

			object m_name;

			public ValueBingindContextAdapterWith(object name,ValueBindingContext adaptee, ValueBingindContextAdapterWith<T,J> parent)
			{
				m_adaptee = adaptee;
				m_name = name;
				m_parent = parent;
				m_me = new ValueBingindContextAdapterWith<T, K>(name,m_adaptee);
			}

			#region IValueBindingContext implementation

			void IValueBindingContext<T, J, K>.To<W> (Func<J,K, W> func) 
			{
				var binding = new Binding(func, Requirements().ToArray());

				m_adaptee.To<W>(binding);
			}

			#endregion

			public List<IBindingRequirement> Requirements()
			{
				List<IBindingRequirement> ret = new List<IBindingRequirement>();

				ret.AddRange(m_parent.Requirements());
				ret.AddRange(m_me.Requirements());


				return ret;
			}
		}

	
	}
}

