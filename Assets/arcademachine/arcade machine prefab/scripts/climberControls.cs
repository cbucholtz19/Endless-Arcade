//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/climberControls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @ClimberControls : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @ClimberControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""climberControls"",
    ""maps"": [
        {
            ""name"": ""movement"",
            ""id"": ""77717084-56d0-4edb-9acb-2b30503b9336"",
            ""actions"": [
                {
                    ""name"": ""move"",
                    ""type"": ""Value"",
                    ""id"": ""540e85a0-382f-4bbc-ba88-13a440250ee4"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""play"",
                    ""type"": ""Button"",
                    ""id"": ""6163bdc8-7017-4e8d-8166-b4d7a45c609b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""wasd"",
                    ""id"": ""6eff2628-d68f-4622-9514-58911ac8b5d8"",
                    ""path"": ""2DVector(mode=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""c9070ced-2dcb-4d25-8b9e-17d0d4f6380a"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""17cca50b-1eea-4a49-ba35-a595c05266fe"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""27ae753b-732a-466b-a5d3-801f0f12040d"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""3ede8c76-ef16-4ca0-86ed-3eaa741b8740"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""8a79533d-a11a-466d-acb2-25e9a441077c"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""play"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // movement
        m_movement = asset.FindActionMap("movement", throwIfNotFound: true);
        m_movement_move = m_movement.FindAction("move", throwIfNotFound: true);
        m_movement_play = m_movement.FindAction("play", throwIfNotFound: true);
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
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // movement
    private readonly InputActionMap m_movement;
    private IMovementActions m_MovementActionsCallbackInterface;
    private readonly InputAction m_movement_move;
    private readonly InputAction m_movement_play;
    public struct MovementActions
    {
        private @ClimberControls m_Wrapper;
        public MovementActions(@ClimberControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @move => m_Wrapper.m_movement_move;
        public InputAction @play => m_Wrapper.m_movement_play;
        public InputActionMap Get() { return m_Wrapper.m_movement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MovementActions set) { return set.Get(); }
        public void SetCallbacks(IMovementActions instance)
        {
            if (m_Wrapper.m_MovementActionsCallbackInterface != null)
            {
                @move.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnMove;
                @move.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnMove;
                @move.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnMove;
                @play.started -= m_Wrapper.m_MovementActionsCallbackInterface.OnPlay;
                @play.performed -= m_Wrapper.m_MovementActionsCallbackInterface.OnPlay;
                @play.canceled -= m_Wrapper.m_MovementActionsCallbackInterface.OnPlay;
            }
            m_Wrapper.m_MovementActionsCallbackInterface = instance;
            if (instance != null)
            {
                @move.started += instance.OnMove;
                @move.performed += instance.OnMove;
                @move.canceled += instance.OnMove;
                @play.started += instance.OnPlay;
                @play.performed += instance.OnPlay;
                @play.canceled += instance.OnPlay;
            }
        }
    }
    public MovementActions @movement => new MovementActions(this);
    public interface IMovementActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnPlay(InputAction.CallbackContext context);
    }
}
