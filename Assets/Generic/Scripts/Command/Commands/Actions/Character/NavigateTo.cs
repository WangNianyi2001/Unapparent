using System;
using UnityEngine;

namespace Unapparent {
	public class NavigateTo : Command {
		public GameObject destination;
		public Command arrival = null;
		bool hasArrival {
			get => arrival != null;
			set {
				if(hasArrival ^ !value)
					return;
				if(value == true)
					arrival = Create(typeof(Sequential));
				else {
					arrival.Dispose();
					arrival = null;
				}
				SetDirty();
			}
		}

		public override object Execute(Carrier target) {
			Character character = target as Character;
			character.Arrival = arrival;
			character.NavigateTo(destination.transform.position);
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
				if(!hasArrival) {
					hasArrival = IGUI.Toggle(hasArrival, new GUIContent("Arrival action"));
					elements[1]?.Invoke();
				}
			});
			if(hasArrival) {
				IGUI.Inline(() =>
					arrival.Inspect(() => IGUI.Label("Arrive"), () => {
						if(IGUI.Button("Remove")) {
							if(IGUI.Confirm("Removing arrival action, proceed?"))
								hasArrival = false;
						}
					}));
			}
		}
	}
}
