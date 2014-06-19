using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using u3dExtensions;
using u3dExtensions.IOC;
using u3dExtensions.IOC.extesions;

namespace u3dExtensions.Engine.Runtime
{
	public interface IBindingProvider 
	{
		IList<IBindingProvider> Dependencies{get;}
	}


}