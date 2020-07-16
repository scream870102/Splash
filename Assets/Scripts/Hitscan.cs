using System.Collections.Generic;
using UnityEngine;

namespace CJStudio.Splash {
    class Hitscan : Bullet {
        [SerializeField] Transform bullet;
        [SerializeField] MeshRenderer rend = null;
        override public void Fire<T> (T props, Weapon weapon) {
            RayAttr rayProps = props as RayAttr;

            rend.material.SetColor ("Bullet_Color", weapon.SplashColor);
            Vector3 scale = transform.localScale;
            scale.z = rayProps.Dis;
            transform.localScale = scale;
            transform.position = rayProps.Pos;
            transform.rotation = Quaternion.LookRotation (rayProps.Dir);
            Debug.DrawRay (rayProps.Pos, rayProps.Dir, Color.blue, 5f);
            RaycastHit hit;
            if (Physics.Raycast (rayProps.Pos, rayProps.Dir, out hit, rayProps.Dis)) {
                if (hit.collider.gameObject.tag != "Paintable")
                    return;
                PaintableObj obj = hit.collider.GetComponent<PaintableObj> ( );
                if (obj) {
                    obj.Paint (hit.textureCoord, weapon.SplashColor, weapon.SplashTex, weapon.SplashTexColors);
                }
            }
        }
    }
}