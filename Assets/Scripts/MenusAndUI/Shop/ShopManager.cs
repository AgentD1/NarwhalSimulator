using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour {
	public GameObject shopElementPrefab;
	public GameObject shopElementPrefabLocation;
	public List<ShopItem> shopItems;

	void Start() {
		foreach (ShopItem i in shopItems) {
			AddShopElement(i);
		}
	}

	void AddShopElement(ShopItem item) {
		Instantiate(shopElementPrefab, shopElementPrefabLocation.transform);
		UpdateShopElement(item);
	}

	void UpdateShopElement(ShopItem item) {
		int index = shopItems.IndexOf(item);
		Transform tf = shopElementPrefabLocation.transform.GetChild(index);
		foreach (Transform c in tf) { // Foreach child
			switch (c.name) {
				case "ItemName":
					c.GetComponent<TextMeshProUGUI>().text = item.itemName;
					break;
				case "ItemSubtext":
					c.GetComponent<TextMeshProUGUI>().text = Player.instance.ownedItems.Contains(item) ? "Owned | ItemRarity" : "Not Owned | ItemRarity";
					break;
				case "ItemCost":
					c.GetComponent<TextMeshProUGUI>().text = "₦: " + item.price;
					break;
				case "ItemDescription":
					c.GetComponent<TextMeshProUGUI>().text = item.description + "\n\n\n\n\n";
					Debug.Log(c.GetComponent<TextMeshProUGUI>().text);
					break;
				case "Purchase":
					if (Player.instance.ownedItems.Contains(item)) {
						if (Player.instance.parts.Find((x) => { return x.name == item.prefabToCreate.name; })) { // equipped?
							c.GetComponent<Button>().onClick.RemoveAllListeners();
							c.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Equipped";
						} else {
							c.GetComponent<Button>().onClick.RemoveAllListeners();
							c.GetComponent<Button>().onClick.AddListener(() => EquipClicked(item.itemName));
							c.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Equip";
						}
					} else {
						c.GetComponent<Button>().onClick.RemoveAllListeners();
						c.GetComponent<Button>().onClick.AddListener(() => PurchaseClicked(item.itemName));
						c.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Purchase";
					}
					break;
				case "ImageBackground":
					c.GetChild(0).GetComponent<Image>().sprite = item.image;
					break;
			}
		}
	}

	public void EquipClicked(string itemName) {
		ShopItem item = shopItems.Find((x) => { return x.itemName == itemName; });
		if (Player.instance.coins >= item.price) {
			Player.instance.ReplaceOrAddPart(item.prefabToCreate);
			UpdateShopElement(item);
		}
	}

	public void PurchaseClicked(string itemName) {
		ShopItem item = shopItems.Find((x) => { return x.itemName == itemName; });
		if (Player.instance.coins >= item.price) {
			Player.instance.RemoveCoins(item.price);
			Player.instance.ownedItems.Add(item);
			Player.instance.ReplaceOrAddPart(item.prefabToCreate);
			UpdateShopElement(item);
		}
	}
}
