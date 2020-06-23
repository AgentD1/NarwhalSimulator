using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerPart : MonoBehaviour {
	public Player player { get; protected set; }
	public string type { get; protected set; } = "NoTypeSet"; // Types: Tusk Flippers Body Flukes
	public virtual void Initialize(Player p) {
		player = p;
	}
}
