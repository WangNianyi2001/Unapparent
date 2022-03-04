using System;
using UnityEngine;

namespace Unapparent {
	public class SwitchState : Command {
		public GameObject destination = null;

		public override object Execute() {
			// TODO
			return null;
		}

		public override void Inspect(ArgList<Action> elements) {
			IGUI.Inline(() => {
				elements[0]?.Invoke();
				IGUI.Label("Switch to state");
				GameObject old = destination;
				destination = IGUI.ObjectField(
					destination, typeof(GameObject), true,
					GUILayout.ExpandWidth(true)
				) as GameObject;
				if(old != destination)
					SetDirty();
				IGUI.FillLine();
				ShowRefBtn();
				elements[1]?.Invoke();
			});
		}
	}
}
