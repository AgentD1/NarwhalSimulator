using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable {
	string DamageLayer { get; }

	void Damage(float damage, Vector2 damageLocation);
	bool CanBeDamaged(string damageLayer);
}
