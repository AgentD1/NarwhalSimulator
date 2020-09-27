using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Creature and the related classes are pretty heavily inspired by Quill18's video on AI. https://www.youtube.com/watch?v=bc86es4YOoc
public class Creature : MonoBehaviour, ICreature {
	public string creatureType = "None";
	public float speed = 3f;
	public float rotationSpeed = 0.1f;
	public bool needsToRotate = true;

	public static Dictionary<string, List<Transform>> creatureTransformsByType = new Dictionary<string, List<Transform>>();

	Rigidbody2D rb;

	public List<WeightedDirection> weightedDirections = new List<WeightedDirection>();

	public WeightedDirection.WeightedDirectionType mostRecentDirectionPriority = WeightedDirection.WeightedDirectionType.Fallback;

	public string damageLayer => "Unfriendly";

	string ICreature.creatureType => creatureType;

	public float health = 5f;

	public int score = 10;

	public void Awake() {
		rb = GetComponent<Rigidbody2D>();
		if (!creatureTransformsByType.ContainsKey(creatureType)) {
			creatureTransformsByType[creatureType] = new List<Transform>();
		}
		creatureTransformsByType[creatureType].Add(transform);
	}

	public void OnDestroy() {
		creatureTransformsByType[creatureType].Remove(transform);
	}

	Vector2 target = Vector2.zero;

	public void FixedUpdate() {
		weightedDirections.Clear();

		SendMessage("CalculateWeightedDirections", SendMessageOptions.DontRequireReceiver);

		Vector2 weightedDirection = Vector2.zero;
		float sum = 0;

		WeightedDirection.WeightedDirectionType highestPriority = WeightedDirection.WeightedDirectionType.Fallback;

		foreach (WeightedDirection d in weightedDirections) {
			if (d.priority < highestPriority) {
				highestPriority = d.priority;
				weightedDirection = Vector2.zero;
				sum = 0;
			}
			if (d.priority == highestPriority) {
				weightedDirection += d.direction * d.weight;
				sum += d.weight;
			}
		}

		mostRecentDirectionPriority = highestPriority;

		if (weightedDirection == Vector2.zero) {
			rb.velocity = Vector2.zero;
			return;
		}

		if (sum != 0) {
			weightedDirection /= sum;
			target = transform.position + (Vector3)weightedDirection;
			weightedDirection.Normalize();
		}

		if (!needsToRotate) {
			rb.velocity = weightedDirection * speed;
		} else {
			float currentDirection = Vector2.SignedAngle(Vector2.right, rb.velocity);
			float newDirection = Mathf.MoveTowardsAngle(currentDirection, Vector2.SignedAngle(Vector2.right, weightedDirection), rotationSpeed);
			this.currentDirection = currentDirection;
			this.newDirection = newDirection;
			rb.velocity = new Vector2(Mathf.Cos(newDirection * Mathf.Deg2Rad), Mathf.Sin(newDirection * Mathf.Deg2Rad)) * speed;
			rb.SetRotation(newDirection - 180);
		}
	}

	public void Damage(float damage, Vector2 damageLocation) {
		health -= damage;
		if (health <= 0) {
			Player.instance.GiveCoins(score);
			Destroy(gameObject);
		}
	}

	float newDirection = 0;
	float currentDirection = 0;

	public void OnDrawGizmosSelected() {
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(target, 0.5f);
		Gizmos.DrawLine(transform.position, transform.position + new Vector3(Mathf.Cos(newDirection * Mathf.Deg2Rad), Mathf.Sin(newDirection * Mathf.Deg2Rad), 0) * speed);
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(transform.position, transform.position + new Vector3(Mathf.Cos(currentDirection * Mathf.Deg2Rad), Mathf.Sin(currentDirection * Mathf.Deg2Rad), 0) * speed);
	}

	public bool CanBeDamaged(string damageLayer) => DamageLayers.damageLayers[damageLayer][this.damageLayer];
}
