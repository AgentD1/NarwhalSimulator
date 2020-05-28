using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour, IDamageable {
	public string DamageLayer { get; protected set; } = "Unfriendly";

	public bool CanBeDamaged(string damageLayer) => DamageLayers.damageLayers[damageLayer][DamageLayer];

	public void Damage(float damage) {
		throw new System.NotImplementedException();
	}

	public void OnCollisionEnter2D(Collision2D collision) {
		if (collision.collider.gameObject.CompareTag("Tusk")) {
			Destroy(gameObject);
		}
	}

}
