using UnityEngine;

namespace Unapparent {
	public abstract class TriggerListener : Listener {
		public Collider trigger;

		public override bool Validate(Carrier target, params object[] args) {
			if(args.Length < 1)
				return false;
			return trigger == args[0] as Collider;
		}
	}
}
