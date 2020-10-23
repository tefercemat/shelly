// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""PlatformsInput"",
            ""id"": ""3b106bb4-a75b-4e98-9791-bf3e3cda32bd"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""PassThrough"",
                    ""id"": ""80450590-25b2-43e0-a5ad-09c1a4cfccdf"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""PassThrough"",
                    ""id"": ""358307cc-30a0-4732-b22f-b124045d4603"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""PassThrough"",
                    ""id"": ""cb33ec37-d49f-4503-ba9f-a6ab5025992f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PauseGame"",
                    ""type"": ""PassThrough"",
                    ""id"": ""a53f4332-ff8f-4915-95cd-28d17e1521bf"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Recall"",
                    ""type"": ""PassThrough"",
                    ""id"": ""95f120da-ea2d-4544-86d0-25d15d8631bd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Sideways"",
                    ""id"": ""baba3400-ba22-4d0a-bd6a-ec8ac5e3b92f"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""b574f5cb-5ef2-4991-965b-f5b7c8d19f0a"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""aec26afb-d071-43b8-a477-49b6c4bb4065"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""b2865d21-9e8b-43b2-b733-c42381974c50"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f24ceb8a-423c-4917-922f-4dfb1c9ca9ac"",
                    ""path"": ""<Keyboard>/p"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""59be785a-7e16-4fdb-b1d5-579c8c03feba"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PauseGame"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6c6837c1-a93b-44d7-a62b-89e9a802051e"",
                    ""path"": ""<Keyboard>/l"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Recall"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // PlatformsInput
        m_PlatformsInput = asset.FindActionMap("PlatformsInput", throwIfNotFound: true);
        m_PlatformsInput_Move = m_PlatformsInput.FindAction("Move", throwIfNotFound: true);
        m_PlatformsInput_Jump = m_PlatformsInput.FindAction("Jump", throwIfNotFound: true);
        m_PlatformsInput_Attack = m_PlatformsInput.FindAction("Attack", throwIfNotFound: true);
        m_PlatformsInput_PauseGame = m_PlatformsInput.FindAction("PauseGame", throwIfNotFound: true);
        m_PlatformsInput_Recall = m_PlatformsInput.FindAction("Recall", throwIfNotFound: true);
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

    // PlatformsInput
    private readonly InputActionMap m_PlatformsInput;
    private IPlatformsInputActions m_PlatformsInputActionsCallbackInterface;
    private readonly InputAction m_PlatformsInput_Move;
    private readonly InputAction m_PlatformsInput_Jump;
    private readonly InputAction m_PlatformsInput_Attack;
    private readonly InputAction m_PlatformsInput_PauseGame;
    private readonly InputAction m_PlatformsInput_Recall;
    public struct PlatformsInputActions
    {
        private @PlayerControls m_Wrapper;
        public PlatformsInputActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_PlatformsInput_Move;
        public InputAction @Jump => m_Wrapper.m_PlatformsInput_Jump;
        public InputAction @Attack => m_Wrapper.m_PlatformsInput_Attack;
        public InputAction @PauseGame => m_Wrapper.m_PlatformsInput_PauseGame;
        public InputAction @Recall => m_Wrapper.m_PlatformsInput_Recall;
        public InputActionMap Get() { return m_Wrapper.m_PlatformsInput; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlatformsInputActions set) { return set.Get(); }
        public void SetCallbacks(IPlatformsInputActions instance)
        {
            if (m_Wrapper.m_PlatformsInputActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_PlatformsInputActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_PlatformsInputActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_PlatformsInputActionsCallbackInterface.OnMove;
                @Jump.started -= m_Wrapper.m_PlatformsInputActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_PlatformsInputActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_PlatformsInputActionsCallbackInterface.OnJump;
                @Attack.started -= m_Wrapper.m_PlatformsInputActionsCallbackInterface.OnAttack;
                @Attack.performed -= m_Wrapper.m_PlatformsInputActionsCallbackInterface.OnAttack;
                @Attack.canceled -= m_Wrapper.m_PlatformsInputActionsCallbackInterface.OnAttack;
                @PauseGame.started -= m_Wrapper.m_PlatformsInputActionsCallbackInterface.OnPauseGame;
                @PauseGame.performed -= m_Wrapper.m_PlatformsInputActionsCallbackInterface.OnPauseGame;
                @PauseGame.canceled -= m_Wrapper.m_PlatformsInputActionsCallbackInterface.OnPauseGame;
                @Recall.started -= m_Wrapper.m_PlatformsInputActionsCallbackInterface.OnRecall;
                @Recall.performed -= m_Wrapper.m_PlatformsInputActionsCallbackInterface.OnRecall;
                @Recall.canceled -= m_Wrapper.m_PlatformsInputActionsCallbackInterface.OnRecall;
            }
            m_Wrapper.m_PlatformsInputActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Attack.started += instance.OnAttack;
                @Attack.performed += instance.OnAttack;
                @Attack.canceled += instance.OnAttack;
                @PauseGame.started += instance.OnPauseGame;
                @PauseGame.performed += instance.OnPauseGame;
                @PauseGame.canceled += instance.OnPauseGame;
                @Recall.started += instance.OnRecall;
                @Recall.performed += instance.OnRecall;
                @Recall.canceled += instance.OnRecall;
            }
        }
    }
    public PlatformsInputActions @PlatformsInput => new PlatformsInputActions(this);
    public interface IPlatformsInputActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
        void OnPauseGame(InputAction.CallbackContext context);
        void OnRecall(InputAction.CallbackContext context);
    }
}
