using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {
	public bool subMenu = false;

	void Start() {

	}

	void Update() {

	}

	/// <summary>
	/// Opens the menu
	/// </summary>
	public void Open() {
		gameObject.SetActive(true);
	}

	/// <summary>
	/// Closes the menu. Returns true if it closed and false if it should stay open
	/// </summary>
	/// <returns>Whether the menu closed properly</returns>
	public bool Close() {
		gameObject.SetActive(false);
		return true;
	}
}
