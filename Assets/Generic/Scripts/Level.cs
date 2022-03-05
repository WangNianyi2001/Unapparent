using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Unapparent {
	[ExecuteInEditMode]
	public class Level : MonoBehaviour {
		public static Level current => SceneManager.GetActiveScene().GetRootGameObjects()[0]?.GetComponent<Level>();

		public GameObject canvas;

		public void ShowMonologue(Monologue monologue) {
			Transform info = canvas.transform.Find("Monologue/Content/Info");
			info.Find("Name").GetComponent<Text>().text = monologue.character.displayName;
			info.Find("Text").GetComponent<Text>().text = monologue.text;
			GameObject optionsObj = canvas.transform.Find("Monologue/Content/Options").gameObject;
			foreach(Monologue.Option option in monologue.options) {
				GameObject optionObj = Monologue.MakeOptionButton(option);
				optionObj.transform.parent = optionsObj.transform;
			}
			canvas.gameObject.SetActive(true);
		}

		public void CloseMonologue() {
			GameObject options = canvas.transform.Find("Options").gameObject;
			while(options.transform.childCount > 0)
				Destroy(options.transform.GetChild(0).gameObject);
		}

#if UNITY_EDITOR
		public void Update() {
			canvas = current.gameObject.GetComponentInChildren<Canvas>().gameObject;
		}
#endif
	}
}
