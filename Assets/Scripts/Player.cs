using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CJStudio.Splash {
    class Player : MonoBehaviour {
        Animator anim = null;
        Vector2 moveDir = Vector2.zero;
        Camera cam = null;
        PlayerInputAction input = null;
        CharacterController characterController = null;
        [SerializeField] Texture2D splashTex = null;
        Color32[ ] splashTexColor = null;
        [SerializeField] Color splashColor = Color.clear;
        [SerializeField] float speed = 0f;
        [SerializeField] float rotationSpeed = 0f;
        void Awake ( ) {
            cam = Camera.main;
            input = new PlayerInputAction ( );
            characterController = GetComponent<CharacterController> ( );
            anim = GetComponent<Animator> ( );
        }

        void Start ( ) {
            InitSplashTex ( );
        }
        void Update ( ) {
            if (moveDir.magnitude >= .1f)
                Move ( );
        }

        void OnEnable ( ) {
            input.GamePlay.Shoot.started += OnShootStarted;
            input.GamePlay.Move.performed += OnMovePerformed;
            input.GamePlay.Move.canceled += OnMoveCanceled;
            input.GamePlay.Enable ( );
        }

        void OnDisable ( ) {
            input.GamePlay.Shoot.started -= OnShootStarted;
            input.GamePlay.Move.performed -= OnMovePerformed;
            input.GamePlay.Move.canceled -= OnMoveCanceled;
            input.GamePlay.Disable ( );
        }

        void OnShootStarted (InputAction.CallbackContext c) {
            RaycastHit hit;
            if (Physics.Raycast (cam.ViewportPointToRay (Vector3.one * .5f), out hit)) {
                if (hit.collider.gameObject.tag != "Paintable")
                    return;
                PaintableObj obj = hit.collider.gameObject.GetComponent<PaintableObj> ( );
                if (obj) {
                    obj.Paint (hit.textureCoord, splashColor, splashTex, splashTexColor);
                }
            }
        }

        void OnMovePerformed (InputAction.CallbackContext c) {
            moveDir = c.ReadValue<Vector2> ( );
        }

        void OnMoveCanceled (InputAction.CallbackContext c) {
            anim.SetFloat ("Velocity", 0f);
            moveDir = Vector2.zero;
        }

        void Move ( ) {
            Vector3 forward = cam.transform.forward;
            Vector3 right = cam.transform.right;
            forward.y = 0f;
            right.y = 0f;
            forward.Normalize ( );
            right.Normalize ( );
            Vector3 move = forward * moveDir.y + right * moveDir.x;
            transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (move), Time.deltaTime * rotationSpeed);
            characterController.Move (move * speed * Time.deltaTime);
            float moveVel = moveDir.magnitude;
            anim.SetFloat ("Velocity", moveVel);
        }

        void InitSplashTex ( ) {
            splashTexColor = splashTex.GetPixels32 ( );
        }
    }
}