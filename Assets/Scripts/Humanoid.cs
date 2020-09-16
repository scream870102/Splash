using Cinemachine;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
namespace CJStudio.Splash {
    class Humanoid : PlayerComponent {
        [SerializeField] HumanoidMovementAttribute movementAttribute = null;
        [SerializeField] HumanoidAimAttribute aimAttribute = null;
        [SerializeField] Transform crosshair = null;
        [SerializeField] CinemachineFreeLook freeCam = null;
        HumanoidMovement movement = null;
        HumanoidAim aim = null;
        Weapon weapon = null;
        public Camera Camera { get; private set; } = null;
        public CharacterController CharacterController { get; private set; } = null;
        public Animator Animator { get; private set; } = null;
        public PlayerInputAction Input { get; private set; } = null;
        public EHumanoidState State { get; private set; } = EHumanoidState.MOVE;
        public Transform Crosshair => crosshair;
        public CinemachineFreeLook FreeCam => freeCam;

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
            movement.Tick ( );
            aim.Tick ( );
        }

        protected override void OnEnable ( ) {
            base.OnEnable ( );
            Input.GamePlay.Enable ( );
            weapon.enabled = true;
        }

        protected override void OnDisable ( ) {
            base.OnDisable ( );
            Input.GamePlay.Disable ( );
            weapon.enabled = false;
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
        public void ChangeState (EHumanoidState state) {
            this.State = state;
        }
        override protected void HandleStateChanged (OnStateChanged e) {
            if (e.State != EPlayerState.HUMANOID)
                return;
            transform.position = e.Transform.position;
            transform.rotation = Quaternion.Euler (0f, 180f+e.Transform.rotation.eulerAngles.y, 0f);
        }
    }

    class HumanoidMovement : Component {
        HumanoidMovementAttribute attr = null;
        Animator anim = null;
        Camera cam = null;
        CharacterController characterController = null;
        Vector2 moveDir = Vector2.zero;
        PlayerInputAction input = null;
        new Humanoid player = null;
        public HumanoidMovement (HumanoidMovementAttribute attribute, Humanoid player) : base (player) {
            attr = attribute;
            cam = player.Camera;
            input = player.Input;
            characterController = player.CharacterController;
            anim = player.Animator;
            this.player = player;

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
            if (player.State != EHumanoidState.SHOOT_START) {
                parent.rotation = Quaternion.Slerp (parent.rotation, Quaternion.LookRotation (move), Time.deltaTime * attr.RotationSpeed);
                player.ChangeState (EHumanoidState.MOVE);
            }

            characterController.Move (move * attr.Speed * Time.deltaTime);
            anim.SetFloat ("Velocity", moveDir.magnitude);
        }
    }

    class HumanoidAim : Component {
        new Humanoid player = null;
        HumanoidAimAttribute attr = null;
        PlayerInputAction input = null;
        Cinemachine.CinemachineFreeLook freeCam = null;
        public HumanoidAim (HumanoidAimAttribute attribute, Humanoid player) : base (player) {
            input = player.Input;
            freeCam = player.FreeCam;
            input.GamePlay.Aim.performed += OnAimPerformed;
            input.GamePlay.Aim.canceled += OnAimCanceled;
            attr = attribute;
            this.player = player;
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