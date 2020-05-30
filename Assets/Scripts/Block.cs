using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour, IDamageable {
	public string DamageLayer { get; protected set; } = "Unfriendly";

	public bool CanBeDamaged(string damageLayer) => DamageLayers.damageLayers[damageLayer][DamageLayer];

	public float health = 10f;

	public void Damage(float damage) {
		health -= damage;
		if (health <= 0) {
			Destroy(gameObject);
		}
	}
}
