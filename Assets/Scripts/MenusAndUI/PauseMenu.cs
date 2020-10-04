using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseMenu : Menu {
	public Slider master, music, sfx;

	public AudioMixer audioMixer;

	float oldTimeScale;

	public void Start() {

		master.value = PlayerPrefs.GetFloat("MasterVolume", 1);
		music.value = PlayerPrefs.GetFloat("MusicVolume", 1);
		sfx.value = PlayerPrefs.GetFloat("SFXVolume", 1);
	}

	public override void Open() {
		oldTimeScale = Time.timeScale;
		Time.timeScale = 0;
		base.Open();
	}

	public override bool Close() {
		Time.timeScale = oldTimeScale;
		PlayerPrefs.Save();
		return base.Close();
	}

	public void AdjustedSlider(Slider s) {
		float sliderValue = Mathf.Log10(s.value) * 20;
		string sliderName;
		if (s == master) {
			sliderName = "MasterVolume";
		} else if (s == music) {
			sliderName = "MusicVolume";
		} else if (s == sfx) {
			sliderName = "SFXVolume";
		} else {
			return;
		}

		audioMixer.SetFloat(sliderName, sliderValue);
		PlayerPrefs.SetFloat(sliderName, s.value);
	}
}
