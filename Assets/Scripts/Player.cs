using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour {
	public static Player instance;

	Controls input;
	Rigidbody2D rb;

	public string damageLayer = "Friendly";

	public float speed = 3f;
	public float acceleration = 1f;
	public float torque = 3f;
	float linearDrag;

	public float damage = 0.1f;
	public float minimumSpeedDamage = 1f;

	public int Coins { get; protected set; } = 0;

	public void Awake() {
		instance = this;
		rb = GetComponent<Rigidbody2D>();
	}

	public void Start() {
		input = new Controls();
		input.Enable();
		linearDrag = rb.drag;
		ResetCoins();
	}


	public void FixedUpdate() {
		float move = input.Player.Move.ReadValue<Vector2>().y;
		float turn = input.Player.Move.ReadValue<Vector2>().x;

		float dotProduct = Vector2.Dot(rb.velocity.normalized, transform.up * -Mathf.Sign(move));
		if (move == 0) {
			rb.drag = linearDrag;
		} else {
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

				dam.Damage((mySpeed - otherSpeed).magnitude * damage, collision.GetContact(0).point - collision.GetContact(0).normal * 0.01f);
			}
		}
	}

	public UnityEvent coinsChanged { get; protected set; } = new UnityEvent();
	public UnityEvent coinsGained { get; protected set; } = new UnityEvent();
	public UnityEvent coinsLost { get; protected set; } = new UnityEvent();
	public UnityEvent coinsReset { get; protected set; } = new UnityEvent();


	public void GiveCoins(int coins) {
		if (coins <= 0) {
			Debug.LogWarning("Can't subtract coins with GiveCoins " + coins);
			return;
		}
		Coins += coins;
		coinsChanged.Invoke();
		coinsGained.Invoke();
	}

	public void RemoveCoins(int coins) {
		if (coins <= 0) {
			Debug.LogWarning("Can't add coins with GiveCoins " + coins);
			return;
		}
		Coins -= coins;
		coinsChanged.Invoke();
		coinsLost.Invoke();
	}

	public void ChangeCoins(int coins) {
		if (coins == 0) {
			Debug.LogWarning("Can't change coins by 0");
		}
		Coins += coins;
		if (coins < 0) {
			coinsLost.Invoke();
		} else {
			coinsGained.Invoke();
		}
		coinsChanged.Invoke();
	}

	public void ResetCoins() {
		Coins = 0;
		coinsChanged.Invoke();
		coinsReset.Invoke();
	}
}
