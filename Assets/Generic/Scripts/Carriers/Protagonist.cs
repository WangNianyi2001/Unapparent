namespace Unapparent {
	public class Protagonist : Character {
		public Identity shape;
		public override Identity appearance => shape;

		public new void Start() {
			base.Start();
			if(shape == null)
				shape = identity;
		}
	}
}
