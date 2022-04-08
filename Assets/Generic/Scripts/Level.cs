using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Unapparent {
	[ExecuteInEditMode]
	public class Level : MonoBehaviour {
		public static Level current => SceneManager.GetActiveScene().GetRootGameObjects()[0]?.GetComponent<Level>();

		public new Camera camera;
		public RectTransform monologue;
		public Text logueName;
		public Text logueText;
		public RectTransform logueOptions;

		public void ClearOptions() {
			GameObject optionsObj = monologue.transform.Find("Content/Options").gameObject;
			foreach(Transform child in optionsObj.transform)
				Destroy(child.gameObject);
		}

		public async Task<object> ShowMonologue(Character character, Monologue.Content content) {
			if(character == null)
				return new NullReferenceException();
			ClearOptions();
			logueName.text = character.identity.name;
			logueText.text = content.text;
			TaskCompletionSource<object> promise = new TaskCompletionSource<object>();
			foreach(Monologue.Content.Option option in content.options) {
				GameObject optionOBtn = Instantiate(Resources.Load<GameObject>("Option Button"));
				optionOBtn.GetComponentInChildren<Text>().text = option.text;
				Button.ButtonClickedEvent ev = new Button.ButtonClickedEvent();
				ev.AddListener(() => {
					current.CloseMonologue();
					option.action?.Execute(character);
					promise.TrySetResult(null);
				});
				optionOBtn.GetComponentInChildren<Button>().onClick = ev;
				optionOBtn.transform.SetParent(logueOptions.gameObject.transform, false);
			}
			monologue.gameObject.SetActive(true);
			return await promise.Task;
		}

		public void CloseMonologue() {
			monologue.gameObject.SetActive(false);
			ClearOptions();
		}
	}
}
