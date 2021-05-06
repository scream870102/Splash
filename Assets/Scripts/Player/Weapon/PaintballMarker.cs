using Lean.Pool;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
namespace CJStudio.Splash
{
    class PaintballMarker : Weapon
    {
        [SerializeField] SphereAttr attr = null;
        [SerializeField] float forceAmount = 10f;
        bool bShootPerformed = false;
        public float ForceAmount => forceAmount;
        void Update()
        {
            if (bShootPerformed && Timer.IsFinished)
            {
                Parent.LookAtCrosshair();
                Timer.Reset();
                GameObject o = LeanPool.Spawn(BulletPrefab);
                Paintball pb = o.GetComponent<Paintball>();
                SphereAttr passAttr = new SphereAttr(ShootPoint.position, attr.Length, (Crosshair.position - ShootPoint.position).normalized);
                pb.Fire<SphereAttr>(passAttr, this);
            }
        }

        override protected async void HandleShootPerformed(InputAction.CallbackContext c)
        {
            var buttonControl = c.control as ButtonControl;
            if (buttonControl.wasPressedThisFrame)
            {
                if (Timer.IsFinished && !bShootPerformed && Parent.State != EHumanoidState.SHOOT_START)
                {
                    Parent.ChangeState(EHumanoidState.SHOOT_START);
                    await Parent.LookAtCrosshairAsync();
                    bShootPerformed = true;
                }
            }
            if (buttonControl.wasReleasedThisFrame)
            {
                if (bShootPerformed)
                {
                    bShootPerformed = false;
                    Parent.ChangeState(EHumanoidState.SHOOT_END);
                }
            }

        }
    }
}