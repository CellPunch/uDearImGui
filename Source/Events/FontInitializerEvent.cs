using Hexa.NET.ImGui;
using UnityEngine.Events;

namespace UImGui.Events
{
	[System.Serializable]
	public class FontInitializerEvent : UnityEvent<ImGuiIOPtr> { }
}