using UnityEngine;
using UnityEngine.InputSystem;
namespace CJStudio.Splash {
    class Squid : PlayerComponent {
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
            movement.Tick ( );
        }

        override protected void HandleStateChanged (OnStateChanged e) {
            if (e.State != EPlayerState.SQUID)
                return;
            transform.position = e.Transform.position;
            Vector3 rot = transform.rotation.eulerAngles;
            rot.y = 180f + e.Transform.rotation.eulerAngles.y;
            rot.x = -90f;
            transform.rotation = Quaternion.Euler (rot);
        }
    }

    class SquidMovement : Component {
        new Squid player = null;
        SquidMovementAttribute attr = null;
        Animator anim = null;
        Camera cam = null;
        CharacterController characterController = null;
        Vector2 moveDir = Vector2.zero;
        PlayerInputAction input = null;
        public SquidMovement (SquidMovementAttribute attr, Squid player) : base (player) {
            this.player = player;
            this.attr = attr;
            cam = player.Camera;
            input = player.Input;
            characterController = player.CharacterController;
            anim = player.Animator;

            input.GamePlay.Move.performed += OnMovePerformed;
            input.GamePlay.Move.canceled += OnMoveCanceled;
        }

        ~SquidMovement ( ) {
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

    class SquidMovementAttribute {
        [SerializeField] float speed = 3f;
        [SerializeField] float rotationSpeed = 5f;
        public float Speed => speed;
        public float RotationSpeed => rotationSpeed;
    }

}