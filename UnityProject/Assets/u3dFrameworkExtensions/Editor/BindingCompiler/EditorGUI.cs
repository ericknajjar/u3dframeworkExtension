using UnityEngine;
using System.Collections;
using UnityEditor;
using u3dExtensions.Engine.Runtime;
using u3dExtensions.IOC.extesions;

namespace u3dExtensions.Engine.Editor
{
	public class EditorGUI  
	{
		[MenuItem("Window/u3dExtensions/CheckBindings")]
		static void  Check()
		{
			//var compiler = new BindingCompiler();
			var bindings = new ReflectiveBindingFinder(typeof(ReflectiveBindingFinder).Assembly);

			var context = new ReflectiveBindingContextFactory(bindings).CreateContext();

			var ret = context.Get<float>();
			var john = context.Get<int>("john");
			var john2 = context.Get<int>("john2");

			Debug.Log("Uhul: "+ret);
			Debug.Log("john: "+john);
			Debug.Log("john2: "+john2);
		}
	
	}
}
