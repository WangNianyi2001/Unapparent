using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Unapparent {
	[Serializable]
	public abstract class Command : ScriptableObject {
		public static Command Create(Type type) => CreateInstance(type) as Command;

		public static T Create<T>() where T : Command => Create(typeof(T)) as T;

		public virtual async Task<object> Execute() {
			throw new NotImplementedException();
		}
	}
}
