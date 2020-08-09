using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cinemachine;
using Cysharp.Threading.Tasks;
using Lean.Pool;
using UnityEngine;
using UnityEngine.InputSystem;
namespace CJStudio.Splash {
    class Player : MonoBehaviour {
        [SerializeField] MovementAttribute movementAttribute = null;
        [SerializeField] AimAttribute aimAttribute = null;
        [SerializeField] Transform crosshair = null;
        [SerializeField] CinemachineFreeLook freeCam = null;
        Movement movement = null;
        Aim aim = null;
        Weapon weapon = null;
        public Camera Camera { get; private set; }
        public CharacterController CharacterController { get; private set; }
        public Animator Animator { get; private set; }
        public PlayerInputAction Input { get; private set; }
        public Transform Crosshair => crosshair;
        public CinemachineFreeLook FreeCam => freeCam;
        public EPlayerState state = EPlayerState.MOVE;
        public EPlayerState State => state;

        void Awake ( ) {
            this.Camera = Camera.main;
            this.CharacterController = GetComponent<CharacterController> ( );
            this.Animator = GetComponent<Animator> ( );
            this.Input = new PlayerInputAction ( );
            this.weapon = GetComponentInChildren<Weapon> ( );
            if (!weapon)
                Debug.LogError ("Can't find the weapon on this player");
            else
                weapon.Init (this);
        }

        void Start ( ) {
            movement = new Movement (movementAttribute, this);
            aim = new Aim (aimAttribute, this);
        }

        void Update ( ) {
            movement.Tick ( );
            aim.Tick ( );
        }

        void OnEnable ( ) {
            Input.GamePlay.Enable ( );
        }

        void OnDisable ( ) {
            Input.GamePlay.Disable ( );
        }

        public async UniTask LookAtCrosshairAsync ( ) {
            await aim.LookAtCrosshairAsync ( );
        }
        public void LookAtCrosshair ( ) {
            aim.LookAtCrosshair ( );
        }

        /// <summary>
        /// Call this method to change player state
        /// </summary>
        /// <param name="state">State to change</param>
        public void ChangeState (EPlayerState state) {
            this.state = state;
        }
    }

    class Component {
        protected Transform parent = null;
        protected Player player = null;
        public virtual void Tick ( ) { }
        public Component (Player player) {
            this.parent = player.transform;
            this.player = player;
        }
    }
    class Movement : Component {
        MovementAttribute attr = null;
        Animator anim = null;
        Camera cam = null;
        CharacterController characterController = null;
        Vector2 moveDir = Vector2.zero;
        PlayerInputAction input = null;
        public Movement (MovementAttribute attribute, Player player) : base (player) {
            attr = attribute;
            cam = player.Camera;
            input = player.Input;
            characterController = player.CharacterController;
            anim = player.Animator;

            input.GamePlay.Move.performed += OnMovePerformed;
            input.GamePlay.Move.canceled += OnMoveCanceled;
        }

        ~Movement ( ) {
            input.GamePlay.Move.performed -= OnMovePerformed;
            input.GamePlay.Move.canceled -= OnMoveCanceled;
        }
        public override void Tick ( ) {
            if (moveDir.magnitude >= .1f)
                Move ( );
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

            // if player is shooting don't change rotation
            // cause player should face to the Crosshair not move direction
            if (player.State != EPlayerState.SHOOT_START) {
                parent.rotation = Quaternion.Slerp (parent.rotation, Quaternion.LookRotation (move), Time.deltaTime * attr.RotationSpeed);
                player.ChangeState (EPlayerState.MOVE);
            }

            characterController.Move (move * attr.Speed * Time.deltaTime);
            anim.SetFloat ("Velocity", moveDir.magnitude);
        }
    }

    class Aim : Component {
        AimAttribute attr = null;
        PlayerInputAction input = null;
        Cinemachine.CinemachineFreeLook freeCam = null;
        public Aim (AimAttribute attribute, Player player) : base (player) {
            input = player.Input;
            freeCam = player.FreeCam;
            input.GamePlay.Aim.performed += OnAimPerformed;
            input.GamePlay.Aim.canceled += OnAimCanceled;
            attr = attribute;
        }

        ~Aim ( ) {
            input.GamePlay.Aim.performed -= OnAimPerformed;
            input.GamePlay.Aim.canceled -= OnAimCanceled;
        }

        void OnAimPerformed (InputAction.CallbackContext c) {
            Vector2 inputValue = c.ReadValue<Vector2> ( );
            freeCam.m_XAxis.m_InputAxisValue = inputValue.x;
            freeCam.m_YAxis.m_InputAxisValue = inputValue.y;

        }

        void OnAimCanceled (InputAction.CallbackContext c) {
            freeCam.m_XAxis.m_InputAxisValue = 0f;
            freeCam.m_YAxis.m_InputAxisValue = 0f;
        }

        public async UniTask LookAtCrosshairAsync ( ) {
            Vector3 lookAt = (player.Crosshair.position - parent.position).normalized;
            lookAt.y = 0f;
            Quaternion lookRotation = Quaternion.LookRotation (lookAt);
            while (Quaternion.Angle (parent.rotation, lookRotation) > 0f) {
                parent.rotation = Quaternion.Slerp (
                    parent.rotation,
                    lookRotation,
                    Time.deltaTime * attr.RotationSpeed
                );
                await UniTask.DelayFrame (1);
            }
        }

        public void LookAtCrosshair ( ) {
            Vector3 lookAt = player.Crosshair.position;
            lookAt.y = 0f;
            parent.LookAt (lookAt);
        }
    }

    [System.Serializable]
    class MovementAttribute {
        [SerializeField] float speed = 3f;
        [SerializeField] float rotationSpeed = 5f;
        public float Speed => speed;
        public float RotationSpeed => rotationSpeed;
    }

    [System.Serializable]
    class AimAttribute {
        [SerializeField] float rotationSpeed = 20f;
        public float RotationSpeed => rotationSpeed;
    }

    [System.Serializable]
    enum EPlayerState {
        MOVE,
        SHOOT_START,
        SHOOT_END,
    }
}