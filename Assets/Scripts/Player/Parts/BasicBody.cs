using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBody : PlayerPart {
	public float maxHealth = 0f;

	public void Start() {
		Player.instance.RefreshMaxHealth();
	}
}
