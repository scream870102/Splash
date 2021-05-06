using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
namespace CJStudio.Splash
{
    class GameManager : MonoBehaviour
    {

        PlayerInputAction input = null;
        void Awake() => input = new PlayerInputAction();

        void OnResetPerformed(InputAction.CallbackContext ctx) => SceneManager.LoadScene(0);

        void OnEnable()
        {
            input.GamePlay.Reset.performed += OnResetPerformed;
            input.GamePlay.Enable();
        }

        void OnDisable()
        {
            input.GamePlay.Reset.performed -= OnResetPerformed;
            input.GamePlay.Disable();
        }
    }

}
