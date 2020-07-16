using UnityEngine;
using UnityEngine.InputSystem;

namespace CJStudio.Splash {
    class Sniper : Weapon {
        override protected void OnShootStarted (InputAction.CallbackContext c) {
            Debug.Log ("ShootEnter");
        }
    }
}