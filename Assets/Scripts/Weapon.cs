using System.Collections.Generic;
using Eccentric.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CJStudio.Splash {
    class Weapon : MonoBehaviour {
        [SerializeField] Texture2D[ ] splashTex = null;
        [SerializeField] Color32 splashColor = new Color32 ( );
        [SerializeField][Range (0f, 10f)] float intensity = 0f;
        [SerializeField] int damage = 0;
        [SerializeField] float fireRate = 0f;
        [SerializeField] GameObject bulletPrefab = null;
        [SerializeField] Transform shootPoint = null;
        Player parent = null;
        ScaledTimer timer = null;
        Camera cam = null;
        Dictionary<int, Color32[ ]> splashTexColors = null;
        PlayerInputAction input = null;
        protected Player Parent => parent;
        protected Transform Crosshair => parent.Crosshair;
        protected int Damage => damage;
        protected GameObject BulletPrefab => bulletPrefab;
        protected Transform ShootPoint => shootPoint;
        protected ScaledTimer Timer => timer;
        protected Camera Cam => cam;
        public Dictionary<int, Color32[ ]> SplashTexColors => splashTexColors;
        public float Intensity => intensity;
        public Texture2D[ ] SplashTex => splashTex;
        public Color32 SplashColor => splashColor;

        virtual protected void Awake ( ) {
            input = new PlayerInputAction ( );
            timer = new ScaledTimer (fireRate);
            cam = Camera.main;
        }

        virtual protected void Start ( ) {
            InitSplashTex ( );
        }

        virtual protected void OnEnable ( ) {
            input.GamePlay.Shoot.started += OnShootStarted;
            input.GamePlay.Shoot.performed += OnShootPerformed;
            input.GamePlay.Shoot.canceled += OnShootCanceled;
            input.GamePlay.Enable ( );
        }
        virtual protected void OnDisable ( ) {
            input.GamePlay.Shoot.started -= OnShootStarted;
            input.GamePlay.Shoot.performed -= OnShootPerformed;
            input.GamePlay.Shoot.canceled -= OnShootCanceled;
        }

        virtual protected void OnShootStarted (InputAction.CallbackContext c) { }
        virtual protected void OnShootPerformed (InputAction.CallbackContext c) { }
        virtual protected void OnShootCanceled (InputAction.CallbackContext c) { }

        void InitSplashTex ( ) {
            splashTexColors = new Dictionary<int, Color32[ ]> ( );
            for (int i = 0; i < splashTex.Length; i++)
                splashTexColors.Add (i, splashTex[i].GetPixels32 ( ));
        }
        public void Init (Player player) {
            this.parent = player;
        }

        public int GetRandomSplashTexIndex ( ) {
            return Random.Range (0, SplashTex.Length);
        }
    }
}