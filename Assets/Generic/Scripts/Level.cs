using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Unapparent {
	[ExecuteInEditMode]
	public class Level : MonoBehaviour {
		public static Level current => SceneManager.GetActiveScene().GetRootGameObjects()[0]?.GetComponent<Level>();

		public GameObject canvas, monologueObj;

		public void ClearOptions() {
			GameObject optionsObj = monologueObj.transform.Find("Content/Options").gameObject;
			foreach(Transform child in optionsObj.transform)
				Destroy(child.gameObject);
		}

		public void ShowMonologue(Monologue monologue) {
			ClearOptions();
			Transform info = monologueObj.transform.Find("Content/Info");
			info.Find("Name").GetComponent<Text>().text = monologue.character.profile.name;
			info.Find("Text").GetComponent<Text>().text = monologue.text;
			GameObject optionsObj = monologueObj.transform.Find("Content/Options").gameObject;
			foreach(Monologue.Option option in monologue.options) {
				GameObject optionObj = monologue.MakeOptionButton(option);
				optionObj.transform.SetParent(optionsObj.transform, false);
			}
			monologueObj.SetActive(true);
		}

		public void CloseMonologue() {
			monologueObj.SetActive(false);
			ClearOptions();
		}

#if UNITY_EDITOR
		public void Update() {
			canvas = current.gameObject.GetComponentInChildren<Canvas>(true).gameObject;
			monologueObj = canvas.transform.Find("Monologue").gameObject;
		}
#endif
	}
}
