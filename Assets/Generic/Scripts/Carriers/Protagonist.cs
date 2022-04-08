using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Unapparent {
	public class Protagonist : Character {
		[NonSerialized] public Identity shape;
		public override Identity appearance => shape;

		public bool ShapeshiftInto(Identity target) {
			shape = target;
			return true;
		}

		[NonSerialized] public bool canMoveActively = true;

		public void DoMouseNavigation() {
			Ray ray = Level.current.camera.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if(!Physics.Raycast(ray, out hit, Level.current.camera.farClipPlane))
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

		public new void Start() {
			base.Start();
			shape = identity;
		}

		public new void Update() {
			base.Update();
			if(!EventSystem.current.IsPointerOverGameObject())
				DoMouseWorks();
		}
	}
}
