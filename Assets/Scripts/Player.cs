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
        [SerializeField] float speed = 0f;
        [SerializeField] float rotationSpeed = 0f;
        void Awake ( ) {
            cam = Camera.main;
            input = new PlayerInputAction ( );
            characterController = GetComponent<CharacterController> ( );
            anim = GetComponent<Animator> ( );
        }

        void Update ( ) {
            if (moveDir.magnitude >= .1f)
                Move ( );
        }

        void OnEnable ( ) {
            input.GamePlay.Move.performed += OnMovePerformed;
            input.GamePlay.Move.canceled += OnMoveCanceled;
            input.GamePlay.Enable ( );
        }

        void OnDisable ( ) {
            input.GamePlay.Move.performed -= OnMovePerformed;
            input.GamePlay.Move.canceled -= OnMoveCanceled;
            input.GamePlay.Disable ( );
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
    }
}