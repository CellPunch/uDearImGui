using Hexa.NET.ImGui;
using UnityEngine;
using UnityEngine.Rendering;

namespace UImGui.Assets
{
	[CreateAssetMenu(menuName = "Dear ImGui/Style")]
	internal sealed class StyleAsset : ScriptableObject
	{
		[Tooltip("Global alpha applies to everything in ImGui.")]
		[Range(0.0f, 1.0f)]
		public float alpha;

		[Tooltip("Additional alpha multiplier applied by BeginDisabled(). Multiply over current value of Alpha.")]
		public float disabledAlpha;

		[Tooltip("Padding within a window.")]
		public Vector2 windowPadding;

		[Tooltip("Radius of window corners rounding. Set to 0.0f to have rectangular windows. " +
		         "Large values tend to lead to variety of artifacts and are not recommended.")]
		public float windowRounding;

		[Tooltip("Thickness of border around windows. Generally set to 0.0f or 1.0f. " +
		         "(Other values are not well tested and more CPU/GPU costly).")]
		public float windowBorderSize;

		[Tooltip("Hit-testing extent outside/inside resizing border. Also extend determination of hovered window. " +
		         "Generally meaningfully larger than WindowBorderSize to make it easy to reach borders.")]
		[Min(0.01f)]
		public float windowBorderHoverPadding;

		[Tooltip("Minimum window size. This is a global setting. " +
		         "If you want to constraint individual windows, use SetNextWindowSizeConstraints().")]
		public Vector2 windowMinSize;

		[Tooltip("Alignment for title bar text. Defaults to (0.0f,0.5f) for left-aligned,vertically centered.")]
		public Vector2 windowTitleAlign;

		[Tooltip("Side of the collapsing/docking button in the title bar (left/right). Defaults to ImGuiDir_Left.")]
		public ImGuiDir windowMenuButtonPosition;

		[Tooltip("Radius of child window corners rounding. Set to 0.0f to have rectangular windows.")]
		public float childRounding;

		[Tooltip("Thickness of border around child windows. Generally set to 0.0f or 1.0f. " +
		         "(Other values are not well tested and more CPU/GPU costly).")]
		public float childBorderSize;

		[Tooltip("Radius of popup window corners rounding. (Note that tooltip windows use WindowRounding)")]
		public float popupRounding;

		[Tooltip("Thickness of border around popup/tooltip windows. Generally set to 0.0f or 1.0f. " +
		         "(Other values are not well tested and more CPU/GPU costly).")]
		public float popupBorderSize;

		[Tooltip("Padding within a framed rectangle (used by most widgets).")]
		public Vector2 framePadding;

		[Tooltip("Radius of frame corners rounding. Set to 0.0f to have rectangular frame (used by most widgets).")]
		public float frameRounding;

		[Tooltip("Thickness of border around frames. Generally set to 0.0f or 1.0f. " +
		         "(Other values are not well tested and more CPU/GPU costly).")]
		public float frameBorderSize;

		[Tooltip("Horizontal and vertical spacing between widgets/lines.")]
		public Vector2 itemSpacing;

		[Tooltip("Horizontal and vertical spacing between within elements of a composed widget (e.g. a slider and its label).")]
		public Vector2 itemInnerSpacing;

		[Tooltip("Padding within a table cell.")]
		public Vector2 cellPadding;

		[Tooltip("Expand reactive bounding box for touch-based system where touch position is not accurate enough. " +
		         "Unfortunately we don't sort widgets so priority on overlap will always be given to the first widget. " +
		         "So don't grow this too much!")]
		public Vector2 touchExtraPadding;

		[Tooltip("Horizontal indentation when e.g. entering a tree node. Generally == (FontSize + FramePadding.x*2).")]
		public float indentSpacing;

		[Tooltip("Minimum horizontal spacing between two columns. Preferably > (FramePadding.x + 1).")]
		public float columnsMinSpacing;

		[Tooltip("Width of the vertical scrollbar, Height of the horizontal scrollbar.")]
		public float scrollbarSize;

		[Tooltip("Radius of grab corners for scrollbar.")]
		public float scrollbarRounding;

		[Tooltip("Padding of scrollbar grab within its frame (same for both axes).")]
		public float scrollbarPadding;

		[Tooltip("Minimum width/height of a grab box for slider/scrollbar.")]
		public float grabMinSize;

		[Tooltip("Radius of grabs corners rounding. Set to 0.0f to have rectangular slider grabs.")]
		public float grabRounding;

		[Tooltip("The size in pixels of the dead-zone around zero on logarithmic sliders that cross zero.")]
		public float logSliderDeadzone;
		
		/* // added in DearImgui 1.92.6 WIP
		[Tooltip("Rounding of Image() calls.")]
		public float imageRounding;
		*/
		
