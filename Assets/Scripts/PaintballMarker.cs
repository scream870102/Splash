using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
namespace CJStudio.Splash {
    class PaintballMarker : Weapon {
        [SerializeField] SphereAttr attr = null;
        [SerializeField] float forceAmount = 10f;
        public float ForceAmount => forceAmount;
        override protected void OnShootStarted (InputAction.CallbackContext c) {
            if (Timer.IsFinished) {
                Timer.Reset ( );
                GameObject o = Instantiate (BulletPrefab);
                Paintball pb = o.GetComponent<Paintball> ( );
                RaycastHit viewPortHit;
                Vector3 viewPortPoint = Vector3.zero;
                if (Physics.Raycast (Cam.ViewportPointToRay (new Vector3 (.5f, .5f, 0f)), out viewPortHit)) {
                    viewPortPoint = viewPortHit.point - ShootPoint.position;
                    Debug.Log (viewPortHit.collider.name);
                }
                SphereAttr passAttr = new SphereAttr (ShootPoint.position,
                    viewPortPoint.normalized,
                    attr.Dis,
                    attr.Radius);
                pb.Fire<SphereAttr> (passAttr, this);
            }
        }
    }
}