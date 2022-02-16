using System;
using UnityEngine;

namespace Unapparent {
	public abstract class Constant<T> : Command {
		public T value = default(T);
		
		public override object Execute() => value;

		public override abstract void Inspect(Action header, Action footer);
	}

	[CreateAssetMenu]
	public class BoolConstant : Constant<bool> {
		public override void Inspect(Action header, Action footer) {
			header?.Invoke();
			IGUI.Toggle(ref value, GUIContent.none);
			footer?.Invoke();
		}
	}
}
