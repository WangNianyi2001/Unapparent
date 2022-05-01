using System.Threading.Tasks;

namespace Unapparent {
	public class HasTag : Expression {
		public string tag;

		public override Task<object> Execute(Carrier subject) =>
			Task.FromResult<object>((subject as Character).Appearance.tags.Contains(tag));
	}
}
