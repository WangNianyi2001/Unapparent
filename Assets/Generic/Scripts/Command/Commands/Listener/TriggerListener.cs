using UnityEngine;

namespace Unapparent {
	public abstract class TriggerListener : List {
		public Collider trigger;

		public override bool Validate(params object[] args) {
			if(args.Length < 1)
				return false;
			return trigger == args[0] as Collider;
		}
	}
}
