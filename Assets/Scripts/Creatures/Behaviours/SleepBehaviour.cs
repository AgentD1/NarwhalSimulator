using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Creature))]
public class SleepBehaviour : MonoBehaviour {
	public WeightedDirection.WeightedDirectionType priority;
	public float weight;

	public float sleepNeed = 0f;
	public float sleepNeedIncrease = 1f;
	public float sleepNeedDecrease = 1f;
	public float maxSleepNeed = 100f;
	public float maxSleepChance = 0.1f;

	Creature me;

	public bool sleeping = false;

	public void Start() {
		me = GetComponent<Creature>();
	}

	public void FixedUpdate() {
		if (me.mostRecentDirectionPriority == WeightedDirection.WeightedDirectionType.Priority) {
			sleeping = false;
		}
		if (sleeping) {
			sleepNeed = Mathf.Max(0, sleepNeed - sleepNeedDecrease * Time.fixedDeltaTime);
		} else {
			sleepNeed = Mathf.Min(sleepNeed + sleepNeedIncrease * me.speed * Time.fixedDeltaTime, maxSleepNeed);
		}
	}

	public void CalculateWeightedDirections() {
		if (!sleeping) {
			if (Random.Range(0, maxSleepNeed / maxSleepChance) < sleepNeed) {
				sleeping = true;
			}
		}
		if (sleeping) {
			if (sleepNeed <= 0) {
				sleeping = false;
				return;
			}
			me.weightedDirections.Add(new WeightedDirection() { direction = Vector2.zero, priority = priority, weight = weight });
		}
	}
}
