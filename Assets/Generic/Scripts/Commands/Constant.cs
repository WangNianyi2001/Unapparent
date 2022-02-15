using System;

namespace Unapparent {
	public abstract class Constant<T> : ICommand {
		public T value = default(T);
		
		public object Execute() => value;

		public abstract void Inspect(Action header, Action footer);
	}

	public class BoolConstant : Constant<bool> {
		public override void Inspect(Action header, Action footer) {
			header();
			IGUI.Toggle(ref value, UnityEngine.GUIContent.none);
			footer();
		}
	}
}
