using System;
using UnityEditor;
using UnityEngine;

namespace Unapparent {
	public class Reference : Command {
		public UnityEngine.Object selected;
		public Command command = null;

		public override object Execute(Carrier target) {
			return command?.Execute(target);
		}

		static Command ParseCommand(UnityEngine.Object obj) {
			if(obj is Command)
				return obj as Command;
			if(obj is GameObject) {
				MonoCommand mc = (obj as GameObject).GetComponent<MonoCommand>();
				if(mc)
					return mc.command;
			}
			return null;
		}

		public override void Inspect(ArgList<Action> elements) {
			IGUI.Inline(() => {
				elements[0]?.Invoke();
				IGUI.Label("Invoke command");
				UnityEngine.Object old = selected;
				selected = IGUI.ObjectField(
					selected, typeof(UnityEngine.Object), true,
					GUILayout.ExpandWidth(true)
				);
				command = ParseCommand(selected);
				if(old != selected) {
					command = ParseCommand(selected);
					SetDirty();
				}
				IGUI.FillLine();
				ShowRefBtn();
				elements[1]?.Invoke();
			});
			if(command == null)
				EditorGUILayout.HelpBox("Selected object is not a command.", MessageType.Warning);
		}
	}
}
