using System;
using UnityEngine;

namespace Unapparent {
	[CreateAssetMenu]
	public class SwitchState : Command {
		GameObject destination = null;

		public override object Execute() {
			// TODO
			return null;
		}

		public override void Inspect(Action header, Action footer) {
			IGUI.Inline(() => {
				header?.Invoke();
				IGUI.Label("Switch to state");
				destination = IGUI.ObjectField(
					destination, typeof(GameObject), true,
					GUILayout.ExpandWidth(true)
				) as GameObject;
				IGUI.FillLine();
				footer?.Invoke();
			});
		}
	}
}
