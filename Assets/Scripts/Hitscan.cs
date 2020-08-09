using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;
namespace CJStudio.Splash {
    /// <summary>
    /// Hitscan will show trajectory before player release shoot button
    /// You can call UpdateTrajectory to update color and position 
    /// </summary>
    [RequireComponent (typeof (MeshRenderer))]
    class Hitscan : Bullet {
        [SerializeField] MeshRenderer rend = null;
        /// <summary>
        /// Overwrite this method to define how bullet detect PaintableObj
        /// </summary>
        /// <param name="props">props include everything bullet need</param>
        /// <param name="weapon">which weapon does this bullet belongs to</param>
        /// <typeparam name="T">type of props inherited from HitAttr</typeparam>
        override public void Fire<T> (T props, Weapon weapon) {
            this.Weapon = weapon;
            RayAttr rayProps = props as RayAttr;
            UpdateTrajectory (rayProps, weapon);
            RaycastHit hit;
            if (Physics.Raycast (rayProps.Pos, rayProps.Dir, out hit, rayProps.Dis)) {
                if (hit.collider.gameObject.tag != "Paintable")
                    return;
                PaintableObj obj = hit.collider.GetComponent<PaintableObj> ( );
                if (obj) {
                    int index = Weapon.GetRandomSplashTexIndex ( );
                    obj.Paint (hit.textureCoord, Weapon.SplashColor, Weapon.SplashTex[index], Weapon.SplashTexColors[index]);
                }
            }
            LeanPool.Despawn (this.gameObject);
        }

        /// <summary>
        /// Call this method to update trajectory color and postion
        /// </summary>
        /// <param name="props">Props for trajectory</param>
        /// <param name="weapon">Which weapon should this bullet belongs to</param>
        public void UpdateTrajectory (RayAttr props, Weapon weapon) {
            this.Weapon = weapon;
            Vector3 scale = transform.localScale;
            scale.z = props.Dis;
            transform.localScale = scale;
            transform.position = props.Pos;
            transform.rotation = Quaternion.LookRotation (props.Dir);
            rend.material.SetColor ("Bullet_Color", Weapon.SplashColor);
            rend.material.SetFloat ("Intensity", Weapon.Intensity);
        }
    }
}