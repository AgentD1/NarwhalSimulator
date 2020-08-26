using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BlockTile : Tile {
	public string blockName;
	public float maxHealth;
	public int score;
	public Tile[] damageTiles;

#if UNITY_EDITOR
	[MenuItem("Assets/Create/BlockTile")]
	public static void CreateBlockTile() {
		string path = EditorUtility.SaveFilePanelInProject("Save Block Tile", "New Block Tile", "Asset", "Save Block Tile", "Assets");
		if (string.IsNullOrEmpty(path)) {
			return;
		}
		AssetDatabase.CreateAsset(CreateInstance<BlockTile>(), path);
	}
#endif
}
