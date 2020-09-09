using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightedDirection {
	public Vector2 direction;
	public float weight;
	public WeightedDirectionType priority;

	public enum WeightedDirectionType { // Directions of a lower type are completely ignored if an upper type is found
		Priority,
		Regular,
		Fallback
	}
}
