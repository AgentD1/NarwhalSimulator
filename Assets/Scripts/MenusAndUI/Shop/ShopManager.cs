using System;
using System.Linq;
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

	Dictionary<ShopItem, GameObject> itemGameObjects = new Dictionary<ShopItem, GameObject>();

	public Button closeButton;

	public string ascendingCharacter;
	public string descendingCharacter;
	public string neutralSortCharacter;

	[Header("Filter Buttons")]
	public Toggle ownedToggle;
	public Toggle unownedToggle;
	public Toggle filterByAffordableToggle;

	public TMP_InputField searchField;
	public Button searchButton;
	public Button clearSearchFieldButton;

	public Button nameSortingButton;
	public Button priceSortingButton;

	public TMP_Dropdown typeDropdown;

	bool showOwned = true;
	bool showUnowned = true;
	bool filterByAffordable = false;
	string searchTerm = "";
	string typeFilter = "All";

	Button currentSorting;
	bool sortAscending = false;

	void Start() {
		currentSorting = nameSortingButton;
		ownedToggle.onValueChanged.AddListener((x) => { OnOwnedToggleChanged(x); });
		unownedToggle.onValueChanged.AddListener((x) => { OnUnownedToggleChanged(x); });
		filterByAffordableToggle.onValueChanged.AddListener((x) => { OnFilterByAffordableToggle(x); });
		searchButton.onClick.AddListener(OnSearchButtonPressed);
		clearSearchFieldButton.onClick.AddListener(OnSearchClearPressed);

		nameSortingButton.onClick.AddListener(() => { OnSortingButtonPressed(nameSortingButton); });
		priceSortingButton.onClick.AddListener(() => { OnSortingButtonPressed(priceSortingButton); });

		typeDropdown.ClearOptions();
		//List<string> temp = new List<string>().Cast<List<string>>());
		typeDropdown.AddOptions(new List<string>() { "All" });
		typeDropdown.AddOptions(Enum.GetValues(typeof(ShopItem.ItemType)).Cast<ShopItem.ItemType>().Select((x) => { return x.ToString(); }).ToList());
		//Debug.Log(Enum.GetValues(typeof(ShopItem.ItemType)).OfType<string>().ToList());
		typeDropdown.onValueChanged.AddListener((x) => { OnTypeDropdownChanged(x); });

		foreach (ShopItem i in shopItems) {
			AddShopElement(i);
		}
		FilterItems();
	}

	/*void OnDisable() {

	}*/

	void AddShopElement(ShopItem item) {
		GameObject myGo = Instantiate(shopElementPrefab, shopElementPrefabLocation.transform);
		myGo.transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text = item.name;
		itemGameObjects.Add(item, myGo);
		UpdateShopElement(item);
	}

	void EnableShopElement(ShopItem item) {
		itemGameObjects[item].SetActive(true);
	}

	void DisableShopElement(ShopItem item) {
		itemGameObjects[item].SetActive(false);
	}

	void UpdateShopElement(ShopItem item) {
		Transform tf = itemGameObjects[item].transform;
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
					c.GetComponent<TextMeshProUGUI>().text = item.description; //+ "\n\n\n\n\n";
					break;
				case "Purchase":
					if (Player.instance.ownedItems.Contains(item)) {
						if (Player.instance.parts.Find((x) => { return x.name == item.prefabToCreate.name; })) { // equipped?
							c.GetComponent<Button>().onClick.RemoveAllListeners();
							c.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Equipped";
						} else {
							c.GetComponent<Button>().onClick.RemoveAllListeners();
							c.GetComponent<Button>().onClick.AddListener(() => EquipClicked(item));
							c.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Equip";
						}
					} else {
						c.GetComponent<Button>().onClick.RemoveAllListeners();
						c.GetComponent<Button>().onClick.AddListener(() => PurchaseClicked(item));
						c.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Purchase";
					}
					break;
				case "ImageBackground":
					c.GetChild(0).GetComponent<Image>().sprite = item.image;
					break;
			}
		}
	}

	public void EquipClicked(ShopItem item) {
		if (Player.instance.coins >= item.price) {
			Player.instance.ReplaceOrAddPart(item.prefabToCreate);
			UpdateShopElement(item);
		}
	}

	public void PurchaseClicked(ShopItem item) {
		if (Player.instance.coins >= item.price) {
			Player.instance.RemoveCoins(item.price);
			Player.instance.ownedItems.Add(item);
			Player.instance.ReplaceOrAddPart(item.prefabToCreate);
			UpdateShopElement(item);
		}
	}

	public void OnOwnedToggleChanged(bool on) {
		showOwned = on;
		FilterItems();
	}

	public void OnUnownedToggleChanged(bool on) {
		showUnowned = on;
		FilterItems();
	}

	public void OnFilterByAffordableToggle(bool on) {
		filterByAffordable = on;
		FilterItems();
	}

	public void OnSearchButtonPressed() {
		if (string.IsNullOrEmpty(searchField.text)) {
			return;
		}
		searchTerm = searchField.text;
		clearSearchFieldButton.interactable = true;
		FilterItems();
	}

	public void OnSearchClearPressed() {
		searchTerm = "";
		searchField.text = "";
		clearSearchFieldButton.interactable = false;
		FilterItems();
	}

	public void OnTypeDropdownChanged(int x) {
		typeFilter = typeDropdown.options[x].text;
		FilterItems();
	}

	public void OnSortingButtonPressed(Button b) {
		if (b == currentSorting) {
			TextMeshProUGUI oldText = currentSorting.transform.GetComponentInChildren<TextMeshProUGUI>();
			oldText.text = oldText.text.Substring(0, oldText.text.Length - (sortAscending ? ascendingCharacter : descendingCharacter).Length);
			sortAscending = !sortAscending;
			oldText.text += sortAscending ? ascendingCharacter : descendingCharacter;
		} else {
			TextMeshProUGUI oldText = currentSorting.transform.GetComponentInChildren<TextMeshProUGUI>();
			oldText.text = oldText.text.Substring(0, oldText.text.Length - (sortAscending ? ascendingCharacter : descendingCharacter).Length);
			oldText.text += neutralSortCharacter;
			currentSorting = b;
			oldText = currentSorting.transform.GetComponentInChildren<TextMeshProUGUI>();
			oldText.text = oldText.text.Substring(0, oldText.text.Length - neutralSortCharacter.Length);
			oldText.text += sortAscending ? ascendingCharacter : descendingCharacter;
		}
		FilterItems();
	}

	public void FilterItems() {
		foreach (ShopItem i in shopItems) {
			DisableShopElement(i);
		}

		if (!showOwned && !showUnowned) {
			return;
		}

		List<ShopItem> il = new List<ShopItem>();
		shopItems.ForEach((x) => {
			bool owned = Player.instance.ownedItems.Contains(x);
			if ((owned && !showOwned) || (!owned && !showUnowned)) {
				return;
			}
			if (filterByAffordable && x.price > Player.instance.coins) {
				return;
			}
			if (!string.IsNullOrEmpty(searchTerm) && !x.name.ToLower().Contains(searchTerm)) {
				return;
			}
			if (typeFilter != "All" && typeFilter != x.type.ToString()) {
				return;
			}
			il.Add(x);
		});

		if (currentSorting == nameSortingButton) {
			il.Sort((x, y) => { return x.itemName.CompareTo(y.itemName); });
		} else if (currentSorting == priceSortingButton) {
			il.Sort((x, y) => { return x.price.CompareTo(y.price); });
		}

		if (!sortAscending) {
			il.Reverse();
		}

		for (int i = 0; i < il.Count; i++) {
			EnableShopElement(il[i]);
			itemGameObjects[il[i]].transform.SetSiblingIndex(i);
		}
	}
}
