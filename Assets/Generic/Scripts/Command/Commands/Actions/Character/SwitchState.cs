using System;
using UnityEngine;

namespace Unapparent {
	public class SwitchState : Command {
		public GameObject destination = null;

		public override object Execute(Carrier target) =>
			target.state = destination.GetComponent<State>();

		public override void Inspect(ArgList<Action> elements) {
			IGUI.Inline(() => {
				elements[0]?.Invoke();
				IGUI.Label("Switch to state");
				if(IGUI.ObjectField(
					ref destination, typeof(GameObject), true,
					GUILayout.ExpandWidth(true)
				))
					SetDirty();
				IGUI.FillLine();
				ShowRefBtn();
				elements[1]?.Invoke();
			});
		}
	}
}
