using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidCreatureTypesBehaviour : MonoBehaviour {
	public WeightedDirection.WeightedDirectionType priority = WeightedDirection.WeightedDirectionType.Priority;
	public float maxWeight, minWeight;

	public List<string> creatureTypesToAvoid = new List<string>();

	public float scaredDistance;

	Creature me;

	public void CalculateWeightedDirections() {
		if (me == null) {
			me = GetComponent<Creature>();
		}

		foreach (string type in creatureTypesToAvoid) {
			if (!Creature.creaturesByType.ContainsKey(type)) continue;
			foreach (Transform c in Creature.creaturesByType[type]) {
				float dist = Vector2.Distance(c.position, transform.position);
				if (dist < scaredDistance) {
					me.weightedDirections.Add(new WeightedDirection() {
						direction = transform.position - c.position,
						weight = Mathf.Lerp(minWeight, maxWeight, dist / scaredDistance),
						priority = priority
					});
				}
			}
		}
	}
}
