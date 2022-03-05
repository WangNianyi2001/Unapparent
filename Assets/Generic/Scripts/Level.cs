using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Unapparent {
	[ExecuteInEditMode]
	public class Level : MonoBehaviour {
		public static Level current => SceneManager.GetActiveScene().GetRootGameObjects()[0]?.GetComponent<Level>();

		public GameObject ui;

		public void ShowMonologue(Monologue monologue) {
			ui.transform.Find("Name").GetComponent<Text>().text = monologue.character.displayName;
			ui.transform.Find("Text").GetComponent<Text>().text = monologue.text;
			GameObject optionsObj = ui.transform.Find("Options").gameObject;
			foreach(Monologue.Option option in monologue.options) {
				GameObject optionObj = Monologue.MakeOptionButton(option);
				optionObj.transform.parent = optionsObj.transform;
			}
			ui.gameObject.SetActive(true);
		}

		public void CloseMonologue() {
			GameObject options = ui.transform.Find("Options").gameObject;
			while(options.transform.childCount > 0)
				Destroy(options.transform.GetChild(0).gameObject);
		}

#if UNITY_EDITOR
		public void Update() {
			ui = current.gameObject.GetComponentInChildren<Canvas>().gameObject;
		}
#endif
	}
}
