using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightCreatureBehaviour : MonoBehaviour {
	public Vector2 direction;
	public float weight;
	public WeightedDirection.WeightedDirectionType priority;

	Creature me;

	public void CalculateWeightedDirections() {
		if (me == null) {
			me = GetComponent<Creature>();
		}

		me.weightedDirections.Add(new WeightedDirection() { direction = direction, weight = weight, priority = priority });
	}
}
