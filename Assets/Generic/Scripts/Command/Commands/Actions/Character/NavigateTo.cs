using System;
using UnityEngine;

namespace Unapparent {
	public class NavigateTo : Command {
		GameObject destination;

		public override object Execute(Carrier target) {
			(target as Character).NavigateTo(destination.transform.position);
			return null;
		}

		public override void Inspect(ArgList<Action> elements) {
			IGUI.Inline(() => {
				elements[0]?.Invoke();
				IGUI.Label("Navigate to");
				GameObject old = destination;
				destination = IGUI.ObjectField(destination, typeof(GameObject), true) as GameObject;
				if(old != destination)
					SetDirty();
				elements[1]?.Invoke();
			});
		}
	}
}
