using Eccentric;
using UnityEngine;
namespace CJStudio.Splash {
    class PlayerComponent : MonoBehaviour {
        public Player Player { get; private set; } = null;
        protected virtual void OnEnable ( ) {
            DomainEvents.Register<OnStateChanged> (HandleStateChanged);
        }

        protected virtual void OnDisable ( ) {
            DomainEvents.UnRegister<OnStateChanged> (HandleStateChanged);
        }

        public void Init (Player player) {
            this.Player = player;
        }

        protected virtual void HandleStateChanged (OnStateChanged e) { }
    }
}