		[Tooltip("Thickness of border around Image() calls.")]
		public float imageBorderSize;
		
		[Tooltip("Radius of upper corners of a tab. Set to 0.0f to have rectangular tabs.")]
		public float tabRounding;

		[Tooltip("Thickness of border around tabs.")]
		public float tabBorderSize;

		[Tooltip("Minimum tab width, to make tabs larger than their contents. TabBar buttons are not affected.")]
		public float tabMinWidthBase;

		[Tooltip(" Minimum tab width after shrinking, when using ImGuiTabBarFlags_FittingPolicyMixed policy.")]
		public float tabMinWidthShrink;

		[Tooltip("-1: always visible. 0.0f: visible when hovered. >0.0f: visible when hovered if minimum width.")]
		public float tabCloseButtonMinWidthSelected;

		[Tooltip("-1: always visible. 0.0f: visible when hovered. >0.0f: visible when hovered if minimum width. FLT_MAX: never show close button when unselected.")]
		public float tabCloseButtonMinWidthUnselected;

		[Tooltip("Thickness of tab-bar separator, which takes on the tab active color to denote focus.")]
		public float tabBarBorderSize;

		[Tooltip("Thickness of tab-bar overline, which highlights the selected tab-bar.")]
		public float tabBarOverlineSize;

		[Tooltip("Angle of angled headers (supported values range from -50.0f degrees to +50.0f degrees).")]
		public float tableAngledHeadersAngle;

		[Tooltip("Alignment of angled headers within the cell")]
		public Vector2 tableAngledHeadersTextAlign;

		[Tooltip("Default way to draw lines connecting TreeNode hierarchy. " +
		         "ImGuiTreeNodeFlags_DrawLinesNone or ImGuiTreeNodeFlags_DrawLinesFull or ImGuiTreeNodeFlags_DrawLinesToNodes.")]
		public ImGuiTreeNodeFlags treeLinesFlags = ImGuiTreeNodeFlags.DrawLinesNone;

		[Tooltip("Thickness of outlines when using ImGuiTreeNodeFlags_DrawLines.")]
		public float treeLinesSize;

		[Tooltip("Radius of lines connecting child nodes to the vertical line.")]
		public float treeLinesRounding;

		/* added in DearImGui 1.92.5
		[Tooltip("Radius of the drag and drop target frame.")]
		public float dragDropTargetRounding;

		[Tooltip("Thickness of the drag and drop target border.")]
		public float dragDropTargetBorderSize;
		
		[Tooltip("Size to expand the drag and drop target from actual target item size.")]
		public float dragDropTargetPadding;
		*/
		
		/* added in DearImGui 1.92.6 WIP
		[Tooltip("Size of R/G/B/A color markers for ColorEdit4() and for Drags/Sliders when using ImGuiSliderFlags_ColorMarkers.")]
		public float colorMarkerSize;
		*/

		[Tooltip("Side of the color button in the ColorEdit4 widget (left/right). Defaults to ImGuiDir_Right.")]
		public ImGuiDir colorButtonPosition;

		[Tooltip("Alignment of button text when button is larger than text. " +
		         "Defaults to (0.5f, 0.5f) (centered).")]
		public Vector2 buttonTextAlign;

		[Tooltip("Alignment of selectable text when selectable is larger than text. " +
		         "Defaults to (0.0f, 0.0f) (top-left aligned).")]
		public Vector2 selectableTextAlign;

		[Tooltip("Thickness of border in SeparatorText()")]
		public float separatorTextBorderSize;

		[Tooltip("Alignment of text within the separator. Defaults to (0.0f, 0.5f) (left aligned, center).")]
		public Vector2 separatorTextAlign;

		[Tooltip("Horizontal offset of text from each edge of the separator + spacing on other axis. " +
		         "Generally small values. .y is recommended to be == FramePadding.y.")]
		public Vector2 separatorTextPadding;
		
		[Tooltip("Window position are clamped to be visible within the display area by at least this amount. " +
		         "Only applies to regular windows.")]
		public Vector2 displayWindowPadding;

		[Tooltip("If you cannot see the edges of your screen (e.g. on a TV) increase the safe area padding. " +
		         "Apply to popups/tooltips as well regular windows. NB: Prefer configuring your TV sets correctly!")]
		public Vector2 displaySafeAreaPadding;

		[Tooltip("Scale software rendered mouse cursor (when io.MouseDrawCursor is enabled). May be removed later.")]
		[Min(1)]
		public float mouseCursorScale;

		[Tooltip("Enable anti-aliasing on lines/borders. Disable if you are really tight on CPU/GPU.")]
		public bool antiAliasedLines;

		[Tooltip("Enable anti-aliased lines/borders using textures where possible. " +
		         "Require backend to render with bilinear filtering. " +
		         "Latched at the beginning of the frame (copied to ImDrawList).")]
		public bool antiAliasedLinesUseTex;

