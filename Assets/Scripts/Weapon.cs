using System.Collections.Generic;
using Eccentric.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CJStudio.Splash {
    class Weapon : MonoBehaviour {
        Player parent = null;
        protected Transform Crosshair => parent.Crosshair;
        protected Player Parent => parent;
        [SerializeField] protected Texture2D splashTex = null;
        [SerializeField][ColorUsage (true, true)] protected Color32 splashColor = new Color32 ( );
        [SerializeField] protected int damage = 0;
        [SerializeField] protected float fireRate = 0f;
        [SerializeField] protected GameObject bulletPrefab = null;
        [SerializeField] protected Transform shootPoint = null;
        protected ScaledTimer timer = null;
        protected Camera cam = null;
        protected Color32[ ] splashTexColors = null;
        protected PlayerInputAction input = null;
        public Texture2D SplashTex => splashTex;
        public Color32 SplashColor => splashColor;
        public Color32[ ] SplashTexColors => splashTexColors;

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
        protected void InitSplashTex ( ) {
            splashTexColors = splashTex.GetPixels32 ( );
        }
        public void Init (Player player) {
            this.parent = player;
        }
    }
}