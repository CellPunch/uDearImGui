using Hexa.NET.ImGui;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UImGui.Assets;
using UImGui.Events;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using Object = UnityEngine.Object;
using UTexture = UnityEngine.Texture;

namespace UImGui.Texture
{
	// TODO: Write documentation for methods
	internal class OldTextureManager
	{
		private Texture2D _atlasTexture;

		private readonly Dictionary<IntPtr, UTexture> _textures = new Dictionary<IntPtr, UTexture>();
		private readonly Dictionary<UTexture, IntPtr> _textureIds = new Dictionary<UTexture, IntPtr>();
		private readonly Dictionary<Sprite, SpriteInfo> _spriteData = new Dictionary<Sprite, SpriteInfo>();

		private readonly HashSet<IntPtr> _allocatedGlyphRangeArrays = new HashSet<IntPtr>();
		public UTexture AtalsTexture => _atlasTexture;

		public unsafe void Initialize(ImGuiIOPtr io)
		{
			ImFontAtlasPtr atlasPtr = io.Fonts;
			// atlasPtr.GetTexDataAsRGBA32(out byte* pixels, out int width, out int height, out int bytesPerPixel); // does not compile
			byte* pixels = null; int width = 0, height = 0; int bytesPerPixel = 0;

			_atlasTexture = new Texture2D(width, height, TextureFormat.RGBA32, true, true)
			{
				name = "IMGUI Atlas Texture",
				filterMode = FilterMode.Bilinear,
				anisoLevel = 8,
			};

			// TODO: Remove collections and make native array manually.
			NativeArray<byte> srcData = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<byte>(pixels, width * height * bytesPerPixel, Allocator.None);
#if ENABLE_UNITY_COLLECTIONS_CHECKS
			NativeArrayUnsafeUtility.SetAtomicSafetyHandle(ref srcData, AtomicSafetyHandle.GetTempMemoryHandle());
#endif
			// Invert y while copying the atlas texture.
			NativeArray<byte> dstData = _atlasTexture.GetRawTextureData<byte>();
			int stride = width * bytesPerPixel;
			for (int y = 0; y < height; ++y)
			{
				NativeArray<byte>.Copy(srcData, y * stride, dstData, (height - y - 1) * stride, stride);
			}

			_atlasTexture.Apply(true);
		}

		public void Shutdown()
		{
			_textures.Clear();
			_textureIds.Clear();
			_spriteData.Clear();

			if (_atlasTexture != null)
			{
				UnityEngine.Object.Destroy(_atlasTexture);
				_atlasTexture = null;
			}
		}

		public void PrepareFrame(ImGuiIOPtr io)
		{
			IntPtr id = RegisterTexture(_atlasTexture);
			// io.Fonts.SetTexID(id); // does not compile
		}

		public bool TryGetTexture(IntPtr id, out UTexture texture)
		{
			return _textures.TryGetValue(id, out texture);
		}

		public IntPtr GetTextureId(UTexture texture)
		{
			return _textureIds.TryGetValue(texture, out IntPtr id) ? id : RegisterTexture(texture);
		}

		public SpriteInfo GetSpriteInfo(Sprite sprite)
		{
			if (!_spriteData.TryGetValue(sprite, out SpriteInfo spriteInfo))
			{
				_spriteData[sprite] = spriteInfo = new SpriteInfo
				{
					Texture = sprite.texture,
					Size = sprite.rect.size,
					UV0 = sprite.uv[0],
					UV1 = sprite.uv[1],
				};
			}

			return spriteInfo;
		}

		private IntPtr RegisterTexture(UTexture texture)
		{
			IntPtr id = texture.GetNativeTexturePtr();
			_textures[id] = texture;
			_textureIds[texture] = id;

			return id;
		}

		public unsafe void BuildFontAtlas(ImGuiIOPtr io, in FontAtlasConfigAsset settings, FontInitializerEvent custom)
		{
			if (io.Fonts.TexIsBuilt)
			{
				DestroyFontAtlas(io);
			}

			if (!io.MouseDrawCursor)
			{
				io.Fonts.Flags |= ImFontAtlasFlags.NoMouseCursors;
			}

			if (settings == null)
			{
				if (custom.GetPersistentEventCount() > 0)
				{
					custom.Invoke(io);
				}
				else
				{
					io.Fonts.AddFontDefault();
				}

				io.FontDefault = io.Fonts.Fonts[0];
				// io.Fonts.Build(); // does not compile
				return;
			}

			// Add fonts from config asset.
			for (int fontIndex = 0; fontIndex < settings.Fonts.Length; fontIndex++)
			{
				FontDefinition fontDefinition = settings.Fonts[fontIndex];
				string fontPath = System.IO.Path.Combine(Application.streamingAssetsPath, fontDefinition.Path);
				if (!System.IO.File.Exists(fontPath))
				{
					Debug.Log($"Font file not found: {fontPath}");
					continue;
				}

				unsafe
				{
					ImFontConfig fontConfig = default;
					ImFontConfigPtr fontConfigPtr = new ImFontConfigPtr(&fontConfig);

					fontDefinition.Config.ApplyTo(fontConfigPtr);
					fontConfigPtr.GlyphRanges = AllocateGlyphRangeArray(fontDefinition.Config);

					// TODO: Add check if is TTF File.
					io.Fonts.AddFontFromFileTTF(fontPath, fontDefinition.Config.SizeInPixels, fontConfigPtr);
				}
			}

			if (io.Fonts.Fonts.Size == 0)
			{
				io.Fonts.AddFontDefault();
			}

			 //io.Fonts.Build(); //does not compile
		}

