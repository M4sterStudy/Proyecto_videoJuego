//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Input_Actions/player.inputactions
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

public partial class @Player: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @Player()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""player"",
    ""maps"": [
        {
            ""name"": ""personaje"",
            ""id"": ""07b1d8ab-c215-46fd-8651-f29779e2ee7c"",
            ""actions"": [
                {
                    ""name"": ""movimiento"",
                    ""type"": ""Value"",
                    ""id"": ""46108b3a-ceda-4b87-8b0c-1d0ca2801bef"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""saltar"",
                    ""type"": ""Button"",
                    ""id"": ""9b4ce59e-9de2-4294-8e71-c5f18ccb5588"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""correr"",
                    ""type"": ""Button"",
                    ""id"": ""0c742c1f-3ab4-4b09-9f6f-3255b597412a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""camara"",
                    ""type"": ""Value"",
                    ""id"": ""032e624b-b0d2-42ea-9cd7-a7650fba51bd"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""LightAttack"",
                    ""type"": ""Button"",
                    ""id"": ""789d3154-0154-4db4-9ffd-dedd2a26f175"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""HeavyAttack"",
                    ""type"": ""Button"",
                    ""id"": ""847d1280-5a2e-434e-b859-abc34790b5e1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""81797b9f-66e3-449f-8f42-edde1217863c"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""movimiento"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""5a8cf852-bf45-4375-92f0-869d723e3fd7"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""movimiento"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""06a2a50f-bb8b-4a98-981f-81d996be0053"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""movimiento"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""fad00b28-76c7-44f5-bac2-e4b4df7e3f88"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""movimiento"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""a79c8c42-df2b-4540-9f8d-fad19b9ccc1b"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""movimiento"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""1162241a-e0ed-48d7-b92b-80173d9d9981"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""movimiento"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""c2fd5d1a-ba6e-4002-a3e1-83bf721baa72"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""saltar"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""844eeacf-01b8-4500-8b31-9c2a7b109b46"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""saltar"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""20da7740-2fc5-40f8-b4bf-a24353d2cbf0"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""correr"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6dd5e1ab-8608-476d-933f-f4f6828e3866"",
                    ""path"": ""<Gamepad>/leftStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""correr"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8647471f-936c-4211-879b-1c84dd64dcc1"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""camara"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1423fe7b-d970-4e19-a899-b45a1dccd641"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""camara"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4e016458-c633-4d72-96f6-eed86e60efa6"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LightAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""780f256a-7259-4d41-91f2-6d7acc92103d"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LightAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e17ea32d-c73b-464c-9d5f-887b9d600fc8"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HeavyAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b037e5a8-51a3-4b86-912a-97922938f521"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HeavyAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // personaje
        m_personaje = asset.FindActionMap("personaje", throwIfNotFound: true);
        m_personaje_movimiento = m_personaje.FindAction("movimiento", throwIfNotFound: true);
        m_personaje_saltar = m_personaje.FindAction("saltar", throwIfNotFound: true);
        m_personaje_correr = m_personaje.FindAction("correr", throwIfNotFound: true);
        m_personaje_camara = m_personaje.FindAction("camara", throwIfNotFound: true);
        m_personaje_LightAttack = m_personaje.FindAction("LightAttack", throwIfNotFound: true);
        m_personaje_HeavyAttack = m_personaje.FindAction("HeavyAttack", throwIfNotFound: true);
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

    // personaje
    private readonly InputActionMap m_personaje;
    private List<IPersonajeActions> m_PersonajeActionsCallbackInterfaces = new List<IPersonajeActions>();
    private readonly InputAction m_personaje_movimiento;
    private readonly InputAction m_personaje_saltar;
    private readonly InputAction m_personaje_correr;
    private readonly InputAction m_personaje_camara;
    private readonly InputAction m_personaje_LightAttack;
    private readonly InputAction m_personaje_HeavyAttack;
    public struct PersonajeActions
    {
        private @Player m_Wrapper;
        public PersonajeActions(@Player wrapper) { m_Wrapper = wrapper; }
        public InputAction @movimiento => m_Wrapper.m_personaje_movimiento;
        public InputAction @saltar => m_Wrapper.m_personaje_saltar;
        public InputAction @correr => m_Wrapper.m_personaje_correr;
        public InputAction @camara => m_Wrapper.m_personaje_camara;
        public InputAction @LightAttack => m_Wrapper.m_personaje_LightAttack;
        public InputAction @HeavyAttack => m_Wrapper.m_personaje_HeavyAttack;
        public InputActionMap Get() { return m_Wrapper.m_personaje; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PersonajeActions set) { return set.Get(); }
        public void AddCallbacks(IPersonajeActions instance)
        {
            if (instance == null || m_Wrapper.m_PersonajeActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PersonajeActionsCallbackInterfaces.Add(instance);
            @movimiento.started += instance.OnMovimiento;
            @movimiento.performed += instance.OnMovimiento;
            @movimiento.canceled += instance.OnMovimiento;
            @saltar.started += instance.OnSaltar;
            @saltar.performed += instance.OnSaltar;
            @saltar.canceled += instance.OnSaltar;
            @correr.started += instance.OnCorrer;
            @correr.performed += instance.OnCorrer;
            @correr.canceled += instance.OnCorrer;
            @camara.started += instance.OnCamara;
            @camara.performed += instance.OnCamara;
            @camara.canceled += instance.OnCamara;
            @LightAttack.started += instance.OnLightAttack;
            @LightAttack.performed += instance.OnLightAttack;
            @LightAttack.canceled += instance.OnLightAttack;
            @HeavyAttack.started += instance.OnHeavyAttack;
            @HeavyAttack.performed += instance.OnHeavyAttack;
            @HeavyAttack.canceled += instance.OnHeavyAttack;
        }

        private void UnregisterCallbacks(IPersonajeActions instance)
        {
            @movimiento.started -= instance.OnMovimiento;
            @movimiento.performed -= instance.OnMovimiento;
            @movimiento.canceled -= instance.OnMovimiento;
            @saltar.started -= instance.OnSaltar;
            @saltar.performed -= instance.OnSaltar;
            @saltar.canceled -= instance.OnSaltar;
            @correr.started -= instance.OnCorrer;
            @correr.performed -= instance.OnCorrer;
            @correr.canceled -= instance.OnCorrer;
            @camara.started -= instance.OnCamara;
            @camara.performed -= instance.OnCamara;
            @camara.canceled -= instance.OnCamara;
            @LightAttack.started -= instance.OnLightAttack;
            @LightAttack.performed -= instance.OnLightAttack;
            @LightAttack.canceled -= instance.OnLightAttack;
            @HeavyAttack.started -= instance.OnHeavyAttack;
            @HeavyAttack.performed -= instance.OnHeavyAttack;
            @HeavyAttack.canceled -= instance.OnHeavyAttack;
        }

        public void RemoveCallbacks(IPersonajeActions instance)
        {
            if (m_Wrapper.m_PersonajeActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPersonajeActions instance)
        {
            foreach (var item in m_Wrapper.m_PersonajeActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PersonajeActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PersonajeActions @personaje => new PersonajeActions(this);
    public interface IPersonajeActions
    {
        void OnMovimiento(InputAction.CallbackContext context);
        void OnSaltar(InputAction.CallbackContext context);
        void OnCorrer(InputAction.CallbackContext context);
        void OnCamara(InputAction.CallbackContext context);
        void OnLightAttack(InputAction.CallbackContext context);
        void OnHeavyAttack(InputAction.CallbackContext context);
    }
}
