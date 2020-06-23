using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicFlukes : PlayerPart {
	Controls input;
	Rigidbody2D rb;

	public float speed = 3f;
	public float acceleration = 1f;
	float linearDrag;

	public BasicFlukes() {
		type = "Flukes";
	}

	public void Awake() {
		input = new Controls();
		input.Enable();
	}

	public void Start() {
		rb = player.rb;
		linearDrag = rb.drag;
	}

	public void FixedUpdate() {
		float move = input.Player.Move.ReadValue<Vector2>().y;

		if (move == 0) {
			rb.drag = linearDrag;
		} else {
			float dotProduct = Vector2.Dot(rb.velocity.normalized, transform.up * -Mathf.Sign(move)); // How close are we to facing where we want to go?
			if (move != 0 && 1 != Mathf.Sign(dotProduct)) {
				if (rb.velocity.sqrMagnitude < speed * speed) {
					rb.drag = 0;
				} else {
					rb.drag = rb.velocity.sqrMagnitude - speed * speed;
				}
			} else {
				rb.drag = -dotProduct * linearDrag;
			}
		}
		rb.AddRelativeForce(new Vector2(0, move * acceleration), ForceMode2D.Impulse);
	}
}
