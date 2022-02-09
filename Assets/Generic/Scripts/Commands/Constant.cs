using System;

namespace Unapparent {
	public abstract class Constant<Type> : Condition {
		public Type value = default(Type);
	}

	public class BoolConstant : Constant<bool> {
		public override bool Execute() => value;

		public override void Inspect(Action header, Action footer) {
			header();
			IGUI.Toggle(ref value, UnityEngine.GUIContent.none);
			footer();
		}
	}
}
