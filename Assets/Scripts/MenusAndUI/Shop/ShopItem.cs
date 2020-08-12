using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopItem", menuName = "ShopItem")]
public class ShopItem : ScriptableObject {
	public enum ItemType {
		Body,
		Flukes,
		Flippers,
		Tusk,
		Other
	}

	public string itemName;
	public string description;
	public Sprite image;
	public GameObject prefabToCreate;
	public int price;
	public ItemType type;
}
