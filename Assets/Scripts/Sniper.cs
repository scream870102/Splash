using UnityEngine;
using UnityEngine.InputSystem;

namespace CJStudio.Splash {
    class Sniper : Weapon {
        [SerializeField] RayAttr attr = null;
        
        override protected void OnShootStarted (InputAction.CallbackContext c) {
            if (timer.IsFinished) {
                timer.Reset ( );
                GameObject o = Instantiate (bulletPrefab);
                Hitscan hs = o.GetComponent<Hitscan> ( );
                RaycastHit viewPortHit;
                Vector3 viewPortPoint = Vector3.zero;
                if (Physics.Raycast (cam.ViewportPointToRay (new Vector3 (.5f, .5f, 0f)), out viewPortHit))
                    viewPortPoint = viewPortHit.point;
                RayAttr passAttr = new RayAttr (shootPoint.position,
                    (viewPortPoint - shootPoint.position).normalized, attr.Dis);
                hs.Fire<RayAttr> (passAttr, this);
            }
        }

    }
}