using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlockType {
#pragma warning disable CA2235 // Mark all non-serializable fields
	public Sprite sprite;

	public string name;
	public float maxHealth;

	public UnityEngine.Tilemaps.Tile[] damageTiles;
#pragma warning restore CA2235 // Mark all non-serializable fields
}
