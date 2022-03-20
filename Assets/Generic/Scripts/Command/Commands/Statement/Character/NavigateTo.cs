﻿using UnityEngine;

namespace Unapparent {
	public class NavigateTo : Statement {
		public GameObject destination;
		public float tolerance = 1f;
		public Statement arrival = null;

		public override object Execute(Carrier target) {
			Character character = target as Character;
			character.Arrival = arrival;
			character.NavigateTo(destination.transform.position, tolerance);
			return null;
		}
	}
}
