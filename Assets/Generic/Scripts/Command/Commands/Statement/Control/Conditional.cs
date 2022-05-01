using System.Threading.Tasks;

namespace Unapparent {
	public class Conditional : Statement {
		public Expression @if = null;

		public Statement then = null, @else = null;

		public override async Task<object> Execute(Carrier subject) {
			if((bool)await @if?.Execute(subject)) {
				if(then != null)
					return await then.Execute(subject);
			}
			else {
				if(@else != null)
					return await @else.Execute(subject);
			}
			return null;
		}
	}
}
