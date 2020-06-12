using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
	public Text coinsText;
	public string coinsTextDefault = "₦: ";

	Player p;

	void Start() {
		p = Player.instance;
		p.coinsChanged.AddListener(CoinsChanged);
	}

	public void CoinsChanged() {
		coinsText.text = coinsTextDefault + p.Coins;
	}
}
