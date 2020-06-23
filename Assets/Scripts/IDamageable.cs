using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable {
	string damageLayer { get; }

	void Damage(float damage, Vector2 damageLocation);
	bool CanBeDamaged(string damageLayer);
}
