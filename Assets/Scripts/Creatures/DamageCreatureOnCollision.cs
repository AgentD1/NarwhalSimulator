using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ICreature))]
public class DamageCreatureOnCollision : MonoBehaviour {
	public bool damageConstantly = false;
	public bool obeyCreatureNoDamage = true;
	public float damage = 0.1f;

	ICreature myCreature;

	public void Awake() {
		myCreature = GetComponent<ICreature>();
	}

	public void OnCollisionEnter2D(Collision2D collision) {
		if (!damageConstantly) {
			DamageCreature(collision);
		}
	}

	public void OnCollisionStay2D(Collision2D collision) {
		if (damageConstantly) {
			DamageCreature(collision);
		}
	}

	void DamageCreature(Collision2D col) {
		if (obeyCreatureNoDamage && col.otherCollider.CompareTag("CreatureNoDamage")) {
			return;
		}

		ICreature creatureHit = col.gameObject.GetComponent<ICreature>();
		if (creatureHit == null) {
			creatureHit = col.gameObject.GetComponentInParent<ICreature>();
		}

		if (creatureHit != null && creatureHit.CanBeDamaged(myCreature.damageLayer)) {
			creatureHit.Damage(damage, col.contacts[0].point);
		}
	}
}
