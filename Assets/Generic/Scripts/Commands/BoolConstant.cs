using System;
using UnityEngine;

namespace Unapparent {
	public class BoolConstant : Constant<bool> {
		public override void Inspect(Action header, Action footer) {
			header?.Invoke();
			IGUI.Toggle(ref value, GUIContent.none);
			footer?.Invoke();
		}
	}
}
