using System;
using System.Collections.Generic;
using System.IO;
using Hexa.NET.ImGui;
using UImGui.Renderer;
using UImGui.Texture;
using UImGui.VR;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UTexture = UnityEngine.Texture;

namespace UImGui
{
	public static class UImGuiUtility
	{
		// public static IntPtr GetTextureId(UTexture texture) => Context?.TextureManager.GetTextureId(texture) ?? IntPtr.Zero; //does not compile
		// internal static SpriteInfo GetSpriteInfo(Sprite sprite) => Context?.TextureManager.GetSpriteInfo(sprite) ?? null; // does not compile

		internal static Context Context;
		internal static VRContext VRContext;

		#region Events
		
		public static event Action<UImGui> Layout;
		public static event Action<UImGui> OnInitialize;
		public static event Action<UImGui> OnDeinitialize;
		internal static void DoLayout(UImGui uimgui) => Layout?.Invoke(uimgui);
		internal static void DoOnInitialize(UImGui uimgui) => OnInitialize?.Invoke(uimgui);
		internal static void DoOnDeinitialize(UImGui uimgui) => OnDeinitialize?.Invoke(uimgui);
		#endregion

		#if UNITY_EDITOR
		[InitializeOnLoadMethod]
		internal static void AddLibraryPath()
		{
#if UNITY_EDITOR_WIN
			HexaGen.Runtime.LibraryLoader.CustomLoadFolders.Add(Path.Join(Application.dataPath, "../Packages/com.ycatdev.uimgui.extended/Plugins/imgui/win-x64"));
#endif
#if UNITY_EDITOR_LINUX
			HexaGen.Runtime.LibraryLoader.CustomLoadFolders.Add(Path.Join(Application.dataPath, "../Packages/com.ycatdev.uimgui.extended/Plugins/imgui/linux-x64"));
#endif
		}
#endif

		static UImGuiUtility()
		{
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
			HexaGen.Runtime.LibraryLoader.CustomLoadFolders.Add(Path.Join(Application.dataPath, "/Plugins/x86_64"));
#endif
#if UNITY_STANDALONE_LINUX && !UNITY_EDITOR
			HexaGen.Runtime.LibraryLoader.CustomLoadFolders.Add(Path.Join(Application.dataPath, "/Plugins/x86_64"));
#endif
		}
		
		internal static unsafe Context CreateContext()
		{
			return new Context
			{
				ImGuiContext = ImGui.CreateContext(),
/*#if !UIMGUI_REMOVE_IMPLOT
				ImPlotContext = ImPlotNET.ImPlot.CreateContext(),
#endif
#if !UIMGUI_REMOVE_IMNODES
			ImNodesContext = new IntPtr(imnodesNET.imnodes.CreateContext()),
#endif*/
				TextureManager = new TextureManager(),
				DrawCommands = new List<DrawCommand>(32)
			};
		}
		
		internal static VRContext CreateVRContext(VRConfiguration configuration)
		{
			return new VRContext
			{
				VirtualXRInput = new VirtualXRInput(configuration.vrInputAsset, configuration.handCursorMode),
				WorldSpaceTransformer = new WorldSpaceTransformer(configuration.worldSpaceConfig),
				VRManipulator = configuration.vrManipulator,
			};
		}

		internal static void DestroyContext(Context context)
		{
			ImGui.DestroyContext(context.ImGuiContext);

/*#if !UIMGUI_REMOVE_IMPLOT
			ImPlotNET.ImPlot.DestroyContext(context.ImPlotContext);
#endif
#if !UIMGUI_REMOVE_IMNODES
			imnodesNET.imnodes.DestroyContext(context.ImNodesContext);
#endif*/
		}

		internal static void SetCurrentContext(Context context, VRContext vrContext)
		{
			Context = context;
			ImGui.SetCurrentContext(context?.ImGuiContext ?? ImGuiContextPtr.Null);

/*#if !UIMGUI_REMOVE_IMPLOT
			ImPlotNET.ImPlot.SetImGuiContext(context?.ImGuiContext ?? IntPtr.Zero);
#endif
#if !UIMGUI_REMOVE_IMGUIZMO
			ImGuizmoNET.ImGuizmo.SetImGuiContext(context?.ImGuiContext ?? IntPtr.Zero);
#endif
#if !UIMGUI_REMOVE_IMNODES
			imnodesNET.imnodes.SetImGuiContext(context?.ImGuiContext ?? IntPtr.Zero);
#endif*/

			VRContext = vrContext;
		}
		
		public static void ResetStaticContext()
		{
			Context = null;
			VRContext = null;

			Layout = null;
			OnInitialize = null;
			OnDeinitialize = null;
		}
	}
}