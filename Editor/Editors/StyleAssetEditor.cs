using Hexa.NET.ImGui;
using UImGui.Assets;
using UnityEditor;
using UnityEngine;

namespace UImGui.Editor
{
	[CustomEditor(typeof(StyleAsset))]
	internal class StyleAssetEditor : UnityEditor.Editor
	{
		private bool _showColors;

		public override void OnInspectorGUI()
		{
			StyleAsset styleAsset = (StyleAsset) target;

			bool hasContext = ImGui.GetCurrentContext() != ImGuiContextPtr.Null;
			if (!hasContext)
			{
				EditorGUILayout.HelpBox("Can't save or apply Style.\n"
					+ "No active ImGui context.", MessageType.Warning, true);
			}

			if (hasContext)
			{
				ImGuiStylePtr style = ImGui.GetStyle();

				GUILayout.BeginHorizontal();
				if (GUILayout.Button("Apply"))
				{
					styleAsset.ApplyTo(style);
				}

				if (GUILayout.Button("Save"))
				{
					bool displayDialog = EditorUtility.DisplayDialog(
						"Save Style",
						"Do you want to save the current style to this asset?",
						"Ok", "Cancel");
					if (displayDialog)
					{
						styleAsset.SetFrom(style);
						EditorUtility.SetDirty(target);
					}
				}
				GUILayout.EndHorizontal();
			}

			DrawDefaultInspector();

			bool changed = false;
			_showColors = EditorGUILayout.Foldout(_showColors, "Colors", true);
			if (_showColors)
			{
				for (int colorId = 0; colorId < (int)ImGuiCol.Count; ++colorId)
				{
					var colorName = ImGui.GetStyleColorNameS((ImGuiCol)colorId);
					if (!styleAsset.colors.TryGetValue(colorName, out Color indexColor))
					{
						ImGuiStyle style = new ImGuiStyle();
						ImGui.StyleColorsDark(ref style);
						indexColor = style.Colors[colorId].ToUnityColor();
						changed = true;
					}
					Color newColor = EditorGUILayout.ColorField(colorName, indexColor);
					changed |= newColor != indexColor;
					styleAsset.colors[colorName] = newColor;
				}
			}

			if (changed)
			{
				EditorUtility.SetDirty(target);
			}
		}
	}
}
