using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Unapparent {
	public abstract class InspectableList<T> where T : Object {
		[SerializeField] public List<T> elements = new List<T>();

		public virtual void OnAdd() { }
	}
}
