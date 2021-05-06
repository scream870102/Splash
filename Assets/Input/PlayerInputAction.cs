// GENERATED AUTOMATICALLY FROM 'Assets/Input/PlayerInputAction.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace CJStudio.Splash
{
    public class @PlayerInputAction : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @PlayerInputAction()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputAction"",
    ""maps"": [
        {
            ""name"": ""GamePlay"",
            ""id"": ""b55d0027-fac0-4f06-8daa-7cec17aaf110"",
            ""actions"": [
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""b99465c1-fb18-4595-a1e8-9b99b8afff4b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""f219c6ff-29d7-47ab-b2a4-7d36aeebfc53"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""Value"",
                    ""id"": ""0910fe58-d411-4d46-8b69-d4d873c71fe9"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Transform"",
                    ""type"": ""Button"",
                    ""id"": ""b5842cc1-7a02-4cb3-879c-2f06c3bc5498"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Reset"",
                    ""type"": ""Button"",
                    ""id"": ""962c9ee4-ad9a-4457-924d-391c74310bd8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""af41135d-1ee4-4b92-9b34-fadf7b8fe82a"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""81d25129-5886-40fb-bb41-271d9666cb4b"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a1c175af-09a6-49b4-9ded-41adc1c60a26"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b6edfd06-97b7-42a5-b247-9e2fa2e8d80e"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e3fc13e3-61da-4ee8-b88c-596a8327d45f"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d425cb2b-73b9-413f-83d6-c5ec29b8c45d"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Transform"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d408681b-e51d-45f0-8cb6-19bfe70cb9aa"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Transform"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""54ae74d2-8f34-4ddb-b12e-8680ea8eeb41"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Reset"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // GamePlay
            m_GamePlay = asset.FindActionMap("GamePlay", throwIfNotFound: true);
            m_GamePlay_Shoot = m_GamePlay.FindAction("Shoot", throwIfNotFound: true);
            m_GamePlay_Move = m_GamePlay.FindAction("Move", throwIfNotFound: true);
            m_GamePlay_Aim = m_GamePlay.FindAction("Aim", throwIfNotFound: true);
            m_GamePlay_Transform = m_GamePlay.FindAction("Transform", throwIfNotFound: true);
            m_GamePlay_Reset = m_GamePlay.FindAction("Reset", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }

        // GamePlay
        private readonly InputActionMap m_GamePlay;
        private IGamePlayActions m_GamePlayActionsCallbackInterface;
        private readonly InputAction m_GamePlay_Shoot;
        private readonly InputAction m_GamePlay_Move;
        private readonly InputAction m_GamePlay_Aim;
        private readonly InputAction m_GamePlay_Transform;
        private readonly InputAction m_GamePlay_Reset;
        public struct GamePlayActions
        {
            private @PlayerInputAction m_Wrapper;
            public GamePlayActions(@PlayerInputAction wrapper) { m_Wrapper = wrapper; }
            public InputAction @Shoot => m_Wrapper.m_GamePlay_Shoot;
            public InputAction @Move => m_Wrapper.m_GamePlay_Move;
            public InputAction @Aim => m_Wrapper.m_GamePlay_Aim;
            public InputAction @Transform => m_Wrapper.m_GamePlay_Transform;
            public InputAction @Reset => m_Wrapper.m_GamePlay_Reset;
            public InputActionMap Get() { return m_Wrapper.m_GamePlay; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(GamePlayActions set) { return set.Get(); }
            public void SetCallbacks(IGamePlayActions instance)
            {
                if (m_Wrapper.m_GamePlayActionsCallbackInterface != null)
                {
                    @Shoot.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnShoot;
                    @Shoot.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnShoot;
                    @Shoot.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnShoot;
                    @Move.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnMove;
                    @Move.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnMove;
                    @Move.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnMove;
                    @Aim.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnAim;
                    @Aim.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnAim;
                    @Aim.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnAim;
                    @Transform.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnTransform;
                    @Transform.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnTransform;
                    @Transform.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnTransform;
                    @Reset.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnReset;
                    @Reset.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnReset;
                    @Reset.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnReset;
                }
                m_Wrapper.m_GamePlayActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Shoot.started += instance.OnShoot;
                    @Shoot.performed += instance.OnShoot;
                    @Shoot.canceled += instance.OnShoot;
                    @Move.started += instance.OnMove;
                    @Move.performed += instance.OnMove;
                    @Move.canceled += instance.OnMove;
                    @Aim.started += instance.OnAim;
                    @Aim.performed += instance.OnAim;
                    @Aim.canceled += instance.OnAim;
                    @Transform.started += instance.OnTransform;
                    @Transform.performed += instance.OnTransform;
                    @Transform.canceled += instance.OnTransform;
                    @Reset.started += instance.OnReset;
                    @Reset.performed += instance.OnReset;
                    @Reset.canceled += instance.OnReset;
                }
            }
        }
        public GamePlayActions @GamePlay => new GamePlayActions(this);
        public interface IGamePlayActions
        {
            void OnShoot(InputAction.CallbackContext context);
            void OnMove(InputAction.CallbackContext context);
            void OnAim(InputAction.CallbackContext context);
            void OnTransform(InputAction.CallbackContext context);
            void OnReset(InputAction.CallbackContext context);
        }
    }
}
