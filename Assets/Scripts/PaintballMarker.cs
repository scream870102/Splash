using Lean.Pool;
using UnityEngine;
using UnityEngine.InputSystem;
namespace CJStudio.Splash {
    class PaintballMarker : Weapon {
        [SerializeField] SphereAttr attr = null;
        [SerializeField] float forceAmount = 10f;
        bool bShootPerformed = false;
        public float ForceAmount => forceAmount;
        void Update ( ) {
            if (bShootPerformed && Timer.IsFinished) {
                Parent.LookAtCrosshair ( );
                Timer.Reset ( );
                GameObject o = LeanPool.Spawn (BulletPrefab);
                Paintball pb = o.GetComponent<Paintball> ( );
                SphereAttr passAttr = new SphereAttr (ShootPoint.position, attr.Length, (Crosshair.position - ShootPoint.position).normalized);
                pb.Fire<SphereAttr> (passAttr, this);
            }
        }

        override protected async void HandleShootStarted (InputAction.CallbackContext c) {
            if (Timer.IsFinished && !bShootPerformed && Parent.State != EHumanoidState.SHOOT_START) {
                Parent.ChangeState (EHumanoidState.SHOOT_START);
                await Parent.LookAtCrosshairAsync ( );
                bShootPerformed = true;
            }
        }

        override protected void HandleShootCanceled (InputAction.CallbackContext c) {
            if (bShootPerformed) {
                bShootPerformed = false;
                Parent.ChangeState (EHumanoidState.SHOOT_END);
            }
        }
    }
}