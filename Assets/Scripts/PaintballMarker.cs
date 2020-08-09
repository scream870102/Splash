using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Lean.Pool;
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
                GameObject o = LeanPool.Spawn(BulletPrefab);
                Paintball pb = o.GetComponent<Paintball> ( );
                SphereAttr passAttr = new SphereAttr (ShootPoint.position, attr.Length, (Crosshair.position - ShootPoint.position).normalized);
                pb.Fire<SphereAttr> (passAttr, this);
            }
        }

        override protected async void OnShootStarted (InputAction.CallbackContext c) {
            if (Timer.IsFinished && !bShootPerformed && Parent.State != EPlayerState.SHOOT_START) {
                Parent.ChangeState (EPlayerState.SHOOT_START);
                await Parent.LookAtCrosshairAsync ( );
                bShootPerformed = true;
            }
        }

        override protected void OnShootPerformed (InputAction.CallbackContext c) {

        }

        override protected void OnShootCanceled (InputAction.CallbackContext c) {
            bShootPerformed = false;
            Parent.ChangeState (EPlayerState.SHOOT_END);
        }
    }
}