using System;
using UnityEngine;

namespace Unapparent {
	public class PrintLog : Command {
		public string content = "";

		public override object Execute(Carrier target) {
			Debug.Log(content);
			return null;
		}

		public override void Inspect(ArgList<Action> elements) {
			IGUI.Inline(() => {
				elements[0]?.Invoke();
				IGUI.Label("Print log");
				string old = content;
				content = IGUI.TextField(content);
				if(old != content)
					SetDirty();
				elements[1]?.Invoke();
			});
		}
	}
}
