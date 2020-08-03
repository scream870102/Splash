using System.Collections.Generic;
using UnityEngine;
namespace CJStudio.Splash {
    [RequireComponent (typeof (Rigidbody))]
    class Paintball : Bullet {
        [SerializeField] Rigidbody rb = null;
        SphereAttr attr = null;

        void FixedUpdate ( ) {
            // RaycastHit hit;
            // if (Physics.SphereCast (transform.position, attr.Radius, attr.Dir, out hit, attr.Dis)) {
            //     if (hit.collider.gameObject.tag != "Paintable")
            //         return;
            //     PaintableObj obj = hit.collider.GetComponent<PaintableObj> ( );
            //     if (obj) {
            //         obj.Paint (hit.textureCoord, weapon.SplashColor, weapon.SplashTex, weapon.SplashTexColors);
            //     }
            // }
        }

        override public void Fire<T> (T props, Weapon weapon) {
            attr = props as SphereAttr;
            transform.position = attr.Pos;
            rb.AddForce (attr.Dir * (weapon as PaintballMarker).ForceAmount, ForceMode.Impulse);

        }
    }
}