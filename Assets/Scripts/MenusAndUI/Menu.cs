using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {
	public bool subMenu = false;

	/// <summary>
	/// Opens the menu
	/// </summary>
	public virtual void Open() {
		gameObject.SetActive(true);
	}

	/// <summary>
	/// Closes the menu. Returns true if it closed and false if it should stay open
	/// </summary>
	/// <returns>Whether the menu closed properly</returns>
	public virtual bool Close() {
		gameObject.SetActive(false);
		return true;
	}
}
