using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CJStudio.Splash {
    class Weapon : MonoBehaviour {
        [SerializeField] protected Player player = null;
        [SerializeField] protected Texture2D splashTex = null;
        [SerializeField] protected Color32 splashColor = new Color32 ( );
        [SerializeField] protected int damage = 0;
        [SerializeField] protected float fireRate = 0f;
        protected Color32[ ] splashTexColors = null;
        protected PlayerInputAction input = null;
        public Texture2D SplashTex => splashTex;
        public Color32 SplashColor => splashColor;
        public Color32[ ] SplashTexColors => splashTexColors;
        public int Damage => damage;
        virtual protected void Awake ( ) {
            input = new PlayerInputAction ( );
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
    }
}