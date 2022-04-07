using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Unapparent {
	public class Monologue : Statement {
		[Serializable]
		public class Content {
			[Serializable]
			public class Option {
				public string text = "Next";
				public Statement action;
			}
			public string text = "Text";
			public List<Option> options = new List<Option>() { new Option() };
		}

		public Character character;
		public Content content = new Content();
		[NonSerialized] public Monologue next = null;

		public override async Task<object> Execute() => await Level.current.ShowMonologue(character, content);
	}
}
