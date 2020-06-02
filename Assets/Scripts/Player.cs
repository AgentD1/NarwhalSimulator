using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour {
	Controls input;
	Rigidbody2D rb;

	public string damageLayer = "Friendly";

	public float speed = 3f;
	public float acceleration = 1f;
	float currentSpeed = 0f;
	public float torque = 3f;
	float linearDrag;

	public float damage = 0.1f;
	public float minimumSpeedDamage = 1f;

	public void Awake() {
		rb = GetComponent<Rigidbody2D>();
	}

	public void Start() {
		input = new Controls();
		input.Enable();
		linearDrag = rb.drag;
	}


	public void FixedUpdate() {
		float move = input.Player.Move.ReadValue<Vector2>().y;
		float turn = input.Player.Move.ReadValue<Vector2>().x;
		if (move != 0 && 1 != Mathf.Sign(Vector2.Dot(rb.velocity.normalized, transform.up * -Mathf.Sign(move)))) {
			if (rb.velocity.sqrMagnitude < speed * speed) {
				rb.drag = 0;
			} else {
				rb.drag = rb.velocity.sqrMagnitude - speed * speed;
			}
		} else {
			rb.drag = linearDrag;
		}
		rb.AddRelativeForce(new Vector2(0, move * acceleration), ForceMode2D.Impulse);
		rb.AddTorque(turn * torque);
	}


	public void OnCollisionEnter2D(Collision2D collision) {
		if (collision.otherCollider.tag != "Tusk") {
			return;
		}
		IDamageable dam;
		if ((dam = collision.collider.GetComponent<IDamageable>()) != null) {
			if (dam.CanBeDamaged(damageLayer)) {
				Vector2 mySpeed = rb.GetPointVelocity(collision.GetContact(0).point);
				Vector2 otherSpeed = collision.rigidbody.GetPointVelocity(collision.GetContact(0).point);
				//Vector2 mySpeed = rb.velocity;
				//Vector2 otherSpeed = collision.rigidbody.velocity;

				/*if ((mySpeed - otherSpeed).sqrMagnitude < minimumSpeedDamage * minimumSpeedDamage) {
					return;
				}*/

				dam.Damage((mySpeed - otherSpeed).sqrMagnitude * damage, collision.GetContact(0).point - collision.GetContact(0).normal * 0.01f);
			}
		}
	}
}