		[Tooltip("Enable anti-aliasing on filled shapes (rounded rectangles, circles, etc.)")]
		public bool antiAliasedFill;

		[Tooltip("Tessellation tolerance when using PathBezierCurveTo() without a specific number of segments. " +
		         "Decrease for highly tessellated curves (higher quality, more polygons), increase to reduce quality.")]
		[Min(0.01f)]
		public float curveTessellationTol;

		[Tooltip("Maximum error (in pixels) allowed when using AddCircle()/AddCircleFilled() or " +
		         "drawing rounded corner rectangles with no explicit segment count specified. " +
		         "Decrease for higher quality but more geometry. Cannot be 0.")]
		[Min(0.01f)]
		public float circleTessellationMaxError;
		
		[HideInInspector]
		public SerializedDictionary<string, Color> colors = new();

		public void ApplyTo(ImGuiStylePtr s)
		{
			s.Alpha = alpha;
			s.DisabledAlpha = disabledAlpha;

			s.WindowPadding = windowPadding.ToSystem();
			s.WindowRounding = windowRounding;
			s.WindowBorderSize = windowBorderSize;
			s.WindowBorderHoverPadding = windowBorderHoverPadding;
			s.WindowMinSize = windowMinSize.ToSystem();
			s.WindowTitleAlign = windowTitleAlign.ToSystem();
			s.WindowMenuButtonPosition = windowMenuButtonPosition;

			s.ChildRounding = childRounding;
			s.ChildBorderSize = childBorderSize;

			s.PopupRounding = popupRounding;
			s.PopupBorderSize = popupBorderSize;

			s.FramePadding = framePadding.ToSystem();
			s.FrameRounding = frameRounding;
			s.FrameBorderSize = frameBorderSize;

			s.ItemSpacing = itemSpacing.ToSystem();
			s.ItemInnerSpacing = itemInnerSpacing.ToSystem();

			s.CellPadding = cellPadding.ToSystem();

			s.TouchExtraPadding = touchExtraPadding.ToSystem();

			s.IndentSpacing = indentSpacing;

			s.ColumnsMinSpacing = columnsMinSpacing;

			s.ScrollbarSize = scrollbarSize;
			s.ScrollbarRounding = scrollbarRounding;
			s.ScrollbarPadding = scrollbarPadding;

			s.GrabMinSize = grabMinSize;
			s.GrabRounding = grabRounding;

			s.LogSliderDeadzone = logSliderDeadzone;
			
			// s.ImageRounding = imageRounding; // added in DearImGui 1.92.6 WIP
			s.ImageBorderSize = imageBorderSize;

			s.TabRounding = tabRounding;
			s.TabBorderSize = tabBorderSize;
			s.TabMinWidthBase = tabMinWidthBase;
			s.TabMinWidthShrink = tabMinWidthShrink;
			s.TabCloseButtonMinWidthSelected = tabCloseButtonMinWidthSelected;
			s.TabCloseButtonMinWidthUnselected = tabCloseButtonMinWidthUnselected;
			s.TabBarBorderSize = tabBarBorderSize;
			s.TabBarOverlineSize = tabBarOverlineSize;

			s.TableAngledHeadersAngle = tableAngledHeadersAngle;
			s.TableAngledHeadersTextAlign = tableAngledHeadersTextAlign.ToSystem();

			s.TreeLinesFlags = treeLinesFlags;
			s.TreeLinesSize = treeLinesSize;
			s.TreeLinesRounding = treeLinesRounding;

			// s.DragDropTargetRounding = dragDropTargetRounding; // added in DearImGui 1.92.5
			// s.DragDropTargetBorderSize = dragDropTargetBorderSize; // ^
			// s.DragDropTargetPadding = dragDropTargetPadding; //       ^

			// s.ColorMarkerSize = colorMarkerSize; // added in DearImGui 1.92.6 WIP
			s.ColorButtonPosition = colorButtonPosition;

			s.ButtonTextAlign = buttonTextAlign.ToSystem();
			s.SelectableTextAlign = selectableTextAlign.ToSystem();

			s.SeparatorTextBorderSize = separatorTextBorderSize;
			s.SeparatorTextAlign = separatorTextAlign.ToSystem();
			s.SeparatorTextPadding = separatorTextPadding.ToSystem();
			
			s.DisplayWindowPadding = displayWindowPadding.ToSystem();
			s.DisplaySafeAreaPadding = displaySafeAreaPadding.ToSystem();

			s.MouseCursorScale = mouseCursorScale;

			s.AntiAliasedLines = antiAliasedLines;
			s.AntiAliasedLinesUseTex = antiAliasedLinesUseTex;
			s.AntiAliasedFill = antiAliasedFill;

			s.CurveTessellationTol = curveTessellationTol;
			s.CircleTessellationMaxError = circleTessellationMaxError;
			
			for (var i = 0; i < s.Colors.Length; i++)
			{
				var colorName = ImGui.GetStyleColorNameS((ImGuiCol)i);
				var color = colors[colorName];
				s.Colors[i] = color.ToSysVec4();
			}
		}

