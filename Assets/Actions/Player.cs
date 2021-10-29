// GENERATED AUTOMATICALLY FROM 'Assets/Actions/Player.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Player : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Player()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Player"",
    ""maps"": [
        {
            ""name"": ""PlayerMan"",
            ""id"": ""f4e24676-dd78-4eef-abfb-3d6aaea130cc"",
            ""actions"": [
                {
                    ""name"": ""MoveAround"",
                    ""type"": ""Value"",
                    ""id"": ""d6e467f7-3112-4d7f-9759-42ed17a77443"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Lock"",
                    ""type"": ""Value"",
                    ""id"": ""e15e5fd6-aa6d-4669-a28e-1b7d3289868d"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""f1023523-81d7-42d1-a5df-2a73894c8f7e"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveAround"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""7f6b188d-c03f-44d0-811c-fe5288419c83"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveAround"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""1e4acf21-570c-4404-9aff-cc8c569a36e8"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveAround"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""a68a9191-47b6-4d98-97ed-ea101f3dc8b8"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveAround"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""b54b80b9-e0db-4d2c-89d0-0e1efe13a25e"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveAround"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""10c4fc05-b228-4f35-a31b-fef8edd2c57f"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveAround"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""36d3684b-cc79-4e54-b928-66bcb012af59"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Lock"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""aaa25404-52b2-40c9-8867-184d4d169c37"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Lock"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""d3183d02-c865-4351-9525-484b6a02df20"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Lock"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""3da7d034-e88d-4fb0-99f3-ad4e16c9aca4"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Lock"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""718dcc64-f8a6-4030-8097-902b7c4d5fe0"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Lock"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""88d02b84-1e80-4bfa-9e2f-03920eb3755c"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Lock"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // PlayerMan
        m_PlayerMan = asset.FindActionMap("PlayerMan", throwIfNotFound: true);
        m_PlayerMan_MoveAround = m_PlayerMan.FindAction("MoveAround", throwIfNotFound: true);
        m_PlayerMan_Lock = m_PlayerMan.FindAction("Lock", throwIfNotFound: true);
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

    // PlayerMan
    private readonly InputActionMap m_PlayerMan;
    private IPlayerManActions m_PlayerManActionsCallbackInterface;
    private readonly InputAction m_PlayerMan_MoveAround;
    private readonly InputAction m_PlayerMan_Lock;
    public struct PlayerManActions
    {
        private @Player m_Wrapper;
        public PlayerManActions(@Player wrapper) { m_Wrapper = wrapper; }
        public InputAction @MoveAround => m_Wrapper.m_PlayerMan_MoveAround;
        public InputAction @Lock => m_Wrapper.m_PlayerMan_Lock;
        public InputActionMap Get() { return m_Wrapper.m_PlayerMan; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerManActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerManActions instance)
        {
            if (m_Wrapper.m_PlayerManActionsCallbackInterface != null)
            {
                @MoveAround.started -= m_Wrapper.m_PlayerManActionsCallbackInterface.OnMoveAround;
                @MoveAround.performed -= m_Wrapper.m_PlayerManActionsCallbackInterface.OnMoveAround;
                @MoveAround.canceled -= m_Wrapper.m_PlayerManActionsCallbackInterface.OnMoveAround;
                @Lock.started -= m_Wrapper.m_PlayerManActionsCallbackInterface.OnLock;
                @Lock.performed -= m_Wrapper.m_PlayerManActionsCallbackInterface.OnLock;
                @Lock.canceled -= m_Wrapper.m_PlayerManActionsCallbackInterface.OnLock;
            }
            m_Wrapper.m_PlayerManActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MoveAround.started += instance.OnMoveAround;
                @MoveAround.performed += instance.OnMoveAround;
                @MoveAround.canceled += instance.OnMoveAround;
                @Lock.started += instance.OnLock;
                @Lock.performed += instance.OnLock;
                @Lock.canceled += instance.OnLock;
            }
        }
    }
    public PlayerManActions @PlayerMan => new PlayerManActions(this);
    public interface IPlayerManActions
    {
        void OnMoveAround(InputAction.CallbackContext context);
        void OnLock(InputAction.CallbackContext context);
    }
}
