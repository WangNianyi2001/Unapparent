using System;
using System.Threading.Tasks;
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

		public async Task<object> ShowMonologue(Character character, Monologue.Content content) {
			if(character == null)
				return new NullReferenceException();
			ClearOptions();
			Transform info = monologueObj.transform.Find("Content/Info");
			info.Find("Name").GetComponent<Text>().text = character.identity.name;
			info.Find("Text").GetComponent<Text>().text = content.text;
			GameObject optionsObj = monologueObj.transform.Find("Content/Options").gameObject;
			TaskCompletionSource<object> promise = new TaskCompletionSource<object>();
			foreach(Monologue.Content.Option option in content.options) {
				GameObject optionOBtn = Instantiate(Resources.Load<GameObject>("Option Button"));
				optionOBtn.GetComponentInChildren<Text>().text = option.text;
				Button.ButtonClickedEvent ev = new Button.ButtonClickedEvent();
				ev.AddListener(async () => {
					var a = option.action?.Execute();
					if(a != null)
						await a;
					current.CloseMonologue();
					promise.TrySetResult(null);
				});
				optionOBtn.GetComponentInChildren<Button>().onClick = ev;
				optionOBtn.transform.SetParent(optionsObj.transform, false);
			}
			monologueObj.SetActive(true);
			return await promise.Task;
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
