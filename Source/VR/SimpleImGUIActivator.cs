using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UImGui
{
    public class SimpleImGUIActivator : MonoBehaviour
    {
        [SerializeField] private UImGui uImGui;
        [SerializeField] private InputActionProperty button;
        [SerializeField] private float holdDuration;
        private float _time;

        private void Start()
        {
            button.action.Enable();
        }

        private void LateUpdate()
        {
            if (button.action.IsPressed())
            {
                if (_time - Time.timeSinceLevelLoad < 0)
                {
                    uImGui.enabled = !uImGui.enabled;
                    _time = Time.time + holdDuration + 1f;
                }
            }
            else
            {
                _time = Time.timeSinceLevelLoad + holdDuration;
            }
        }
    }
}