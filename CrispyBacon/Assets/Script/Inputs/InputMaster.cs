// GENERATED AUTOMATICALLY FROM 'Assets/Script/Inputs/InputMaster.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @InputMaster : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputMaster()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputMaster"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""4324a46f-87a7-4ca1-8504-eb6f7406d1a2"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""829ebcf8-71ef-4532-a8f2-99f38ff3d463"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""StickDeadzone(max=1)"",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Charge"",
                    ""type"": ""Button"",
                    ""id"": ""6e2a6b49-b1a5-4cc7-bbf0-9cd635734a11"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                },
                {
                    ""name"": ""AirBreaks"",
                    ""type"": ""Value"",
                    ""id"": ""15f7683a-88d7-469f-871b-90b4d5e0ced8"",
                    ""expectedControlType"": ""Double"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""7bbaad84-ddba-44a8-a397-accc9819744b"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Charge"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a063f373-3b46-457e-a639-21e737744ecb"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""KeyBoardAndMouse"",
                    ""action"": ""Charge"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ba4f2d61-c5b1-40dd-a346-29fe6eafa00c"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""AirBreaks"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""85a94c9e-caa0-44c3-bccd-da508ff0e103"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""AirBreaks"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ed6f9c85-93b4-47d3-bb58-e6073020e023"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""KeyBoardAndMouse"",
            ""bindingGroup"": ""KeyBoardAndMouse"",
            ""devices"": []
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Movement = m_Player.FindAction("Movement", throwIfNotFound: true);
        m_Player_Charge = m_Player.FindAction("Charge", throwIfNotFound: true);
        m_Player_AirBreaks = m_Player.FindAction("AirBreaks", throwIfNotFound: true);
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

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_Movement;
    private readonly InputAction m_Player_Charge;
    private readonly InputAction m_Player_AirBreaks;
    public struct PlayerActions
    {
        private @InputMaster m_Wrapper;
        public PlayerActions(@InputMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Player_Movement;
        public InputAction @Charge => m_Wrapper.m_Player_Charge;
        public InputAction @AirBreaks => m_Wrapper.m_Player_AirBreaks;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Charge.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCharge;
                @Charge.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCharge;
                @Charge.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnCharge;
                @AirBreaks.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAirBreaks;
                @AirBreaks.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAirBreaks;
                @AirBreaks.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAirBreaks;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Charge.started += instance.OnCharge;
                @Charge.performed += instance.OnCharge;
                @Charge.canceled += instance.OnCharge;
                @AirBreaks.started += instance.OnAirBreaks;
                @AirBreaks.performed += instance.OnAirBreaks;
                @AirBreaks.canceled += instance.OnAirBreaks;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    private int m_KeyBoardAndMouseSchemeIndex = -1;
    public InputControlScheme KeyBoardAndMouseScheme
    {
        get
        {
            if (m_KeyBoardAndMouseSchemeIndex == -1) m_KeyBoardAndMouseSchemeIndex = asset.FindControlSchemeIndex("KeyBoardAndMouse");
            return asset.controlSchemes[m_KeyBoardAndMouseSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnCharge(InputAction.CallbackContext context);
        void OnAirBreaks(InputAction.CallbackContext context);
    }
}
