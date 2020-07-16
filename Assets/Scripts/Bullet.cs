using System.Collections.Generic;
using UnityEngine;

// Maybe we should have a collider to keep check if the bullet hit something
// We now have mop,sniper and paintball marker
// - sniper use raycast 
// - mop use boxcast
// - paintball marker use sphere marker

namespace CJStudio.Splash {
    class Bullet : MonoBehaviour {
        protected Weapon weapon = null;
        virtual protected void FixedUpdate ( ) {

        }

        /// <summary>
        /// Overwrite this method to define how bullet detect PaintableObj
        /// </summary>
        /// <param name="props">props include everything bullet need</param>
        /// <param name="weapon">which weapon does this bullet belongs to</param>
        /// <typeparam name="T">type of props inherited from HitAttr</typeparam>
        virtual public void Fire<T> (T props, Weapon weapon) where T : HitAttr {

        }

    }

    [System.Serializable]
    class HitAttr { }

    [System.Serializable]
    class RayAttr : HitAttr {
        public RayAttr (Vector3 pos, Vector3 dir, float dis = Mathf.Infinity) {
            this.pos = pos;
            this.dir = dir;
            this.dis = dis;
        }
        Vector3 pos;
        Vector3 dir;
        [SerializeField] float dis;
        public Vector3 Pos => pos;
        public Vector3 Dir => dir;
        public float Dis => dis;
    }

    [System.Serializable]
    class SphereAttr : HitAttr {
        public SphereAttr (Vector3 pos, Vector3 dir, float dis, float radius) {
            this.pos = pos;
            this.dir = dir;
            this.dis = dis;
            this.radius = radius;
        }
        Vector3 pos;
        Vector3 dir;
        [SerializeField] float dis;
        [SerializeField] float radius;
        public Vector3 Pos => pos;
        public Vector3 Dir => dir;
        public float Dis => dis;
        public float Radius => radius;

    }

    [System.Serializable]
    class BoxAttr : HitAttr {
        public BoxAttr (Vector3 center, Vector3 halfExtents, Vector3 dir, Quaternion orientation, float dis) {
            this.center = center;
            this.halfExtents = halfExtents;
            this.dir = dir;
            this.orientation = orientation;
            this.dis = dis;
        }
        Vector3 center;
        Vector3 halfExtents;
        Vector3 dir;
        Quaternion orientation;
        float dis;
        public Vector3 Center => center;
        public Vector3 HalfExtents => halfExtents;
        public Vector3 Dir => dir;
        public Quaternion Orientation => orientation;
        public float Dis => dis;
    }

}