using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour {
    // Start is called before the first frame update
    Controls input;

    void Start() {
        input = new Controls();
        input.Enable();
    }

    // Update is called once per frame
    void Update() {
        Debug.Log(input.Player.Move.ReadValue<Vector2>().x);
    }
}
