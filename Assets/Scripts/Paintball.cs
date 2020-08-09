using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;
namespace CJStudio.Splash {
    [RequireComponent (typeof (Rigidbody))]
    class Paintball : Bullet {
        [SerializeField] Rigidbody rb = null;
        [SerializeField] SphereAttr attr = null;

        override public void Fire<T> (T props, Weapon weapon) {
            this.Weapon = weapon;
            attr = props as SphereAttr;
            transform.position = attr.Pos;
            rb.AddForce (attr.Dir * (weapon as PaintballMarker).ForceAmount, ForceMode.Impulse);

        }

        void OnCollisionStay (Collision other) {
            if (other.collider.tag != "Paintable")
                return;
            PaintableObj obj = other.gameObject.GetComponent<PaintableObj> ( );
            RaycastHit hit;
            if (Physics.Raycast (transform.position, (other.GetContact (0).point - transform.position).normalized, out hit, 1f)) {
                int index = Weapon.GetRandomSplashTexIndex ( );
                obj.Paint (hit.textureCoord, Weapon.SplashColor, Weapon.SplashTex[index], Weapon.SplashTexColors[index]);
                LeanPool.Despawn (this.gameObject);
            }
        }

        void OnDisable ( ) {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

    }
}