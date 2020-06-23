using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainbowSprite : MonoBehaviour {
	SpriteRenderer spriteRenderer;
	public float hue = 0f;
	public float speed = 3f;
	public void Start() {
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void Update() {
		spriteRenderer.color = Color.HSVToRGB(hue, 1f, 1f);
		hue += speed;
		if (hue > 1f) {
			hue--;
		}
	}
}
