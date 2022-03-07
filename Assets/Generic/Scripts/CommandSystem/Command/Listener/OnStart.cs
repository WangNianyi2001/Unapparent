using System;

namespace Unapparent {
	public class OnStart : CertainListener {
		public override void Inspect(ArgList<Action> elements) {
			IGUI.Label("On Start");
			base.Inspect(elements);
		}
	}
}
