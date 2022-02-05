using System;
using System.Data;
using UnityEngine;

namespace Unapparent {
	[Serializable]
	public abstract class Statement : IInspectable, ISerializationCallbackReceiver {
		public static readonly System.Action Nil = delegate () { };
		public static Statement Make(Type t) {
			if(!t.IsSubclassOf(typeof(Statement))) {
				throw new ConstraintException("" + t.Name + " is not a derived class from Action.");
			}
			return (Statement)Activator.CreateInstance(t);
		}
		public abstract void Execute();
		public virtual void Inspect(System.Action header, System.Action footer) {
			// TODO: default inspector view
		}
		public virtual void OnBeforeSerialize() {
			// TODO
		}
		public virtual void OnAfterDeserialize() {
			// TODO
		}
	}
}