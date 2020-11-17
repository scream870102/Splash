using Cinemachine;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
namespace CJStudio.Splash {
    class Humanoid : Character {
        [SerializeField] HumanoidMovementAttribute movementAttribute = null;
        [SerializeField] HumanoidAimAttribute aimAttribute = null;
        [SerializeField] Transform crosshair = null;
        HumanoidMovement movement = null;
        HumanoidAim aim = null;
        Weapon weapon = null;
        public Camera Camera { get; private set; } = null;
        public CharacterController CharacterController { get; private set; } = null;
        public Animator Animator { get; private set; } = null;
        public PlayerInputAction Input { get; private set; } = null;
        public EHumanoidState State { get; private set; } = EHumanoidState.MOVE;
        public Transform Crosshair => crosshair;
        public CinemachineFreeLook FreeCam => Player.HumanoidCam;

        void Awake ( ) {
            this.Camera = Camera.main;
            this.CharacterController = GetComponent<CharacterController> ( );
            this.Animator = GetComponent<Animator> ( );
            this.Input = new PlayerInputAction ( );
            this.weapon = GetComponentInChildren<Weapon> ( );
            weapon?.Init (this);
        }

        void Start ( ) {
            movement = new HumanoidMovement (movementAttribute, this);
            aim = new HumanoidAim (aimAttribute, this);
        }

        void Update ( ) {
            if (Player.State == EPlayerState.HUMANOID) {
                movement.Tick ( );
                aim.Tick ( );
            }
        }

        override protected void OnEnable ( ) {
            base.OnEnable ( );
            Input.GamePlay.Enable ( );
            weapon.enabled = true;
        }

        override protected void OnDisable ( ) {
            base.OnDisable ( );
            Input.GamePlay.Disable ( );
            weapon.enabled = false;
        }

        async public UniTask LookAtCrosshairAsync ( ) {
            await aim.LookAtCrosshairAsync ( );
        }
        public void LookAtCrosshair ( ) {
            aim.LookAtCrosshair ( );
        }

        /// <summary>
        /// Call this method to change player state
        /// </summary>
        /// <param name="state">State to change</param>
        public void ChangeState (EHumanoidState state) {
            this.State = state;
        }
        override protected void HandleStateChanged (OnStateChanged e) {
            if (e.State != EPlayerState.HUMANOID)
                return;
            transform.localPosition = e.Transform.localPosition;
            //transform.rotation = Quaternion.Euler (0f, 180f + e.Transform.rotation.eulerAngles.y, 0f);
            transform.rotation = Quaternion.Euler (0f, e.Transform.rotation.eulerAngles.y, 0f);
        }
    }

    class HumanoidMovement : Component {
        HumanoidMovementAttribute attr = null;
        Animator anim = null;
        Camera cam = null;
        CharacterController characterController = null;
        Vector2 moveDir = Vector2.zero;
        PlayerInputAction input = null;
        new Humanoid character = null;
        public HumanoidMovement (HumanoidMovementAttribute attribute, Humanoid character) : base (character) {
            attr = attribute;
            cam = character.Camera;
            input = character.Input;
            characterController = character.CharacterController;
            anim = character.Animator;
            this.character = character;

            input.GamePlay.Move.performed += OnMovePerformed;
            input.GamePlay.Move.canceled += OnMoveCanceled;
        }

        ~HumanoidMovement ( ) {
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
            if (character.State != EHumanoidState.SHOOT_START) {
                parent.rotation = Quaternion.Slerp (parent.rotation, Quaternion.LookRotation (move), Time.deltaTime * attr.RotationSpeed);
                character.ChangeState (EHumanoidState.MOVE);
            }
            characterController.Move (move * attr.Speed * Time.deltaTime);
            anim.SetFloat ("Velocity", moveDir.magnitude);
        }
    }

    class HumanoidAim : Component {
        new Humanoid character = null;
        HumanoidAimAttribute attr = null;
        PlayerInputAction input = null;
        Cinemachine.CinemachineFreeLook freeCam = null;
        public HumanoidAim (HumanoidAimAttribute attribute, Humanoid character) : base (character) {
            input = character.Input;
            freeCam = character.FreeCam;
            input.GamePlay.Aim.performed += OnAimPerformed;
            input.GamePlay.Aim.canceled += OnAimCanceled;
            attr = attribute;
            this.character = character;
        }

        ~HumanoidAim ( ) {
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
            Vector3 lookAt = (character.Crosshair.position - parent.position).normalized;
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
            Vector3 lookAt = character.Crosshair.position;
            lookAt.y = 0f;
            parent.LookAt (lookAt);
        }
    }

    [System.Serializable]
    class HumanoidMovementAttribute {
        [SerializeField] float speed = 3f;
        [SerializeField] float rotationSpeed = 5f;
        public float Speed => speed;
        public float RotationSpeed => rotationSpeed;
    }

    [System.Serializable]
    class HumanoidAimAttribute {
        [SerializeField] float rotationSpeed = 20f;
        public float RotationSpeed => rotationSpeed;
    }

    [System.Serializable]
    enum EHumanoidState {
        MOVE,
        SHOOT_START,
        SHOOT_END,
        TRANSFORM,
    }
}