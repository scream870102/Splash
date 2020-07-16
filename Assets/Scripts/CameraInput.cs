using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
namespace CJStudio.Splash {
    class CameraInput : MonoBehaviour {
        CameraInputAction input = null;
        CinemachineFreeLook cam = null;
        void Awake ( ) {
            input = new CameraInputAction ( );
            cam = GetComponent<CinemachineFreeLook> ( );
        }
        
        void OnEnable ( ) {
            input.GamePlay.Aim.performed += OnAimPerformed;
            input.GamePlay.Aim.canceled += OnAimCanceled;
            input.GamePlay.Enable ( );
        }
        void OnDisable ( ) {
            input.GamePlay.Aim.performed -= OnAimPerformed;
            input.GamePlay.Aim.canceled -= OnAimCanceled;
            input.GamePlay.Disable ( );
        }

        void OnAimPerformed (InputAction.CallbackContext c) {
            Vector2 inputValue = c.ReadValue<Vector2> ( );
            cam.m_XAxis.m_InputAxisValue = inputValue.x;
            cam.m_YAxis.m_InputAxisValue = inputValue.y;
        }

        void OnAimCanceled (InputAction.CallbackContext c) {
            cam.m_XAxis.m_InputAxisValue = 0f;
            cam.m_YAxis.m_InputAxisValue = 0f;
        }

    }
}