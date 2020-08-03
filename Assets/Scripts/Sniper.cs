using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
namespace CJStudio.Splash {
    class Sniper : Weapon {
        [SerializeField] RayAttr attr = null;
        bool bShootPerformed = false;
        // store current bullet refference
        Hitscan hs = null;

        void Update ( ) {
            // if player keep pressing shoot button keep update trajectory and keep player face to crosshair
            if (bShootPerformed && hs != null) {
                Parent.LookAtCrosshair ( );
                RayAttr passAttr = new RayAttr (ShootPoint.position,
                    (Crosshair.position - ShootPoint.position).normalized, attr.Dis);
                hs.UpdateTrajectory (passAttr, this);
            }
        }

        override protected async void OnShootStarted (InputAction.CallbackContext c) {
            // if player can shoot face to Crosshair first and spawn trajectory and bullet 
            if (Timer.IsFinished && !bShootPerformed) {
                Parent.ChangeState (EPlayerState.SHOOT_START);
                await Parent.LookAtCrosshairAsync ( );
                GameObject o = Instantiate (BulletPrefab);
                hs = o.GetComponent<Hitscan> ( );
                RayAttr passAttr = new RayAttr (ShootPoint.position,
                    (Crosshair.position - ShootPoint.position).normalized, attr.Dis);
                hs.UpdateTrajectory (passAttr, this);
                bShootPerformed = true;
            }
        }

        override protected void OnShootCanceled (InputAction.CallbackContext c) {
            // if player released shoot button and have shot bullet before update the bullet
            if (bShootPerformed && Timer.IsFinished) {
                bShootPerformed = false;
                Timer.Reset ( );
                Parent.LookAtCrosshair ( );
                RayAttr passAttr = new RayAttr (ShootPoint.position,
                    (Crosshair.position - ShootPoint.position).normalized, attr.Dis);
                hs.Fire<RayAttr> (passAttr, this);
                Parent.ChangeState (EPlayerState.SHOOT_END);
                hs = null;
            }
        }

    }
}