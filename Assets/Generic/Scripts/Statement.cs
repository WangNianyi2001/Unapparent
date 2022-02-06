using System;
using System.Data;
using UnityEngine;

namespace Unapparent {
	public interface IStatement : IInspectable, ISerializationCallbackReceiver {
		public void Execute();
	}

	[Serializable]
	public abstract class Statement : IStatement {
		public static Statement Make(Type t) {
			if(!t.IsSubclassOf(typeof(Statement))) {
				throw new ConstraintException("" + t.Name + " is not a derived class from Action.");
			}
			return (Statement)Activator.CreateInstance(t);
		}
		public abstract void Execute();
		public abstract void Inspect(Action header, Action footer);
		public abstract void OnAfterDeserialize();
		public abstract void OnBeforeSerialize();
	}
}