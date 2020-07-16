// GENERATED AUTOMATICALLY FROM 'Assets/Input/CameraInputAction.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace CJStudio.Splash
{
    public class @CameraInputAction : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @CameraInputAction()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""CameraInputAction"",
    ""maps"": [
        {
            ""name"": ""GamePlay"",
            ""id"": ""b45cb274-705e-4e08-afa5-b04dc96cc2fc"",
            ""actions"": [
                {
                    ""name"": ""Aim"",
                    ""type"": ""Value"",
                    ""id"": ""5ac6d7ff-8e25-4bf7-bed9-f9a8aac6cbed"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""face138f-fd58-4afd-835a-6e75181d10aa"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
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
            m_GamePlay_Aim = m_GamePlay.FindAction("Aim", throwIfNotFound: true);
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
        private readonly InputAction m_GamePlay_Aim;
        public struct GamePlayActions
        {
            private @CameraInputAction m_Wrapper;
            public GamePlayActions(@CameraInputAction wrapper) { m_Wrapper = wrapper; }
            public InputAction @Aim => m_Wrapper.m_GamePlay_Aim;
            public InputActionMap Get() { return m_Wrapper.m_GamePlay; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(GamePlayActions set) { return set.Get(); }
            public void SetCallbacks(IGamePlayActions instance)
            {
                if (m_Wrapper.m_GamePlayActionsCallbackInterface != null)
                {
                    @Aim.started -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnAim;
                    @Aim.performed -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnAim;
                    @Aim.canceled -= m_Wrapper.m_GamePlayActionsCallbackInterface.OnAim;
                }
                m_Wrapper.m_GamePlayActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Aim.started += instance.OnAim;
                    @Aim.performed += instance.OnAim;
                    @Aim.canceled += instance.OnAim;
                }
            }
        }
        public GamePlayActions @GamePlay => new GamePlayActions(this);
        public interface IGamePlayActions
        {
            void OnAim(InputAction.CallbackContext context);
        }
    }
}
