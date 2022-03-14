using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Unapparent {
	[Serializable]
	public class InspectableList<T> where T : Object {
		[SerializeField] public List<T> elements = new List<T>();
		public virtual void OnAdd() { }
	}
}
