using UnityEngine;
using UnityEngine.InputSystem;
namespace CJStudio.Splash {
    class Squid : Character {
        [SerializeField] SquidMovementAttribute movementAttribute = null;
        SquidMovement movement = null;
        public Camera Camera { get; private set; } = null;
        public CharacterController CharacterController { get; private set; } = null;
        public Animator Animator { get; private set; } = null;
        public PlayerInputAction Input { get; private set; } = null;
        void Awake ( ) {
            this.Camera = Camera.main;
            this.CharacterController = GetComponent<CharacterController> ( );
            this.Animator = GetComponent<Animator> ( );
            this.Input = new PlayerInputAction ( );
        }
        void Start ( ) {
            movement = new SquidMovement (movementAttribute, this);
        }

        void Update ( ) {
            if(Player.State==EPlayerState.SQUID)
                movement.Tick ( );
        }
        override protected void OnEnable ( ) {
            base.OnEnable ( );
            Input.GamePlay.Enable ( );
        }

        protected override void OnDisable ( ) {
            base.OnDisable ( );
            Input.GamePlay.Disable ( );
        }

        override protected void HandleStateChanged (OnStateChanged e) {
            if (e.State != EPlayerState.SQUID)
                return;
            transform.localPosition = e.Transform.localPosition;
            // Vector3 rot = transform.rotation.eulerAngles;
            // rot.y = 180f + e.Transform.rotation.eulerAngles.y;
            // rot.x = -90f;
            transform.rotation = Quaternion.Euler (0f, e.Transform.rotation.eulerAngles.y, 0f);
        }
    }

    class SquidMovement : Component {
        new Squid character = null;
        SquidMovementAttribute attr = null;
        Animator anim = null;
        Camera cam = null;
        CharacterController characterController = null;
        Vector2 moveDir = Vector2.zero;
        PlayerInputAction input = null;
        public SquidMovement (SquidMovementAttribute attr, Squid character) : base (character) {
            this.character = character;
            this.attr = attr;
            cam = character.Camera;
            input = character.Input;
            characterController = character.CharacterController;
            anim = character.Animator;

            input.GamePlay.Move.performed += HandleMovePerformed;
            input.GamePlay.Move.canceled += HandleMoveCanceled;

        }

        ~SquidMovement ( ) {
            input.GamePlay.Move.performed -= HandleMovePerformed;
            input.GamePlay.Move.canceled -= HandleMoveCanceled;
        }

        public override void Tick ( ) {
            if (moveDir.magnitude >= .1f)
                Move ( );
        }

        void HandleMovePerformed (InputAction.CallbackContext c) {
            moveDir = c.ReadValue<Vector2> ( );
        }

        void HandleMoveCanceled (InputAction.CallbackContext c) {
            anim?.SetFloat ("Velocity", 0f);
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

            parent.rotation = Quaternion.Slerp (parent.rotation, Quaternion.LookRotation (move), Time.deltaTime * attr.RotationSpeed);

            characterController.Move (move * attr.Speed * Time.deltaTime);
            anim?.SetFloat ("Velocity", moveDir.magnitude);
        }
    }

    [System.Serializable]
    class SquidMovementAttribute {
        [SerializeField] float speed = 3f;
        [SerializeField] float rotationSpeed = 5f;
        public float Speed => speed;
        public float RotationSpeed => rotationSpeed;
    }

}