		public void SetFrom(ImGuiStylePtr s)
		{
			alpha = s.Alpha;
			disabledAlpha = s.DisabledAlpha;

			windowPadding = s.WindowPadding.ToUnity();
			windowRounding = s.WindowRounding;
			windowBorderSize = s.WindowBorderSize;
			windowBorderHoverPadding = s.WindowBorderHoverPadding;
			windowMinSize = s.WindowMinSize.ToUnity();
			windowTitleAlign = s.WindowTitleAlign.ToUnity();
			windowMenuButtonPosition = s.WindowMenuButtonPosition;

			childRounding = s.ChildRounding;
			childBorderSize = s.ChildBorderSize;

			popupRounding = s.PopupRounding;
			popupBorderSize = s.PopupBorderSize;

			framePadding = s.FramePadding.ToUnity();
			frameRounding = s.FrameRounding;
			frameBorderSize = s.FrameBorderSize;

			itemSpacing = s.ItemSpacing.ToUnity();
			itemInnerSpacing = s.ItemInnerSpacing.ToUnity();

			cellPadding = s.CellPadding.ToUnity();

			touchExtraPadding = s.TouchExtraPadding.ToUnity();

			indentSpacing = s.IndentSpacing;

			columnsMinSpacing = s.ColumnsMinSpacing;

			scrollbarSize = s.ScrollbarSize;
			scrollbarRounding = s.ScrollbarRounding;
			scrollbarPadding = s.ScrollbarPadding;

			grabMinSize = s.GrabMinSize;
			grabRounding = s.GrabRounding;

			logSliderDeadzone = s.LogSliderDeadzone;

			// imageRounding = s.ImageRounding; // added in DearImGui 1.92.6 WIP
			imageBorderSize = s.ImageBorderSize;

			tabRounding = s.TabRounding;
			tabBorderSize = s.TabBorderSize;
			tabMinWidthBase = s.TabMinWidthBase;
			tabMinWidthShrink = s.TabMinWidthShrink;
			tabCloseButtonMinWidthSelected =  s.TabCloseButtonMinWidthSelected;
			tabCloseButtonMinWidthUnselected = s.TabCloseButtonMinWidthUnselected;
			tabBarBorderSize = s.TabBarBorderSize;
			tabBarOverlineSize = s.TabBarOverlineSize;
			
			tableAngledHeadersAngle = s.TableAngledHeadersAngle;
			tableAngledHeadersTextAlign = s.TableAngledHeadersTextAlign.ToUnity();
			
			treeLinesFlags = s.TreeLinesFlags;
			treeLinesSize = s.TreeLinesSize;
			treeLinesRounding = s.TreeLinesRounding;

			// dragDropTargetRounding = s.DragDropTargetRounding; // added in DearImGui 1.92.5
			// dragDropTargetBorderSize = s.DragDropTargetBorderSize; // ^
			// dragDropTargetPadding = s.DragDropTargetPadding; //       ^

			// colorMarkerSize = s.ColorMarkerSize; // added in DearImGui 1.92.6 WIP
			colorButtonPosition = s.ColorButtonPosition;

			buttonTextAlign = s.ButtonTextAlign.ToUnity();
			selectableTextAlign = s.SelectableTextAlign.ToUnity();

			separatorTextBorderSize = s.SeparatorTextBorderSize;
			separatorTextAlign = s.SeparatorTextAlign.ToUnity();
			separatorTextPadding = s.SeparatorTextPadding.ToUnity();

			displayWindowPadding = s.DisplayWindowPadding.ToUnity();
			displaySafeAreaPadding = s.DisplaySafeAreaPadding.ToUnity();

			mouseCursorScale = s.MouseCursorScale;

			antiAliasedLines = s.AntiAliasedLines;
			antiAliasedLinesUseTex = s.AntiAliasedLinesUseTex;
			antiAliasedFill = s.AntiAliasedFill;

			curveTessellationTol = s.CurveTessellationTol;
			circleTessellationMaxError = s.CircleTessellationMaxError;

			for (var i = 0; i < s.Colors.Length; i++)
			{
				var color = s.Colors[i];
				var colorName = ImGui.GetStyleColorNameS((ImGuiCol)i);
				colors[colorName] = color.ToUnityColor();
			}
		}

		public void SetDefault()
		{
			ImGuiContextPtr context = ImGui.CreateContext();
			ImGui.SetCurrentContext(context);
			SetFrom(ImGui.GetStyle());
			ImGui.DestroyContext(context);
		}
	}
}
