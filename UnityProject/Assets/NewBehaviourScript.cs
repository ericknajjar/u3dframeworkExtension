using UnityEngine;
using System.Collections;
using u3dExtensions.Engine.Runtime;

[BindingProvider()]
public class NewBehaviourScript : MonoBehaviour 
{

	[BindingProvider(Name = "john",DependencyCount = 1)]
	public static int Func1(float dependencie)
	{
		return (int)(3*dependencie);
	}

	[BindingProvider(Name = "john2",DependencyCount = 1)]
	public static int Func2(float dependencie)
	{
		return (int)(2*dependencie);
	}

	[BindingProvider()]
	public static float Func2()
	{
		return 33.0f;
	}
	
}
