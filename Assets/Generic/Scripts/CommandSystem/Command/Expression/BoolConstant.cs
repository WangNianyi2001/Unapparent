using System;
using UnityEngine;

namespace Unapparent {
	public class BoolConstant : Constant<bool> {
		public override void Inspect(ArgList<Action> elements) {
			elements[0]?.Invoke();
			if(IGUI.Toggle(ref value))
				SetDirty();
			elements[1]?.Invoke();
		}
	}
}
