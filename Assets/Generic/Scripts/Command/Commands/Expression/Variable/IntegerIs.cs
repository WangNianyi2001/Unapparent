using System.Threading.Tasks;

namespace Unapparent {
	public class IntegerIs : Expression {
		public new string name;
		public int anchor;
		public enum Condition {
			Greator, Less, Equal, NotEqual
		};
		public Condition condition;

		public override Task<object> Execute(Carrier subject) {
			bool result = false;
			int value = Variable<int>.Get(name).value;
			switch(condition) {
				case Condition.Greator:
					result = value > anchor;
					break;
				case Condition.Less:
					result = value < anchor;
					break;
				case Condition.Equal:
					result = value == anchor;
					break;
				case Condition.NotEqual:
					result = value != anchor;
					break;
			}
			return Task.FromResult<object>(result);
		}
	}
}
