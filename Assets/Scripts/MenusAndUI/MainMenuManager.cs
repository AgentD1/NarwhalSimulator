using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {
	public RectTransform loadingSpinner;
	public bool loading = false;
	public float loadingSpinSpeed = 1f;

	public TextMeshProUGUI versionText;
	public string versionTextPrefix = "Version ";

	public void Awake() {
		versionText.text = versionTextPrefix + Application.version;
	}

	public void PlayButtonPressed() {
		if (!loading) {
			StartCoroutine(LoadGameCoroutine());
		}
	}

	public void OptionsButtonPressed() {

	}

	public void ExitButtonPressed() {
		Application.Quit();
	}

	public void Update() {
		if (loading) {
			loadingSpinner.Rotate(new Vector3(0, 0, loadingSpinSpeed));
		}
	}

	public IEnumerator LoadGameCoroutine() {
		loading = true;
		loadingSpinner.gameObject.SetActive(true);
		AsyncOperation o = SceneManager.LoadSceneAsync("MainGame", LoadSceneMode.Single);
		o.allowSceneActivation = true;
		yield return o;
	}
}
