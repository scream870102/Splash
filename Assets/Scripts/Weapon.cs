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
        [SerializeField] float capacity = 100f;
        [SerializeField] float consumption = 0f;
        [SerializeField] GameObject bulletPrefab = null;
        [SerializeField] Transform shootPoint = null;
        PlayerInputAction input = null;
        protected float Capacity => capacity;
        protected Transform Crosshair => Parent.Crosshair;
        protected int Damage => damage;
        protected GameObject BulletPrefab => bulletPrefab;
        protected Transform ShootPoint => shootPoint;
        public float Intensity => intensity;
        public Texture2D[ ] SplashTex => splashTex;
        public Color32 SplashColor => splashColor;
        protected Player Parent { get; private set; } = null;
        protected ScaledTimer Timer { get; private set; } = null;
        protected Camera Cam { get; private set; } = null;
        public Dictionary<int, Color32[ ]> SplashTexColors { get; private set; } = null;

        virtual protected void Awake ( ) {
            input = new PlayerInputAction ( );
            Timer = new ScaledTimer (fireRate);
            Cam = Camera.main;
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
            SplashTexColors = new Dictionary<int, Color32[ ]> ( );
            for (int i = 0; i < splashTex.Length; i++)
                SplashTexColors.Add (i, splashTex[i].GetPixels32 ( ));
        }
        public void Init (Player player) {
            this.Parent = player;
        }

        public int GetRandomSplashTexIndex ( ) {
            return Random.Range (0, SplashTex.Length);
        }

        public float ConsumeInk ( ) {
            capacity -= consumption;
            if (capacity < 0f)
                capacity = 0f;
            return capacity;
        }
    }
}