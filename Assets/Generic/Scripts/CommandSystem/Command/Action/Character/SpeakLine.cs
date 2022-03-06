using System;

namespace Unapparent {
	public class SpeakLine : Command {
		public string text;
		public Command next = null;

		public override object Execute(Carrier target) {
			Monologue monologue = new Monologue();
			monologue.character = target as Character;
			monologue.text = text;
			Monologue.Option option;
			option.text = "Next";
			option.command = next;
			monologue.options.Add(option);
			Level.current.ShowMonologue(monologue);
			return null;
		}

		public override void Inspect(ArgList<Action> elements) {
			IGUI.Inline(() => {
				elements[0]?.Invoke();
				IGUI.Label("Speak line");
				string old = text;
				text = IGUI.TextField(text);
				if(old != text)
					SetDirty();
				elements[1]?.Invoke();
			});
		}
	}
}
