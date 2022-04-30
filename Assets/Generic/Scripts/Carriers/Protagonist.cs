using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Unapparent {
	public class Protagonist : Character {
		public bool ShapeshiftInto(Identity target) {
			Appearance = target;
			return true;
		}

		[NonSerialized] public bool canMoveActively = true;
		public List<Identity> shapeshiftables;

		static LayerMask walkable;
		public void DoMouseNavigation() {
			Ray ray = Level.current.camera.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if(!Physics.Raycast(ray, out hit, Level.current.camera.farClipPlane, walkable))
				return;
			agent.SetDestination(hit.point);
			agent.isStopped = false;
		}

		public void DoMouseWorks() {
			if(Input.GetMouseButtonDown(0)) {
				if(canMoveActively)
					DoMouseNavigation();
			}
		}

		public void Awake() {
			walkable = LayerMask.GetMask("Walkable");
		}

		public new void Update() {
			base.Update();
			if(!EventSystem.current.IsPointerOverGameObject())
				DoMouseWorks();
		}
	}
}
