using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour, IDamageable {
	public string damageLayer { get; protected set; } = "Unfriendly";

	public bool CanBeDamaged(string damageLayer) => DamageLayers.damageLayers[damageLayer][this.damageLayer];

	public float health = 10f;

	public void Damage(float damage, Vector2 damageLocation) {
		health -= damage;
		if (health <= 0) {
			Destroy(gameObject);
		}
	}
}
