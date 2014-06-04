using System;

namespace u3dExtensions.IOC.extesions
{
	public static class IUnsafeBindingContextExtensions
	{
		public static object Get(this IUnsafeBindingContext me,object name,object key)
		{
			return me.Get(new BindingName(name), new BindingKey(key));
		}

	
		public static  IUnsafeValueBindingContext Bind(this IUnsafeBindingContext me,object key)
		{
			return me.Bind(new BindingKey(key));
		}

		public static IUnsafeValueBindingContext Bind (this IUnsafeBindingContext me,object name,object key)
		{
			return me.Bind(new BindingName(name),new BindingKey(key));
		}

	}

	public static class IBindingContextExtensions
	{
		static public IValueBindingContext<T> Bind<T> (this IBindingContext me,object name)
		{
			return me.Bind<T>(new BindingName(name));
		}
			
		static public  T Get<T>(this IBindingContext me,object name)
		{
			return me.Get<T>(new BindingName(name));
		}

		static public  T Get<T>(this IBindingContext me,object name, params object[] extras)
		{
			return me.Get<T>(new BindingName(name),extras);
		}
	}

	public static class BindingRequirementsExtensions
	{
		static public IBindingRequirement With<T>(this BindingRequirements me, object name)
		{
			return me.With<T>(new BindingName(name));
		}

		static public IBindingRequirement With(this BindingRequirements me, object name,object key)
		{
			return me.With(new BindingName(name), new BindingKey(key));
		}
	}

	public static class IBindingExtensions
	{
		static public void CheckRequiremets (this IBinding me,object key, object name)
		{
			 me.CheckRequiremets(new BindingKey(key), new BindingName(name));
		}
	}
}
	

