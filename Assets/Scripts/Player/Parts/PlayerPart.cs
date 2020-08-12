using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerPart : MonoBehaviour {
	public Player player { get; protected set; }
	public ShopItem item { get; protected set; }
	public virtual void Initialize(Player p, ShopItem i) {
		player = p;
		item = i;
	}
}
