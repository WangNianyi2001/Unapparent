using System;
using UnityEngine;

namespace Unapparent {
	public class BoolConstant : Constant<bool> {
		public override void Inspect(ArgList<Action> elements) {
			elements[0]?.Invoke();
			IGUI.Toggle(ref value, GUIContent.none);
			elements[1]?.Invoke();
		}
	}
}
