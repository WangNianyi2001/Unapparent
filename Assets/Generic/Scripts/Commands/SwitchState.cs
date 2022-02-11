using System;
using UnityEngine;
using UnityEditor;

namespace Unapparent {
	public class SwitchState : Statement {
		GameObject destination = null;

		public override object Execute() {
			// TODO
			return null;
		}

		public override void Inspect(Action header, Action footer) {
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
