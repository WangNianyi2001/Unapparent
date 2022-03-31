using UnityEngine;

namespace Unapparent {
	public class CloseLogue : Statement {
		[Header("Close logue")]
		public bool dummy = true;
		public override object Execute(Carrier target) {
			Level.current.CloseMonologue();
			return null;
		}
	}
}