		public unsafe void DestroyFontAtlas(ImGuiIOPtr io)
		{
			FreeGlyphRangeArrays();

			io.Fonts.Clear(); // Previous FontDefault reference no longer valid.
			io.FontDefault = default; // NULL uses Fonts[0].
		}

		private unsafe uint* AllocateGlyphRangeArray(in FontConfig fontConfig)
		{
			List<uint> values = fontConfig.BuildRanges();
			if (values.Count == 0)
			{
				return null;
			}

			int byteCount = sizeof(uint) * (values.Count + 1); // terminating zero.
			uint* ranges = (uint*)Marshal.AllocHGlobal(byteCount);
			_allocatedGlyphRangeArrays.Add((IntPtr)ranges);

			for (int i = 0; i < values.Count; ++i)
			{
				ranges[i] = values[i];
			}
			ranges[values.Count] = 0;

			return ranges;
		}

		private unsafe void FreeGlyphRangeArrays()
		{
			foreach (IntPtr range in _allocatedGlyphRangeArrays)
			{
				Marshal.FreeHGlobal(range);
			}

			_allocatedGlyphRangeArrays.Clear();
		}
	}

	internal class TextureManager
	{
		private readonly Dictionary<IntPtr, Texture2D> _textures = new Dictionary<IntPtr, Texture2D>();

		public void Initialize(ImGuiIOPtr io, FontAtlasConfigAsset fontAtlasConfiguration, FontInitializerEvent fontCustomInitializer)
		{
			
			if (fontCustomInitializer.GetPersistentEventCount() > 0)
			{
				fontCustomInitializer.Invoke(io);
			}
			else
			{
				unsafe
				{
					io.Fonts.AddFontDefault();
				}
				io.FontDefault = io.Fonts.Fonts[0];
			}
			
			// TODO: maybe implement adding fonts from FontAtlasConfigAsset
			
			if (io.Fonts.Fonts.Size == 0)
			{
				unsafe
				{
					io.Fonts.AddFontDefault();
				}
			}
		}

		public void UpdateTextures(ref ImVector<ImTextureDataPtr> drawDataTextures)
		{
			if (drawDataTextures.Size == 0) return;
			for (int i = 0; i < drawDataTextures.Size; i++)
			{
				UpdateTexture(drawDataTextures[i]);
			}
		}
		
		private void UpdateTexture(ImTextureDataPtr texture)
		{
			switch (texture.Status)
			{
				case ImTextureStatus.Ok:
					break;
				case ImTextureStatus.WantCreate:
					Texture2D texture2D = new Texture2D(texture.Width, texture.Height, TextureFormat.RGBA32, true, true);
					texture.SetTexID(texture2D.GetNativeTexturePtr());
					_textures.Add(texture.TexID, texture2D);
					goto case ImTextureStatus.WantUpdates;
				case ImTextureStatus.WantUpdates:
					texture2D =	_textures[texture.TexID];
					unsafe
					{
						var pixelArray = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<byte>(texture.Pixels, texture.Height * texture.Width * texture.BytesPerPixel, Allocator.None);
                        #if ENABLE_UNITY_COLLECTIONS_CHECKS
						NativeArrayUnsafeUtility.SetAtomicSafetyHandle(ref pixelArray, AtomicSafetyHandle.GetTempMemoryHandle());
						#endif
						var dstTexture = texture2D.GetRawTextureData<byte>();
						int stride = texture.Width * texture.BytesPerPixel;
						for (int y = 0; y < texture.Height; ++y)
						{
							NativeArray<byte>.Copy(pixelArray, y * stride, dstTexture, (texture.Height - y - 1) * stride, stride);
						}
					}
					texture2D.Apply(true);
					texture.SetStatus(ImTextureStatus.Ok);
					break;
				case ImTextureStatus.WantDestroy:
					texture2D = _textures[texture.TexID];
					Object.Destroy(texture2D);
					_textures.Remove(texture.TexID);
					texture.SetStatus(ImTextureStatus.Destroyed);
					texture.SetTexID(ImTextureID.Null);
					break;
			}
		}

		public bool TryGetTexture(ImTextureID texId, out Texture2D texture)
		{
			return _textures.TryGetValue(texId, out texture);
		}

		public void Shutdown()
		{
			/*foreach (var (_, texture2D) in _textures)
			{
				Object.Destroy(texture2D);
			}

			var imguiTextures = ImGui.GetPlatformIO().Textures;
			for (int i = 0; i < imguiTextures.Size; i++)
			{
				imguiTextures[i].SetStatus(ImTextureStatus.Destroyed);
				imguiTextures[i].SetTexID(ImTextureID.Null);
			}
			_textures.Clear();*/
		}
	}
}
