using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidsBehaviour : MonoBehaviour {
	public List<string> creatureTypesToBoidWith = new List<string>();

	public WeightedDirection.WeightedDirectionType priority = WeightedDirection.WeightedDirectionType.Fallback;

	public float seperationWeight, alignmentWeight, cohesionWeight, seperationDist, neighbourhoodDist, neighbourhoodAngle;

	public Rigidbody2D rb;

	Creature me;

	public void CalculateWeightedDirections() {
		if (me == null) {
			me = GetComponent<Creature>();
		}

		Vector2 alignmentAverage = Vector2.zero;
		Vector2 positionAverage = Vector2.zero;
		int count = 0;

		foreach (string type in creatureTypesToBoidWith) {
			if (!Creature.creaturesByType.ContainsKey(type)) continue;
			foreach (Transform t in Creature.creaturesByType[type]) {
				float dist = Vector2.Distance(transform.position, t.position);
				float alignmentDifference = Vector2.SignedAngle(rb.velocity, t.GetComponent<Rigidbody2D>().velocity);
				if (dist <= neighbourhoodDist && Mathf.Abs(alignmentDifference) <= neighbourhoodAngle) {
					if (dist < seperationDist) {
						me.weightedDirections.Add(new WeightedDirection() {
							direction = transform.position - t.position,
							weight = seperationWeight,
							priority = priority
						});
					}

					alignmentAverage += t.GetComponent<Rigidbody2D>().velocity;
					positionAverage += (Vector2)t.position;
					count++;
				}
			}
		}

		if (count != 0) {
			alignmentAverage /= count;
			positionAverage /= count;

			me.weightedDirections.Add(new WeightedDirection() {
				direction = (Vector3)alignmentAverage - transform.position,
				weight = alignmentWeight,
				priority = priority
			});

			me.weightedDirections.Add(new WeightedDirection() {
				direction = (Vector3)positionAverage - transform.position,
				weight = cohesionWeight,
				priority = priority
			});
		}
	}
}
