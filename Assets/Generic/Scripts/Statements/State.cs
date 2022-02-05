using System;
using UnityEngine;
using UnityEditor;

namespace Unapparent {
	public class SwitchState : Statement {
		GameObject destination = null;
		public override void Execute() {
			// TODO
		}
		public override void Inspect(Action header, Action footer) {
			GUILayout.BeginHorizontal();
			header();
			IGUI.Label("Switch to state");
			destination = EditorGUILayout.ObjectField(
				destination,
				typeof(GameObject),
				true,
				GUILayout.ExpandWidth(true)
			) as GameObject;
			GUILayout.FlexibleSpace();
			footer();
			GUILayout.EndHorizontal();
		}
	}
}