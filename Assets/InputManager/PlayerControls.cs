//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.11.2
//     from Assets/InputManager/PlayerControls.inputactions
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

public partial class @PlayerControls: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Character"",
            ""id"": ""b298c919-c201-418f-9fc7-054f6a4a2d3d"",
            ""actions"": [
                {
                    ""name"": ""Fire"",
                    ""type"": ""Button"",
                    ""id"": ""596aa7f8-3e81-4d6f-91c3-4eb1d5ebd141"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""fc865e3e-12cc-4c6d-8d98-610b30bdfc65"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""Value"",
                    ""id"": ""cfa6806e-c563-486e-b33f-af4918b39444"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Run"",
                    ""type"": ""Button"",
                    ""id"": ""0d6682c0-14f5-4199-afe4-593583ee6e07"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""EquipWeapon1"",
                    ""type"": ""Button"",
                    ""id"": ""562f0e5e-7c07-4031-8ab7-24ce376e476e"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""EquipWeapon2"",
                    ""type"": ""Button"",
                    ""id"": ""4ed9c390-a9b9-4592-80fc-d09164830023"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""EquipWeapon3"",
                    ""type"": ""Button"",
                    ""id"": ""77cb8418-c67c-45aa-a54b-9dfa50dfaba7"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""EquipWeapon4"",
                    ""type"": ""Button"",
                    ""id"": ""8dd7c6c4-084e-4d14-944e-eb97127a7d83"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""EquipWeapon5"",
                    ""type"": ""Button"",
                    ""id"": ""d01a149b-2288-43ce-a2f6-aa6053139a1a"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""DropCurrentWeapon"",
                    ""type"": ""Button"",
                    ""id"": ""43f38db3-c9b4-4d35-a535-df91eff02da7"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Reload"",
                    ""type"": ""Button"",
                    ""id"": ""1796c6b4-28bc-4974-af76-f9b1877c1f63"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""AimPrecisely"",
                    ""type"": ""Button"",
                    ""id"": ""e422c202-f6d0-4a34-9bdc-d3e5044a89ea"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""LockIntoTarget"",
                    ""type"": ""Button"",
                    ""id"": ""0d4a81c9-00e4-4302-a63d-9d8f4ae5d497"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ToogleBurst"",
                    ""type"": ""Button"",
                    ""id"": ""6f9494f9-d34d-4103-9919-1a7f15451416"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""82cdba68-f924-4fa9-b31d-7197f68e8ad9"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""83b5273a-1578-4b7f-be54-da76008f96f1"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""18554ed3-7c15-4898-80ae-ff8f79e95fb0"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""c81dd3b5-4d50-440d-83b2-f50d9b179673"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""dfc0949c-3731-41dc-b1d5-5644cdcd0cdc"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""3300d716-146a-414f-a376-0def67e6d1f0"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""9bf7522d-fd6e-48c7-a239-f22f12564e0a"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""96c49808-a829-4b8c-9940-66d5092216bf"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""15a62888-d296-4a5e-a523-edde0710c690"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""33c98092-f083-4ff9-8dab-16c7bd0e50dd"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""EquipWeapon1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""80590da1-4f51-46d0-90d3-4a51b3fb4df2"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""EquipWeapon2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7fbd2a2a-c7b1-4291-abd6-61ffc677826c"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""EquipWeapon3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3ac8b4cf-6ae2-48cc-9296-f153d246bb7a"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""EquipWeapon4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3aba73cb-5537-4112-a3ba-928785634f53"",
                    ""path"": ""<Keyboard>/5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""EquipWeapon5"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3d242e22-93ed-4efe-9054-8d0879d99e14"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Reload"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c4f36350-4eba-4c56-ab6a-77e75d07ea64"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""AimPrecisely"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6c13aa57-cd3a-482d-9a42-df52d4cfccbd"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LockIntoTarget"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f20d9c52-753d-4c43-8e16-d0e2cdb44bf9"",
                    ""path"": ""<Keyboard>/g"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DropCurrentWeapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""429777ac-89ab-4cc6-b7a9-5ce3ab4a5082"",
                    ""path"": ""<Keyboard>/t"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ToogleBurst"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c21d62f7-8c44-41d8-927a-191d9a398617"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Character
        m_Character = asset.FindActionMap("Character", throwIfNotFound: true);
        m_Character_Fire = m_Character.FindAction("Fire", throwIfNotFound: true);
        m_Character_Movement = m_Character.FindAction("Movement", throwIfNotFound: true);
        m_Character_Aim = m_Character.FindAction("Aim", throwIfNotFound: true);
        m_Character_Run = m_Character.FindAction("Run", throwIfNotFound: true);
        m_Character_EquipWeapon1 = m_Character.FindAction("EquipWeapon1", throwIfNotFound: true);
        m_Character_EquipWeapon2 = m_Character.FindAction("EquipWeapon2", throwIfNotFound: true);
        m_Character_EquipWeapon3 = m_Character.FindAction("EquipWeapon3", throwIfNotFound: true);
        m_Character_EquipWeapon4 = m_Character.FindAction("EquipWeapon4", throwIfNotFound: true);
        m_Character_EquipWeapon5 = m_Character.FindAction("EquipWeapon5", throwIfNotFound: true);
        m_Character_DropCurrentWeapon = m_Character.FindAction("DropCurrentWeapon", throwIfNotFound: true);
        m_Character_Reload = m_Character.FindAction("Reload", throwIfNotFound: true);
        m_Character_AimPrecisely = m_Character.FindAction("AimPrecisely", throwIfNotFound: true);
        m_Character_LockIntoTarget = m_Character.FindAction("LockIntoTarget", throwIfNotFound: true);
        m_Character_ToogleBurst = m_Character.FindAction("ToogleBurst", throwIfNotFound: true);
        m_Character_Interact = m_Character.FindAction("Interact", throwIfNotFound: true);
    }

    ~@PlayerControls()
    {
        UnityEngine.Debug.Assert(!m_Character.enabled, "This will cause a leak and performance issues, PlayerControls.Character.Disable() has not been called.");
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

    // Character
    private readonly InputActionMap m_Character;
    private List<ICharacterActions> m_CharacterActionsCallbackInterfaces = new List<ICharacterActions>();
    private readonly InputAction m_Character_Fire;
    private readonly InputAction m_Character_Movement;
    private readonly InputAction m_Character_Aim;
    private readonly InputAction m_Character_Run;
    private readonly InputAction m_Character_EquipWeapon1;
    private readonly InputAction m_Character_EquipWeapon2;
    private readonly InputAction m_Character_EquipWeapon3;
    private readonly InputAction m_Character_EquipWeapon4;
    private readonly InputAction m_Character_EquipWeapon5;
    private readonly InputAction m_Character_DropCurrentWeapon;
    private readonly InputAction m_Character_Reload;
    private readonly InputAction m_Character_AimPrecisely;
    private readonly InputAction m_Character_LockIntoTarget;
    private readonly InputAction m_Character_ToogleBurst;
    private readonly InputAction m_Character_Interact;
    public struct CharacterActions
    {
        private @PlayerControls m_Wrapper;
        public CharacterActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Fire => m_Wrapper.m_Character_Fire;
        public InputAction @Movement => m_Wrapper.m_Character_Movement;
        public InputAction @Aim => m_Wrapper.m_Character_Aim;
        public InputAction @Run => m_Wrapper.m_Character_Run;
        public InputAction @EquipWeapon1 => m_Wrapper.m_Character_EquipWeapon1;
        public InputAction @EquipWeapon2 => m_Wrapper.m_Character_EquipWeapon2;
        public InputAction @EquipWeapon3 => m_Wrapper.m_Character_EquipWeapon3;
        public InputAction @EquipWeapon4 => m_Wrapper.m_Character_EquipWeapon4;
        public InputAction @EquipWeapon5 => m_Wrapper.m_Character_EquipWeapon5;
        public InputAction @DropCurrentWeapon => m_Wrapper.m_Character_DropCurrentWeapon;
        public InputAction @Reload => m_Wrapper.m_Character_Reload;
        public InputAction @AimPrecisely => m_Wrapper.m_Character_AimPrecisely;
        public InputAction @LockIntoTarget => m_Wrapper.m_Character_LockIntoTarget;
        public InputAction @ToogleBurst => m_Wrapper.m_Character_ToogleBurst;
        public InputAction @Interact => m_Wrapper.m_Character_Interact;
        public InputActionMap Get() { return m_Wrapper.m_Character; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CharacterActions set) { return set.Get(); }
        public void AddCallbacks(ICharacterActions instance)
        {
            if (instance == null || m_Wrapper.m_CharacterActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_CharacterActionsCallbackInterfaces.Add(instance);
            @Fire.started += instance.OnFire;
            @Fire.performed += instance.OnFire;
            @Fire.canceled += instance.OnFire;
            @Movement.started += instance.OnMovement;
            @Movement.performed += instance.OnMovement;
            @Movement.canceled += instance.OnMovement;
            @Aim.started += instance.OnAim;
            @Aim.performed += instance.OnAim;
            @Aim.canceled += instance.OnAim;
            @Run.started += instance.OnRun;
            @Run.performed += instance.OnRun;
            @Run.canceled += instance.OnRun;
            @EquipWeapon1.started += instance.OnEquipWeapon1;
            @EquipWeapon1.performed += instance.OnEquipWeapon1;
            @EquipWeapon1.canceled += instance.OnEquipWeapon1;
            @EquipWeapon2.started += instance.OnEquipWeapon2;
            @EquipWeapon2.performed += instance.OnEquipWeapon2;
            @EquipWeapon2.canceled += instance.OnEquipWeapon2;
            @EquipWeapon3.started += instance.OnEquipWeapon3;
            @EquipWeapon3.performed += instance.OnEquipWeapon3;
            @EquipWeapon3.canceled += instance.OnEquipWeapon3;
            @EquipWeapon4.started += instance.OnEquipWeapon4;
            @EquipWeapon4.performed += instance.OnEquipWeapon4;
            @EquipWeapon4.canceled += instance.OnEquipWeapon4;
            @EquipWeapon5.started += instance.OnEquipWeapon5;
            @EquipWeapon5.performed += instance.OnEquipWeapon5;
            @EquipWeapon5.canceled += instance.OnEquipWeapon5;
            @DropCurrentWeapon.started += instance.OnDropCurrentWeapon;
            @DropCurrentWeapon.performed += instance.OnDropCurrentWeapon;
            @DropCurrentWeapon.canceled += instance.OnDropCurrentWeapon;
            @Reload.started += instance.OnReload;
            @Reload.performed += instance.OnReload;
            @Reload.canceled += instance.OnReload;
            @AimPrecisely.started += instance.OnAimPrecisely;
            @AimPrecisely.performed += instance.OnAimPrecisely;
            @AimPrecisely.canceled += instance.OnAimPrecisely;
            @LockIntoTarget.started += instance.OnLockIntoTarget;
            @LockIntoTarget.performed += instance.OnLockIntoTarget;
            @LockIntoTarget.canceled += instance.OnLockIntoTarget;
            @ToogleBurst.started += instance.OnToogleBurst;
            @ToogleBurst.performed += instance.OnToogleBurst;
            @ToogleBurst.canceled += instance.OnToogleBurst;
            @Interact.started += instance.OnInteract;
            @Interact.performed += instance.OnInteract;
            @Interact.canceled += instance.OnInteract;
        }

        private void UnregisterCallbacks(ICharacterActions instance)
        {
            @Fire.started -= instance.OnFire;
            @Fire.performed -= instance.OnFire;
            @Fire.canceled -= instance.OnFire;
            @Movement.started -= instance.OnMovement;
            @Movement.performed -= instance.OnMovement;
            @Movement.canceled -= instance.OnMovement;
            @Aim.started -= instance.OnAim;
            @Aim.performed -= instance.OnAim;
            @Aim.canceled -= instance.OnAim;
            @Run.started -= instance.OnRun;
            @Run.performed -= instance.OnRun;
            @Run.canceled -= instance.OnRun;
            @EquipWeapon1.started -= instance.OnEquipWeapon1;
            @EquipWeapon1.performed -= instance.OnEquipWeapon1;
            @EquipWeapon1.canceled -= instance.OnEquipWeapon1;
            @EquipWeapon2.started -= instance.OnEquipWeapon2;
            @EquipWeapon2.performed -= instance.OnEquipWeapon2;
            @EquipWeapon2.canceled -= instance.OnEquipWeapon2;
            @EquipWeapon3.started -= instance.OnEquipWeapon3;
            @EquipWeapon3.performed -= instance.OnEquipWeapon3;
            @EquipWeapon3.canceled -= instance.OnEquipWeapon3;
            @EquipWeapon4.started -= instance.OnEquipWeapon4;
            @EquipWeapon4.performed -= instance.OnEquipWeapon4;
            @EquipWeapon4.canceled -= instance.OnEquipWeapon4;
            @EquipWeapon5.started -= instance.OnEquipWeapon5;
            @EquipWeapon5.performed -= instance.OnEquipWeapon5;
            @EquipWeapon5.canceled -= instance.OnEquipWeapon5;
            @DropCurrentWeapon.started -= instance.OnDropCurrentWeapon;
            @DropCurrentWeapon.performed -= instance.OnDropCurrentWeapon;
            @DropCurrentWeapon.canceled -= instance.OnDropCurrentWeapon;
            @Reload.started -= instance.OnReload;
            @Reload.performed -= instance.OnReload;
            @Reload.canceled -= instance.OnReload;
            @AimPrecisely.started -= instance.OnAimPrecisely;
            @AimPrecisely.performed -= instance.OnAimPrecisely;
            @AimPrecisely.canceled -= instance.OnAimPrecisely;
            @LockIntoTarget.started -= instance.OnLockIntoTarget;
            @LockIntoTarget.performed -= instance.OnLockIntoTarget;
            @LockIntoTarget.canceled -= instance.OnLockIntoTarget;
            @ToogleBurst.started -= instance.OnToogleBurst;
            @ToogleBurst.performed -= instance.OnToogleBurst;
            @ToogleBurst.canceled -= instance.OnToogleBurst;
            @Interact.started -= instance.OnInteract;
            @Interact.performed -= instance.OnInteract;
            @Interact.canceled -= instance.OnInteract;
        }

        public void RemoveCallbacks(ICharacterActions instance)
        {
            if (m_Wrapper.m_CharacterActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(ICharacterActions instance)
        {
            foreach (var item in m_Wrapper.m_CharacterActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_CharacterActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public CharacterActions @Character => new CharacterActions(this);
    public interface ICharacterActions
    {
        void OnFire(InputAction.CallbackContext context);
        void OnMovement(InputAction.CallbackContext context);
        void OnAim(InputAction.CallbackContext context);
        void OnRun(InputAction.CallbackContext context);
        void OnEquipWeapon1(InputAction.CallbackContext context);
        void OnEquipWeapon2(InputAction.CallbackContext context);
        void OnEquipWeapon3(InputAction.CallbackContext context);
        void OnEquipWeapon4(InputAction.CallbackContext context);
        void OnEquipWeapon5(InputAction.CallbackContext context);
        void OnDropCurrentWeapon(InputAction.CallbackContext context);
        void OnReload(InputAction.CallbackContext context);
        void OnAimPrecisely(InputAction.CallbackContext context);
        void OnLockIntoTarget(InputAction.CallbackContext context);
        void OnToogleBurst(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
    }
}
