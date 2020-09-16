using Eccentric;
using UnityEngine;
using UnityEngine.InputSystem;
namespace CJStudio.Splash {
    class Player : MonoBehaviour {
        public EPlayerState State { get; private set; } = EPlayerState.HUMANOID;
        public PlayerInputAction Input { get; private set; } = null;
        [SerializeField] Humanoid humanoid = null;
        [SerializeField] Squid squid = null;
        void Awake ( ) {
            humanoid?.Init (this);
            squid?.Init (this);
            Input = new PlayerInputAction ( );
        }

        void Start ( ) => ToggleState (EPlayerState.HUMANOID, humanoid.transform);

        public EPlayerState ToggleState (EPlayerState newState, Transform transform) {
            bool bSquid = newState == EPlayerState.SQUID;
            squid.gameObject.SetActive (bSquid);
            humanoid.gameObject.SetActive (!bSquid);
            State = newState;
            DomainEvents.Raise (new OnStateChanged (State, transform));
            return State;
        }
        void OnTransformStarted (InputAction.CallbackContext c) {
            ToggleState (EPlayerState.SQUID, humanoid.transform);
        }

        void OnTranformCanceled (InputAction.CallbackContext c) {
            ToggleState (EPlayerState.HUMANOID, squid.transform);
        }

        void OnEnable ( ) {
            Input.GamePlay.Transform.started += OnTransformStarted;
            Input.GamePlay.Transform.canceled += OnTranformCanceled;
            Input.GamePlay.Enable ( );
        }

        void OnDisable ( ) {
            Input.GamePlay.Transform.started -= OnTransformStarted;
            Input.GamePlay.Transform.canceled -= OnTranformCanceled;
            Input.GamePlay.Disable ( );
        }
    }

    class Component {
        protected Transform parent = null;
        protected PlayerComponent player = null;
        public virtual void Tick ( ) { }
        public Component (PlayerComponent player) {
            this.parent = player.transform;
            this.player = player;
        }
    }

    enum EPlayerState {
        HUMANOID,
        SQUID,
    }

    class OnStateChanged : IDomainEvent {
        public EPlayerState State { get; private set; } = EPlayerState.HUMANOID;
        public Transform Transform { get; private set; } = null;
        public OnStateChanged (EPlayerState state, Transform transform) {
            State = state;
            Transform = transform;
        }
    }
}