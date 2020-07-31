using UnityEngine;
using UnityEngine.InputSystem;

namespace CJStudio.Splash {
    class Sniper : Weapon {
        [SerializeField] RayAttr attr = null;

        override protected void OnShootStarted (InputAction.CallbackContext c) {
            if (timer.IsFinished) {
                timer.Reset ( );
                Parent.LookAtCrosshair ( );
                GameObject o = Instantiate (bulletPrefab);
                Hitscan hs = o.GetComponent<Hitscan> ( );
                RayAttr passAttr = new RayAttr (shootPoint.position,
                    (Crosshair.position - shootPoint.position).normalized, attr.Dis);
                hs.Fire<RayAttr> (passAttr, this);
            }
        }

    }
}