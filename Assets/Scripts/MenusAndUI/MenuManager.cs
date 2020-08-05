using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour {
	public List<Menu> menus = new List<Menu>();

	public Stack<Menu> menuStack = new Stack<Menu>();

	Controls input;

	public void Awake() {
		input = new Controls();
		input.Enable();
	}

	void Update() {
		if (input.UI.Cancel.triggered) {
			CloseCurrentMenu();
		}

		if (Keyboard.current[Key.O].wasPressedThisFrame) {
			OpenMenu(menus[0]);
		}
	}

	public void OpenMenu(Menu m) {
		m.Open();
		menuStack.Push(m);
	}

	public void CloseCurrentMenu() {
		if (menuStack.Count != 0) {
			Menu m = menuStack.Peek();
			if (m.Close()) {
				menuStack.Pop();
			}
		}
	}
}
