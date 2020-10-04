using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
	public Text coinsText;
	public string coinsTextDefault = "₦: ";
	public RectTransform healthbarForeground;
	public RectTransform healthbarBackground;

	Player p;

	void Start() {
		p = Player.instance;
		p.coinsChanged.AddListener(CoinsChanged);
		p.healthChanged.AddListener(HealthChanged);
	}

	public void CoinsChanged() {
		coinsText.text = coinsTextDefault + p.coins;
	}

	public void HealthChanged() {
		if (p.maxHealth == 0) {
			return;
		}
		healthbarForeground.sizeDelta = new Vector2(-healthbarBackground.rect.width * (1 - (p.health / p.maxHealth)), healthbarForeground.sizeDelta.y);
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "UnityActions can't point to static methods")]
	public void CloseGame() {
		Application.Quit(0);
	}
}
