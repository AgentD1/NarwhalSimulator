using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GamewideManager : MonoBehaviour {
	public static GamewideManager instance;
	public void Awake() {
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad(gameObject);
		} else {
			if (instance != this) {
				Destroy(gameObject);
			}
		}
	}

	public void Update() {
		if (UnityEngine.InputSystem.Keyboard.current.f2Key.wasPressedThisFrame) {
			string fileName = DateTime.Now.ToString();
			fileName = fileName.Replace("/", "-").Replace(" ", "_").Replace(":", "-");
			string path = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "Screenshots" + Path.AltDirectorySeparatorChar;
			if (!File.Exists(path)) {
				Directory.CreateDirectory(path);
			}

			path += "NarwhalSimulator" + fileName + ".png";
			ScreenCapture.CaptureScreenshot(path);
			Debug.Log("Screenshot taken and saved as " + path);
		}
	}
}
