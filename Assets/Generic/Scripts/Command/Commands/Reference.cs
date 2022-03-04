using System;
using UnityEngine;

namespace Unapparent {
	public class Reference : Command {
		public Command command = null;
		public override object Execute(Carrier target) {
			return command?.Execute(target);
		}

		public override void Inspect(ArgList<Action> elements) {
			IGUI.Inline(() => {
				elements[0]?.Invoke();
				IGUI.Label("Invoke command");
				Command old = command;
				command = IGUI.ObjectField(
					command, typeof(Command), true,
					GUILayout.ExpandWidth(true)
				) as Command;
				if(old != command)
					SetDirty();
				IGUI.FillLine();
				elements[1]?.Invoke();
			});
		}
	}
}
