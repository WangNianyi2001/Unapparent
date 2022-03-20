using System;
using UnityEngine;

namespace Unapparent {
	[Serializable]
	public abstract class Command : ScriptableObject {
		public static Command Create(Type type) => CreateInstance(type) as Command;

		public static T Create<T>() where T : Command => Create(typeof(T)) as T;

		public abstract object Execute(Carrier target);
	}
}
