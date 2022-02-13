using System;
using UnityEngine;
using UnityEditor;

namespace Unapparent {
	public class SwitchState : ICommand {
		GameObject destination = null;

		public object Execute() {
			// TODO
			return null;
		}

		public void Inspect(Action header, Action footer) {
			IGUI.Inline(delegate {
				header();
				IGUI.Label("Switch to state");
				destination = EditorGUILayout.ObjectField(
					destination,
					typeof(GameObject),
					true,
					GUILayout.ExpandWidth(true)
				) as GameObject;
				IGUI.FillLine();
				footer();
			});
		}
	}
}
