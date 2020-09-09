using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMoveBehaviour : MonoBehaviour {
	public Vector2 currentDirection;
	[Range(0f, 1f)]
	public float randomChangeChance;
	public WeightedDirection.WeightedDirectionType priority;
	public float weight;

	Creature me;

	public void CalculateWeightedDirections() {
		if (me == null) {
			me = GetComponent<Creature>();
		}

		if (currentDirection == null || currentDirection == Vector2.zero || Random.Range(0f, 1f) < randomChangeChance) {
			currentDirection = Random.insideUnitCircle;
		}

		me.weightedDirections.Add(new WeightedDirection() { direction = currentDirection, weight = weight, priority = priority });
	}
}
