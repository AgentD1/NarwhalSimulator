using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable {
	string DamageLayer { get; }

	void Damage(float damage);
	bool CanBeDamaged(string damageLayer);
}
