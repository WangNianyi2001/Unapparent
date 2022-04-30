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
				public Expression condition = null;
				public Statement action;
			}
			public string text = "Text";
			public List<Option> options = new List<Option>() { new Option() };
		}

		public Content content = new Content();
		[NonSerialized] public Monologue next = null;

		public override async Task<object> Execute(Carrier subject) =>
			await Level.current.ui.ShowMonologue(subject as Character, content);
	}
}
