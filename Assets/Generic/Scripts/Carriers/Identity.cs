using System.Collections.Generic;
using UnityEngine;

namespace Unapparent {
	public class Identity : ScriptableObject {
		public new string name;
		public Texture2D portrait, avatar, left, right;
		public List<string> tags;
	}
}
