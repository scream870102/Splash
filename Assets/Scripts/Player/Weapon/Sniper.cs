using Lean.Pool;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
namespace CJStudio.Splash
{
    class Sniper : Weapon
    {
        [SerializeField] RayAttr attr = null;
        bool bShootPerformed = false;
        // store current bullet refference
        Hitscan hs = null;

        void Update()
        {
            // if player keep pressing shoot button keep update trajectory and keep player face to crosshair
            if (bShootPerformed && hs)
            {
                Parent.LookAtCrosshair();
                RayAttr passAttr = new RayAttr(ShootPoint.position,
                    (Crosshair.position - ShootPoint.position).normalized, attr.Dis);
                hs.UpdateTrajectory(passAttr, this);
            }
        }

        override protected async void HandleShootPerformed(InputAction.CallbackContext c)
        {
            var buttonControll = c.control as ButtonControl;
            if (buttonControll.wasPressedThisFrame)
            {
                // if player can shoot face to Crosshair first and spawn trajectory and bullet 
                if (Timer.IsFinished && !bShootPerformed)
                {
                    Parent.ChangeState(EHumanoidState.SHOOT_START);
                    await Parent.LookAtCrosshairAsync();
                    hs = LeanPool.Spawn(BulletPrefab).GetComponent<Hitscan>();
                    RayAttr passAttr = new RayAttr(ShootPoint.position,
                        (Crosshair.position - ShootPoint.position).normalized, attr.Dis);
                    hs.UpdateTrajectory(passAttr, this);
                    bShootPerformed = true;
                }
            }
            if (buttonControll.wasReleasedThisFrame)
            {
                // if player released shoot button and have shot bullet before update the bullet
                if (bShootPerformed && Timer.IsFinished)
                {
                    bShootPerformed = false;
                    Timer.Reset();
                    Parent.LookAtCrosshair();
                    RayAttr passAttr = new RayAttr(ShootPoint.position,
                        (Crosshair.position - ShootPoint.position).normalized, attr.Dis);
                    hs.Fire<RayAttr>(passAttr, this);
                    Parent.ChangeState(EHumanoidState.SHOOT_END);
                    hs = null;
                }
            }

        }
    }
}