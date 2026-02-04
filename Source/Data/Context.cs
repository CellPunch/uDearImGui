using System;
using System.Collections.Generic;
using Hexa.NET.ImGui;
using UImGui.Renderer;
using UImGui.Texture;

namespace UImGui
{
	internal sealed class Context
	{
		public ImGuiContextPtr ImGuiContext;
		public IntPtr ImNodesContext;
		public IntPtr ImPlotContext;
		public TextureManager TextureManager;
		public List<DrawCommand> DrawCommands;
	}
}