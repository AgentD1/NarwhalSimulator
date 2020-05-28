using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour {

	Controls input;
	Rigidbody2D rb;

	public float speed = 3f;
	public float torque = 3f;

	public void Awake() {
		rb = GetComponent<Rigidbody2D>();
	}

	public void Start() {
		input = new Controls();
		input.Enable();
	}


	public void FixedUpdate() {
		//Debug.Log(input.Player.Move.ReadValue<Vector2>().x);
		float move = input.Player.Move.ReadValue<Vector2>().y;
		float turn = input.Player.Move.ReadValue<Vector2>().x;
		rb.AddRelativeForce(new Vector2(0, move * speed));
		rb.AddTorque(turn * torque);
	}
}
