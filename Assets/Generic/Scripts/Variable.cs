using System.Collections.Generic;
using UnityEngine;

namespace Unapparent {
	public class Variable<T> {
		public T value;

		public static Dictionary<string, Variable<T>> dictionary;

		public static Variable<T> Create(string name, T value) {
			if(dictionary.ContainsKey(name))
				return dictionary[name];
			var variable = new Variable<T>();
			variable.value = value;
			dictionary[name] = variable;
			return variable;
		}

		public static Variable<T> Get(string name) {
			if(!dictionary.ContainsKey(name))
				throw new MissingReferenceException();
			return dictionary[name];
		}
	}
}
