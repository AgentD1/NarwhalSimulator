using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Creature), typeof(AvoidCreatureTypesBehaviour), typeof(Animator))]
public class LazyFishAnimator : MonoBehaviour {
	Creature myCreature;
	Animator myAnimator;
	AvoidCreatureTypesBehaviour avoidBehaviour;

	public float scaredSpeed = 5f;
	public float notScaredSpeed = 1f;
	public float scaredRotationSpeed = 1f;
	public float notScaredRotationSpeed = 0.1f;

	public float scaredDistanceAsleep = 1f;
	public float scaredDistanceAwake = 5f;

	public void Start() {
		myCreature = GetComponent<Creature>();
		myAnimator = GetComponent<Animator>();
		avoidBehaviour = GetComponent<AvoidCreatureTypesBehaviour>();
	}

	public void FixedUpdate() {
		switch (myCreature.mostRecentDirectionPriority) {
			case WeightedDirection.WeightedDirectionType.Priority:
				myAnimator.SetBool("Alerted", true);
				myAnimator.SetBool("Asleep", false);
				myCreature.speed = scaredSpeed;
				myCreature.rotationSpeed = scaredRotationSpeed;
				avoidBehaviour.scaredDistance = scaredDistanceAwake;
				break;
			case WeightedDirection.WeightedDirectionType.Regular:
				myAnimator.SetBool("Asleep", true);
				myAnimator.SetBool("Alerted", false);
				avoidBehaviour.scaredDistance = scaredDistanceAsleep;
				break;
			case WeightedDirection.WeightedDirectionType.Fallback:
				myAnimator.SetBool("Asleep", false);
				myAnimator.SetBool("Alerted", false);
				myCreature.speed = notScaredSpeed;
				myCreature.rotationSpeed = notScaredRotationSpeed;
				avoidBehaviour.scaredDistance = scaredDistanceAwake;
				break;
		}
	}
}
