using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;
using System;

public class Player : MonoBehaviour, IDamageable {
	public static Player instance;

	Controls input;
	public Rigidbody2D rb { get; protected set; }

	public float damage = 0.1f;

	public List<PlayerPart> parts = new List<PlayerPart>();
	public List<ShopItem> defaultPlayerItems = new List<ShopItem>();
	public List<ShopItem> ownedItems = new List<ShopItem>();

	public void Awake() {
		if (instance != null) {
			Debug.LogError("There are more than 1 player!");
		}
		instance = this;
		rb = GetComponent<Rigidbody2D>();

		Creature.creaturesByType.Add("player", new List<Transform>() { transform });
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
		RefreshMaxHealth();
		health = maxHealth;
		Damage(0); // Refresh health bar
	}

	public void Update() {
		if (UnityEngine.InputSystem.Keyboard.current.f2Key.wasPressedThisFrame) {
			string fileName = DateTime.Now.ToString();
			fileName = fileName.Replace("/", "-").Replace(" ", "_").Replace(":", "-");
			string path = Application.persistentDataPath + System.IO.Path.AltDirectorySeparatorChar + "Screenshots" + System.IO.Path.AltDirectorySeparatorChar + "NarwhalSimulator" + fileName + ".png";
			ScreenCapture.CaptureScreenshot(path);
			Debug.Log("Screenshot taken and saved as " + path);
		}
		// Damage(0.01f, Vector2.zero);
	}

	#region Parts

	/// <summary>
	/// Instantiate the requested Player Part GameObject
	/// </summary>
	/// <param name="partGameObject">The part's game object</param>
	/*public void AddPart(GameObject partGameObject) {
		Vector3 newPos = partGameObject.transform.position;
		Quaternion newRot = partGameObject.transform.rotation;
		GameObject go = Instantiate(partGameObject, transform);
		go.name = partGameObject.name;
		go.transform.localPosition = newPos;
		go.transform.localRotation = newRot;

		PlayerPart part = go.GetComponent<PlayerPart>();

		parts.Add(part);

		part.Initialize(this);
	}*/

	public UnityEvent partAdded { get; protected set; } = new UnityEvent();

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

		part.Initialize(this, partObject);

		partAdded.Invoke();
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

	/*public void ReplaceOrAddPart(GameObject go) {
		PlayerPart replacePart = GetPartOfSameType(go);
		if (replacePart != null) {
			RemovePart(replacePart);
		}
		AddPart(go);
	}*/

	public void ReplaceOrAddPart(ShopItem go) {
		PlayerPart replacePart = GetPartOfType(go);
		if (replacePart != null) {
			RemovePart(replacePart);
		}
		AddPart(go);
	}

	#endregion

	#region Coins

	public int coins { get; protected set; } = 0;

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

	#endregion

	#region Health

	public float health;
	public float maxHealth;

	public UnityEvent healthChanged { get; protected set; } = new UnityEvent();

	public void RefreshMaxHealth() {
		maxHealth = GetPartOfType(ShopItem.ItemType.Body).GetComponent<BasicBody>().maxHealth;
	}

	public void Damage(float damage) {
		Damage(damage, Vector2.zero);
	}

	public void Damage(float damage, Vector2 damageLocation) {
		health = Mathf.Clamp(health - damage, 0, maxHealth);

		healthChanged.Invoke();
	}

	public string damageLayer = "Friendly";
	string IDamageable.damageLayer => damageLayer;

	public bool CanBeDamaged(string damageLayer) => DamageLayers.damageLayers[damageLayer][this.damageLayer];

	#endregion
}
