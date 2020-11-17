using Cinemachine;
using Eccentric;
using UnityEngine;
using UnityEngine.InputSystem;
namespace CJStudio.Splash {
    class Player : MonoBehaviour {
        public EPlayerState State { get; private set; } = EPlayerState.HUMANOID;
        public PlayerInputAction Input { get; private set; } = null;
        [SerializeField] Humanoid humanoid = null;
        [SerializeField] Squid squid = null;
        [SerializeField] CinemachineFreeLook humanoidCam = null;
        [SerializeField] CinemachineFreeLook squidCam = null;
        public CinemachineFreeLook HumanoidCam => humanoidCam;
        public CinemachineFreeLook SquidCam => squidCam;
        void Awake ( ) {
            humanoid?.Init (this);
            squid?.Init (this);
            Input = new PlayerInputAction ( );
        }

        void Start ( ) => ToggleState (EPlayerState.HUMANOID, humanoid.transform);

        EPlayerState ToggleState (EPlayerState newState, Transform transform) {
            Debug.Log($"{newState} : {transform.position}");
            bool bSquid = newState == EPlayerState.SQUID;
            squid.gameObject.SetActive (bSquid);
            humanoid.gameObject.SetActive (!bSquid);
            humanoidCam.Priority = bSquid?10 : 11;
            squidCam.Priority = bSquid?11 : 10;
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
        protected Character character = null;
        public virtual void Tick ( ) { }
        public Component (Character character) {
            this.parent = character.transform;
            this.character = character;
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