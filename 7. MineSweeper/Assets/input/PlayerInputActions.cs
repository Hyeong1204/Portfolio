//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/input/PlayerInputActions.inputactions
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

public partial class @PlayerInputActions : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputActions"",
    ""maps"": [
        {
            ""name"": ""Test"",
            ""id"": ""8aea1346-0ff4-432d-960e-5ca48e463e0c"",
            ""actions"": [
                {
                    ""name"": ""Test5"",
                    ""type"": ""Button"",
                    ""id"": ""a2f2a926-b23d-4973-bbc5-4047440213ae"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Test4"",
                    ""type"": ""Button"",
                    ""id"": ""7a36f204-6b2d-4fcc-92a0-71ce80f209e8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Test3"",
                    ""type"": ""Button"",
                    ""id"": ""a26be95c-4549-47a5-992e-4dc3dc000ab3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Test2"",
                    ""type"": ""Button"",
                    ""id"": ""ea640c8b-d26f-467d-914d-d3920389e06c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Test1"",
                    ""type"": ""Button"",
                    ""id"": ""ae089d06-cafe-4002-9c89-4c2b3583c621"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""1591bc12-108b-4e1b-b379-b7f5088568c6"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Kkeyboard & Mouse"",
                    ""action"": ""Test1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""35a49e30-c82e-42d7-9ec3-ccf54feb182a"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Kkeyboard & Mouse"",
                    ""action"": ""Test2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9db18535-a13c-4639-9dfe-614b950328e8"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Kkeyboard & Mouse"",
                    ""action"": ""Test3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""41c801a9-dd72-4740-9fe5-562cec43ee91"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Kkeyboard & Mouse"",
                    ""action"": ""Test4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8d611f96-cef2-4cb1-8dd6-ecc640647921"",
                    ""path"": ""<Keyboard>/5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Kkeyboard & Mouse"",
                    ""action"": ""Test5"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Player"",
            ""id"": ""8a587878-53df-4dcf-8bf2-54a59152d3e1"",
            ""actions"": [
                {
                    ""name"": ""MouseMove"",
                    ""type"": ""Value"",
                    ""id"": ""47c734ed-d726-4e3b-8750-02f7540964be"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""LeftClick"",
                    ""type"": ""Button"",
                    ""id"": ""2875231d-1e20-4419-895e-cbbf526c07cf"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""RightClick"",
                    ""type"": ""Button"",
                    ""id"": ""16508135-f275-4fba-a835-63d5baab013f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""482821ef-b14b-46ab-9f0a-47e250ec8452"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Kkeyboard & Mouse"",
                    ""action"": ""MouseMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dea5cd8f-0a4b-4090-8661-276aa597dd84"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Kkeyboard & Mouse"",
                    ""action"": ""LeftClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fd9b5037-15ad-4b44-9feb-4e0495c40cb5"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Kkeyboard & Mouse"",
                    ""action"": ""RightClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Kkeyboard & Mouse"",
            ""bindingGroup"": ""Kkeyboard & Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Test
        m_Test = asset.FindActionMap("Test", throwIfNotFound: true);
        m_Test_Test5 = m_Test.FindAction("Test5", throwIfNotFound: true);
        m_Test_Test4 = m_Test.FindAction("Test4", throwIfNotFound: true);
        m_Test_Test3 = m_Test.FindAction("Test3", throwIfNotFound: true);
        m_Test_Test2 = m_Test.FindAction("Test2", throwIfNotFound: true);
        m_Test_Test1 = m_Test.FindAction("Test1", throwIfNotFound: true);
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_MouseMove = m_Player.FindAction("MouseMove", throwIfNotFound: true);
        m_Player_LeftClick = m_Player.FindAction("LeftClick", throwIfNotFound: true);
        m_Player_RightClick = m_Player.FindAction("RightClick", throwIfNotFound: true);
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

    // Test
    private readonly InputActionMap m_Test;
    private ITestActions m_TestActionsCallbackInterface;
    private readonly InputAction m_Test_Test5;
    private readonly InputAction m_Test_Test4;
    private readonly InputAction m_Test_Test3;
    private readonly InputAction m_Test_Test2;
    private readonly InputAction m_Test_Test1;
    public struct TestActions
    {
        private @PlayerInputActions m_Wrapper;
        public TestActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Test5 => m_Wrapper.m_Test_Test5;
        public InputAction @Test4 => m_Wrapper.m_Test_Test4;
        public InputAction @Test3 => m_Wrapper.m_Test_Test3;
        public InputAction @Test2 => m_Wrapper.m_Test_Test2;
        public InputAction @Test1 => m_Wrapper.m_Test_Test1;
        public InputActionMap Get() { return m_Wrapper.m_Test; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(TestActions set) { return set.Get(); }
        public void SetCallbacks(ITestActions instance)
        {
            if (m_Wrapper.m_TestActionsCallbackInterface != null)
            {
                @Test5.started -= m_Wrapper.m_TestActionsCallbackInterface.OnTest5;
                @Test5.performed -= m_Wrapper.m_TestActionsCallbackInterface.OnTest5;
                @Test5.canceled -= m_Wrapper.m_TestActionsCallbackInterface.OnTest5;
                @Test4.started -= m_Wrapper.m_TestActionsCallbackInterface.OnTest4;
                @Test4.performed -= m_Wrapper.m_TestActionsCallbackInterface.OnTest4;
                @Test4.canceled -= m_Wrapper.m_TestActionsCallbackInterface.OnTest4;
                @Test3.started -= m_Wrapper.m_TestActionsCallbackInterface.OnTest3;
                @Test3.performed -= m_Wrapper.m_TestActionsCallbackInterface.OnTest3;
                @Test3.canceled -= m_Wrapper.m_TestActionsCallbackInterface.OnTest3;
                @Test2.started -= m_Wrapper.m_TestActionsCallbackInterface.OnTest2;
                @Test2.performed -= m_Wrapper.m_TestActionsCallbackInterface.OnTest2;
                @Test2.canceled -= m_Wrapper.m_TestActionsCallbackInterface.OnTest2;
                @Test1.started -= m_Wrapper.m_TestActionsCallbackInterface.OnTest1;
                @Test1.performed -= m_Wrapper.m_TestActionsCallbackInterface.OnTest1;
                @Test1.canceled -= m_Wrapper.m_TestActionsCallbackInterface.OnTest1;
            }
            m_Wrapper.m_TestActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Test5.started += instance.OnTest5;
                @Test5.performed += instance.OnTest5;
                @Test5.canceled += instance.OnTest5;
                @Test4.started += instance.OnTest4;
                @Test4.performed += instance.OnTest4;
                @Test4.canceled += instance.OnTest4;
                @Test3.started += instance.OnTest3;
                @Test3.performed += instance.OnTest3;
                @Test3.canceled += instance.OnTest3;
                @Test2.started += instance.OnTest2;
                @Test2.performed += instance.OnTest2;
                @Test2.canceled += instance.OnTest2;
                @Test1.started += instance.OnTest1;
                @Test1.performed += instance.OnTest1;
                @Test1.canceled += instance.OnTest1;
            }
        }
    }
    public TestActions @Test => new TestActions(this);

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_MouseMove;
    private readonly InputAction m_Player_LeftClick;
    private readonly InputAction m_Player_RightClick;
    public struct PlayerActions
    {
        private @PlayerInputActions m_Wrapper;
        public PlayerActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @MouseMove => m_Wrapper.m_Player_MouseMove;
        public InputAction @LeftClick => m_Wrapper.m_Player_LeftClick;
        public InputAction @RightClick => m_Wrapper.m_Player_RightClick;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @MouseMove.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMouseMove;
                @MouseMove.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMouseMove;
                @MouseMove.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMouseMove;
                @LeftClick.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeftClick;
                @LeftClick.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeftClick;
                @LeftClick.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLeftClick;
                @RightClick.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRightClick;
                @RightClick.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRightClick;
                @RightClick.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRightClick;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MouseMove.started += instance.OnMouseMove;
                @MouseMove.performed += instance.OnMouseMove;
                @MouseMove.canceled += instance.OnMouseMove;
                @LeftClick.started += instance.OnLeftClick;
                @LeftClick.performed += instance.OnLeftClick;
                @LeftClick.canceled += instance.OnLeftClick;
                @RightClick.started += instance.OnRightClick;
                @RightClick.performed += instance.OnRightClick;
                @RightClick.canceled += instance.OnRightClick;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    private int m_KkeyboardMouseSchemeIndex = -1;
    public InputControlScheme KkeyboardMouseScheme
    {
        get
        {
            if (m_KkeyboardMouseSchemeIndex == -1) m_KkeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Kkeyboard & Mouse");
            return asset.controlSchemes[m_KkeyboardMouseSchemeIndex];
        }
    }
    public interface ITestActions
    {
        void OnTest5(InputAction.CallbackContext context);
        void OnTest4(InputAction.CallbackContext context);
        void OnTest3(InputAction.CallbackContext context);
        void OnTest2(InputAction.CallbackContext context);
        void OnTest1(InputAction.CallbackContext context);
    }
    public interface IPlayerActions
    {
        void OnMouseMove(InputAction.CallbackContext context);
        void OnLeftClick(InputAction.CallbackContext context);
        void OnRightClick(InputAction.CallbackContext context);
    }
}
