using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unapparent
{
	[DisallowMultipleComponent]
	public class UIController : MonoBehaviour
	{
		public static UIController main;
		
		public Animator currentState;
		
		private static readonly int ShowTrigger = Animator.StringToHash("Show");
		private static readonly int HideTrigger = Animator.StringToHash("Hide");

		private void Awake()
		{
			if (main == null) main = this;
			else Destroy(this);
		}

		public void ChangeTo(Animator state)
		{
			if (currentState != null) currentState.SetTrigger(HideTrigger);
			currentState = state;
			if (currentState != null) currentState.SetTrigger(ShowTrigger);
		}
	}
}
