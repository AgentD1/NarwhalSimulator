using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BlocksManager : MonoBehaviour, IDamageable {
	public string DamageLayer { get; protected set; } = "Unfriendly";
	public bool CanBeDamaged(string damageLayer) => DamageLayers.damageLayers[damageLayer][DamageLayer];

	public Grid grid;
	public Tilemap blocksTilemap;
	public Tilemap breakTexturesTilemap;

	public BlockType[] blockTypes;

	Dictionary<Sprite, BlockType> blockTypesDict = new Dictionary<Sprite, BlockType>();

	public Dictionary<Vector2Int, float> dynamicTileData = new Dictionary<Vector2Int, float>();

	public void Start() {
		grid = GetComponent<Grid>();
		foreach (BlockType b in blockTypes) {
			blockTypesDict.Add(b.sprite, b);
		}
	}

	public void Update() {

	}

	public void Damage(float damage, Vector2 damageLocation) {
		Vector3Int position = grid.WorldToCell(new Vector3(damageLocation.x, damageLocation.y, 0));
		TileBase damaged = blocksTilemap.GetTile(position);
		if (damaged is Tile t) {
			if (blockTypesDict.ContainsKey(t.sprite)) {
				Vector2Int pos = new Vector2Int(position.x, position.y);
				if (!dynamicTileData.ContainsKey(pos)) {
					dynamicTileData.Add(pos, blockTypesDict[t.sprite].maxHealth);
				}
				dynamicTileData[pos] -= damage;
				if (dynamicTileData[pos] <= 0) {
					dynamicTileData.Remove(pos);
					blocksTilemap.SetTile(position, null);
					breakTexturesTilemap.SetTile(position, null);
				} else {
					int n = blockTypesDict[t.sprite].damageTiles.Length;
					int i = Mathf.FloorToInt((1 - (dynamicTileData[pos] / blockTypesDict[t.sprite].maxHealth)) * n);
					i = Mathf.Clamp(i, 0, n);
					breakTexturesTilemap.SetTile(position, blockTypesDict[t.sprite].damageTiles[i]);
				}
			}
		}
	}
}
