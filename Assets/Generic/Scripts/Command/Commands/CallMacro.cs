using System.Threading.Tasks;

namespace Unapparent {
	public class CallMacro : Statement {
		public Carrier subject = null;
		public Macro macro;

		public override Task<object> Execute(Carrier subject) {
			if(this.subject != null)
				subject = this.subject;
			else if(macro.subject != null)
				subject = macro.subject;
			return macro.command.Execute(subject);
		}
	}
}
