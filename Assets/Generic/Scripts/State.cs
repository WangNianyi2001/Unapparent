using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Data;

namespace Unapparent {
	public class State : MonoBehaviour {
		public static List<Type> actionTypes = new List<Type> {
			typeof(Sequential),
			typeof(SwitchState),
		};

		[Serializable]
		public abstract class Action : IInspectable {
			public static readonly System.Action Nil = delegate () { };
			public static Action Make(Type t) {
				if(!t.IsSubclassOf(typeof(Action))) {
					throw new ConstraintException("" + t.Name + " is not a derived class from Action.");
				}
				return (Action)Activator.CreateInstance(t);
			}
			public abstract void Execute();
			public virtual void Inspect(System.Action header, System.Action footer) {
				// TODO: default inspector view
			}
		}

		public class ActionList : Sequential {
			public readonly string name;
			public ActionList(string name) {
				this.name = name;
			}
			public void Inspect() {
				IGUI.Bold(name);
				base.Inspect(Nil, Nil);
			}
		}

		public ActionList
			enter = new ActionList("Enter"),
			update = new ActionList("Update"),
			exit = new ActionList("Exit");
	}

	[DisallowMultipleComponent]
	[CustomEditor(typeof(State))]
	public class StateInspector : Inspector<State> {
		public override void OnInspectorGUI() {
			target.enter.Inspect();
			IGUI.HorizontalLine();
			target.update.Inspect();
			IGUI.HorizontalLine();
			target.exit.Inspect();
		}
	}
}