using System;
using UnityEngine;

namespace Unapparent {
	public abstract class Command : ScriptableObject {
		public abstract object Execute();
		public abstract void Inspect(Action header, Action footer);
	}
}
