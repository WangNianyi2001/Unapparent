using UnityEngine;

namespace Unapparent {
	public class SwitchState : Statement {
		public GameObject destination = null;

		public override object Execute(Carrier target) =>
			target.state = destination.GetComponent<State>();
	}
}
