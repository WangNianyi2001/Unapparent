using System;

namespace Unapparent {
	public class Protagonist : Character {
		[NonSerialized] public Identity shape;
		public override Identity appearance => shape;

		public bool ShapeshiftInto(Identity target) {
			shape = target;
			return true;
		}

		public new void Start() {
			base.Start();
			shape = identity;
		}
	}
}
