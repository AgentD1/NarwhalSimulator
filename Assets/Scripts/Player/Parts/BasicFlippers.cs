using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicFlippers : PlayerPart {
	Controls input;
	Rigidbody2D rb;

	public float torque = 3f;

	public BasicFlippers() {
		//type = "Flippers";
	}

	public void Awake() {
		input = new Controls();
		input.Enable();
	}

	public void Start() {
		rb = player.rb;
	}

	public void FixedUpdate() {
		float turn = input.Player.Move.ReadValue<Vector2>().x;
		rb.AddTorque(turn * torque);
	}
}
