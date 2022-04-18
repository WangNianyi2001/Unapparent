using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unapparent
{
	public class DialogueUI : MonoBehaviour
	{
		private Animator animator;
		private bool shown;
		private static readonly int ShowTrigger = Animator.StringToHash("Show");
		private static readonly int HideTrigger = Animator.StringToHash("Hide");

		private void Awake()
		{
			animator = GetComponent<Animator>();
			shown = false;
		}

		public void SetVisible(bool v)
		{
			if (v && !shown) animator.SetTrigger(ShowTrigger);
			if (!v && shown) animator.SetTrigger(HideTrigger);
			shown = v;
		}
	}
}