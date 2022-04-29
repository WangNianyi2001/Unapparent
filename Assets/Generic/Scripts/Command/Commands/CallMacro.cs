using System.Threading.Tasks;

namespace Unapparent {
	public class CallMacro : Statement {
		public Macro macro;

		public override Task<object> Execute(Carrier subject) =>
			macro.command.Execute(subject);
	}
}
