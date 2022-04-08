using UnityEngine;

namespace Unapparent {
	public abstract class TriggerListener : Listener {
		public Collider collider;

		public override bool Validate(Carrier target, params object[] args) {
			if(args.Length < 1)
				return false;
			return collider == args[0] as Collider;
		}
	}
}
