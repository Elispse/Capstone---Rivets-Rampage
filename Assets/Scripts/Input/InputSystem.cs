using Unity.Entities;
using Unity.Mathematics;
using UnityEngine.InputSystem;
using UnityEngine;

public partial class InputSystem : SystemBase
{
    private IA_Controls controls;

    protected override void OnCreate()
    {
        if (!SystemAPI.TryGetSingleton<InputComponent>(out InputComponent input))
        {
            EntityManager.CreateEntity(typeof(InputComponent));
        }

        controls = new IA_Controls();
        controls.Enable();
    }

    protected override void OnUpdate()
    {
        Vector2 moveVector = controls.FindAction("Movement").ReadValue<Vector2>();
        Vector2 mousePosition = controls.FindAction("MousePosition").ReadValue<Vector2>();
        bool isPressingLMB = controls.FindAction("Shoot").ReadValue<float>() == 1 ? true : false;

    }
}
