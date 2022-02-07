using System;

namespace Unapparent {
	public abstract class Constant<Type> : Condition {
		public Type value = default(Type);
	}

	public class BoolConstant : Constant<bool> {
		public override bool Execute(object arg) => value;

		public override void Inspect(Action header, Action footer) {
			header();
			IGUI.Toggle(ref value, UnityEngine.GUIContent.none);
			footer();
		}

		public override void OnAfterDeserialize() {
			throw new NotImplementedException();
		}

		public override void OnBeforeSerialize() {
			throw new NotImplementedException();
		}
	}
}