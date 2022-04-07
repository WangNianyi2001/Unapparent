using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Unapparent {
	public class Monologue : Statement {
		[Serializable]
		public class Option {
			public string text;
			public Statement action;
		}

		public Character character;
		public string text;
		public List<Option> options = new List<Option>();
		[NonSerialized] public Monologue next = null;

		public override async Task<object> Execute() => await Level.current.ShowMonologue(this);
	}
}
