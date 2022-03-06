using System;
using UnityEngine;

namespace Unapparent {
	public class NavigateTo : Command {
		public GameObject destination;
		public float remainingDistance = 1f;
		public Command arrival = null;
		public bool useArrival = false;

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
				if(IGUI.ObjectField(ref destination, true))
					SetDirty();
				if(IGUI.Toggle(ref useArrival))
					SetDirty();
				if(!useArrival)
					elements[1]?.Invoke();
			});
			if(useArrival) {
				if(arrival == null)
					arrival = Create<Sequential>(this);
				IGUI.Inline(
					() => arrival.Inspect(() => IGUI.Label("Arrive"),
					() => {
						if(IGUI.Button("Remove")) {
							if(IGUI.Confirm("Removing arrival action, proceed?")) {
								arrival?.Dispose();
								arrival = null;
							}
						}
					})
				);
			}
		}

		public override void Dispose() {
			arrival?.Dispose();
			base.Dispose();
		}
	}
}
