using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseCreatureTypesBehaviour : MonoBehaviour {
	public WeightedDirection.WeightedDirectionType priority = WeightedDirection.WeightedDirectionType.Regular;
	public float maxWeight, minWeight;

	public List<string> creatureTypesToChase = new List<string>();

	public float chaseDistance;

	Creature me;

	public void CalculateWeightedDirections() {
		if (me == null) {
			me = GetComponent<Creature>();
		}

		float minDist = chaseDistance;
		Transform creatureToChase = null;

		foreach (string type in creatureTypesToChase) {
			if (!Creature.creaturesByType.ContainsKey(type)) continue;
			foreach (Transform c in Creature.creaturesByType[type]) {
				float dist = Vector2.Distance(c.position, transform.position);
				if (dist <= minDist) {
					minDist = dist;
					creatureToChase = c;
				}
			}
		}

		if (creatureToChase != null) {
			me.weightedDirections.Add(new WeightedDirection() {
				direction = creatureToChase.position - transform.position,
				weight = Mathf.Lerp(minWeight, maxWeight, minDist / chaseDistance),
				priority = priority
			});
		}
	}
}
