using System.Collections.Generic;
using Eccentric.Utils;
using UnityEngine;
// Maybe we should have a collider to keep check if the bullet hit something
// We now have mop,sniper and paintball marker
// - sniper use raycast 
// - mop use boxcast
// - paintball marker use sphere marker

namespace CJStudio.Splash {
    class Bullet : MonoBehaviour {
        protected Weapon Weapon { get; set; } = null;
        /// <summary>
        /// Overwrite this method to define how bullet detect PaintableObj
        /// </summary>
        /// <param name="props">props include everything bullet need</param>
        /// <param name="weapon">which weapon does this bullet belongs to</param>
        /// <typeparam name="T">type of props inherited from HitAttr</typeparam>
        virtual public void Fire<T> (T props, Weapon weapon) where T : HitAttr { }

    }

    abstract class HitAttr { }

    [System.Serializable]
    class RayAttr : HitAttr {
        public RayAttr (Vector3 pos, Vector3 dir, float dis = Mathf.Infinity) {
            this.Pos = pos;
            this.Dir = dir;
            this.dis = dis;
        }

        [SerializeField] float dis = 0f;
        public float Dis => dis;
        public Vector3 Pos { get; private set; } = Vector3.zero;
        public Vector3 Dir { get; private set; } = Vector3.zero;
    }

    [System.Serializable]
    class SphereAttr : HitAttr {
        public SphereAttr (Vector3 pos, float length, Vector3 dir) {
            this.Pos = pos;
            this.length = length;
            this.Dir = dir;
        }

        [SerializeField] float length = 0f;
        public float Length => length;
        public Vector3 Pos { get; private set; } = Vector3.zero;
        public Vector3 Dir { get; private set; } = Vector3.zero;

    }

    // [System.Serializable]
    // class BoxAttr : HitAttr {
    //     public BoxAttr (Vector3 center, Vector3 halfExtents, Vector3 dir, Quaternion orientation, float dis) {
    //         this.center = center;
    //         this.halfExtents = halfExtents;
    //         this.dir = dir;
    //         this.orientation = orientation;
    //         this.dis = dis;
    //     }
    //     Vector3 center;
    //     Vector3 halfExtents;
    //     Vector3 dir;
    //     Quaternion orientation;
    //     float dis;
    //     public Vector3 Center => center;
    //     public Vector3 HalfExtents => halfExtents;
    //     public Vector3 Dir => dir;
    //     public Quaternion Orientation => orientation;
    //     public float Dis => dis;
    // }

}