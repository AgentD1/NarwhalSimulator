using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;

public class Player : MonoBehaviour {
	public static Player instance;

	Controls input;
	public Rigidbody2D rb { get; protected set; }

	public string damageLayer = "Friendly";

	/*public float speed = 3f;
	public float acceleration = 1f;
	public float torque = 3f;
	float linearDrag;*/

	public float damage = 0.1f;

	public int coins { get; protected set; } = 0;

	public List<PlayerPart> parts = new List<PlayerPart>();
	public List<ShopItem> defaultPlayerItems = new List<ShopItem>();
	public List<ShopItem> ownedItems = new List<ShopItem>();

	public GameObject newTusk;

	public void Awake() {
		if (instance != null) {
			Debug.LogError("There are more than 1 player!");
		}
		instance = this;
		rb = GetComponent<Rigidbody2D>();
	}

	public void Start() {
		input = new Controls();
		input.Enable();
		foreach (ShopItem item in defaultPlayerItems) {
			AddPart(item);
		}
		foreach (ShopItem item in defaultPlayerItems) {
			ownedItems.Add(item);
		}
		//linearDrag = rb.drag;
		ResetCoins();
	}

	/// <summary>
	/// Instantiate the requested Player Part GameObject
	/// </summary>
	/// <param name="partGameObject">The part's game object</param>
	public void AddPart(GameObject partGameObject) {
		Vector3 newPos = partGameObject.transform.position;
		Quaternion newRot = partGameObject.transform.rotation;
		GameObject go = Instantiate(partGameObject, transform);
		go.name = partGameObject.name;
		go.transform.localPosition = newPos;
		go.transform.localRotation = newRot;

		PlayerPart part = go.GetComponent<PlayerPart>();

		parts.Add(part);

		part.Initialize(this);
	}

	public void AddPart(ShopItem partObject) {
		GameObject partGameObject = partObject.prefabToCreate;
		Vector3 newPos = partGameObject.transform.position;
		Quaternion newRot = partGameObject.transform.rotation;
		GameObject go = Instantiate(partGameObject, transform);
		go.name = partGameObject.name;
		go.transform.localPosition = newPos;
		go.transform.localRotation = newRot;

		PlayerPart part = go.GetComponent<PlayerPart>();

		parts.Add(part);

		part.Initialize(this);
	}

	public void RemovePart(GameObject partGameObject) {
		parts.Remove(partGameObject.GetComponent<PlayerPart>());
		Destroy(partGameObject);
	}

	public void RemovePart(PlayerPart part) {
		parts.Remove(part);
		Destroy(part.gameObject);
	}

	public void RemovePart(ShopItem part) {
		PlayerPart myGo = FindObjectsOfType<PlayerPart>().First((x) => { return x.gameObject.name == part.itemName; });
		parts.Remove(myGo);
		Destroy(myGo.gameObject);
	}

	public PlayerPart GetPartOfType(string type) {
		return parts.FirstOrDefault((p) => { return p.item.type.ToString() == type; });
	}

	public PlayerPart GetPartOfType(ShopItem itemWithType) {
		return parts.FirstOrDefault((p) => { return p.item.type == itemWithType.type; });
	}

	public PlayerPart GetPartOfType(ShopItem.ItemType type) {
		return parts.FirstOrDefault((p) => { return p.item.type == type; });
	}

	public PlayerPart GetPartOfSameType(GameObject go) {
		PlayerPart goPart = go.GetComponent<PlayerPart>();
		if (goPart != null) {
			return GetPartOfType(goPart.item);
		} else {
			return null;
		}
	}

	public void ReplaceOrAddPart(GameObject go) {
		PlayerPart replacePart = GetPartOfSameType(go);
		if (replacePart != null) {
			RemovePart(replacePart);
		}
		AddPart(go);
	}

	public UnityEvent partAdded { get; protected set; } = new UnityEvent();

	/*public void Update() {
		/*if (UnityEngine.InputSystem.Keyboard.current[UnityEngine.InputSystem.Key.Space].wasPressedThisFrame) {
			ReplaceOrAddPart(newTusk);
		} * / //bruhhhh i have to put a space here :(
	}*/

	public UnityEvent coinsChanged { get; protected set; } = new UnityEvent();
	public UnityEvent coinsGained { get; protected set; } = new UnityEvent();
	public UnityEvent coinsLost { get; protected set; } = new UnityEvent();
	public UnityEvent coinsReset { get; protected set; } = new UnityEvent();


	public void GiveCoins(int coins) {
		if (coins <= 0) {
			Debug.LogWarning("Can't subtract coins with GiveCoins " + coins);
			return;
		}
		this.coins += coins;
		coinsChanged.Invoke();
		coinsGained.Invoke();
	}

	public void RemoveCoins(int coins) {
		if (coins <= 0) {
			Debug.LogWarning("Can't add coins with GiveCoins " + coins);
			return;
		}
		this.coins -= coins;
		coinsChanged.Invoke();
		coinsLost.Invoke();
	}

	public void ChangeCoins(int coins) {
		if (coins == 0) {
			Debug.LogWarning("Can't change coins by 0");
		}
		this.coins += coins;
		if (coins < 0) {
			coinsLost.Invoke();
		} else {
			coinsGained.Invoke();
		}
		coinsChanged.Invoke();
	}

	public void ResetCoins() {
		coins = 0;
		coinsChanged.Invoke();
		coinsReset.Invoke();
	}
}
