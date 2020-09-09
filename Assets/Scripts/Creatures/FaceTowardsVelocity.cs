using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceTowardsVelocity : MonoBehaviour {
	public Rigidbody2D rb;
	void Update() {
		transform.rotation = Quaternion.Euler(0, 0, Vector2.Angle(Vector2.right, rb.velocity) - 180);
	}
}
