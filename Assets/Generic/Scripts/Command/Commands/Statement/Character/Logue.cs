using System.Collections.Generic;
using System.Threading.Tasks;

namespace Unapparent {
	public class Logue : Statement {
		public Character character;
		public List<string> contents = new List<string>() { "Text" };
		public List<Monologue.Content.Option> options = new List<Monologue.Content.Option>() {
			new Monologue.Content.Option()
		};

		public override async Task<object> Execute() {
			Monologue.Content content;
			for(int i = 0; i < contents.Count - 1; ++i) {
				content = new Monologue.Content();
				content.text = contents[i];
				await Level.current.ShowMonologue(character, content);
			}
			content = new Monologue.Content();
			content.text = contents[contents.Count - 1];
			content.options = options;
			return await Level.current.ShowMonologue(character, content);
		}
	}
}
