using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using UnityEngine.Events;

public class BlocksManager : MonoBehaviour, IDamageable {
	public string damageLayer { get; protected set; } = "Unfriendly";
	public bool CanBeDamaged(string damageLayer) => DamageLayers.damageLayers[damageLayer][this.damageLayer];

	public Grid grid;
	public Tilemap blocksTilemap;
	public Tilemap breakTexturesTilemap;

	public Dictionary<Vector2Int, float> dynamicTileData = new Dictionary<Vector2Int, float>();

	public void Start() {
		grid = GetComponent<Grid>();
	}

	public List<TileBase> GetTilesOfType<T>() where T : TileBase {
		TileBase[] allTiles = blocksTilemap.GetTilesBlock(blocksTilemap.cellBounds);

		List<TileBase> matches = (from tile in allTiles
								  where tile is T
								  select tile).ToList();

		return matches;
	}

	public void Damage(float damage, Vector2 damageLocation) {
		Vector3Int position = grid.WorldToCell(new Vector3(damageLocation.x, damageLocation.y, 0));
		TileBase damaged = blocksTilemap.GetTile(position);
		if (damaged is BlockTile t) {
			Vector2Int pos = new Vector2Int(position.x, position.y);
			if (!dynamicTileData.ContainsKey(pos)) {
				dynamicTileData.Add(pos, t.maxHealth);
			}
			dynamicTileData[pos] -= damage;
			if (dynamicTileData[pos] <= 0) {
				Player.instance.GiveCoins(t.score);
				dynamicTileData.Remove(pos);
				SetTile(position, null);
				breakTexturesTilemap.SetTile(position, null);
			} else {
				int n = t.damageTiles.Length;
				int i = Mathf.FloorToInt((1 - (dynamicTileData[pos] / t.maxHealth)) * n);
				i = Mathf.Clamp(i, 0, n);
				breakTexturesTilemap.SetTile(position, t.damageTiles[i]);
			}
		}
	}

	public UnityEventTileBaseVector3Int onTileModified = new UnityEventTileBaseVector3Int();

	public void SetTile(Vector3Int pos, TileBase newTile) {
		TileBase oldTile = blocksTilemap.GetTile(pos);
		blocksTilemap.SetTile(pos, newTile);
		onTileModified.Invoke(oldTile, pos);
	}
}

[System.Serializable] // original, new (at location)
public class UnityEventTileBaseVector3Int : UnityEvent<TileBase, Vector3Int> { }
