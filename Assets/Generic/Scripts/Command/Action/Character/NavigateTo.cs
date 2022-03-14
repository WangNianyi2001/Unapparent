using System;
using UnityEngine;

namespace Unapparent {
	public class NavigateTo : Command {
		public GameObject destination;
		public float tolerance = 1f;
		public Command arrival = null;
		public bool useArrival = false;

		public override object Execute(Carrier target) {
			Character character = target as Character;
			character.Arrival = arrival;
			character.NavigateTo(destination.transform.position, tolerance);
			return null;
		}
	}
}
