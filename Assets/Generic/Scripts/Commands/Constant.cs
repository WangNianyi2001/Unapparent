using System;

namespace Unapparent {
	public class BoolConstant : Condition {
		bool value = false;

		public override bool Execute(object arg) {
			return value;
		}

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