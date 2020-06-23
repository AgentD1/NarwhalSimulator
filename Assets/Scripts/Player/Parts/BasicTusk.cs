using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTusk : PlayerPart {
	public float damage = 0.1f;
	public string damageLayer = "Friendly";

	public BasicTusk() {
		type = "Tusk";
	}

	public void OnCollisionEnter2D(Collision2D collision) {
		if (!collision.otherCollider.CompareTag("Tusk")) {
			return;
		}
		IDamageable dam = collision.collider.GetComponent<IDamageable>();
		if (dam != null) {
			if (dam.CanBeDamaged(damageLayer)) {
				Vector2 mySpeed = player.rb.GetPointVelocity(collision.GetContact(0).point);
				Vector2 otherSpeed = collision.rigidbody.GetPointVelocity(collision.GetContact(0).point);

				dam.Damage((mySpeed - otherSpeed).magnitude * damage, collision.GetContact(0).point - collision.GetContact(0).normal * 0.01f);
			}
		}
	}
}
