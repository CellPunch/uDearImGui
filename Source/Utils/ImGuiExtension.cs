using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Hexa.NET.ImGui;

namespace UImGui
{
	internal static unsafe class ImGuiExtension
	{
		private static readonly HashSet<IntPtr> _managedAllocations = new HashSet<IntPtr>();

		internal static void SetBackendPlatformName(this ImGuiIOPtr io, string name)
		{
			if (io.BackendPlatformName != (byte*)0)
			{
				if (_managedAllocations.Contains((IntPtr)io.BackendPlatformName))
				{
					Marshal.FreeHGlobal(new IntPtr(io.BackendPlatformName));
				}
				io.BackendPlatformName = (byte*)0;
			}
			if (name != null)
			{
				int byteCount = Encoding.UTF8.GetByteCount(name);
				byte* nativeName = (byte*)Marshal.AllocHGlobal(byteCount + 1);
				int offset = Utils.GetUtf8(name, nativeName, byteCount);

				nativeName[offset] = 0;

				io.BackendPlatformName = nativeName;
				_managedAllocations.Add((IntPtr)nativeName);
			}
		}

		internal static void SetIniFilename(this ImGuiIOPtr io, string name)
		{
			if (io.IniFilename != (byte*)0)
			{
				if (_managedAllocations.Contains((IntPtr)io.IniFilename))
				{
					Marshal.FreeHGlobal((IntPtr)io.IniFilename);
				}
				io.IniFilename = (byte*)0;
			}
			if (name != null)
			{
				int byteCount = Encoding.UTF8.GetByteCount(name);
				byte* nativeName = (byte*)Marshal.AllocHGlobal(byteCount + 1);
				int offset = Utils.GetUtf8(name, nativeName, byteCount);

				nativeName[offset] = 0;

				io.IniFilename = nativeName;
				_managedAllocations.Add((IntPtr)nativeName);
			}
		}

		public static void SetBackendRendererName(this ImGuiIOPtr io, string name)
		{
			if (io.BackendRendererName != (byte*)0)
			{
				if (_managedAllocations.Contains((IntPtr)io.BackendRendererName))
				{
					Marshal.FreeHGlobal((IntPtr)io.BackendRendererName);
					io.BackendRendererName = (byte*)0;
				}
			}
			if (name != null)
			{
				int byteCount = Encoding.UTF8.GetByteCount(name);
				byte* nativeName = (byte*)Marshal.AllocHGlobal(byteCount + 1);
				int offset = Utils.GetUtf8(name, nativeName, byteCount);

				nativeName[offset] = 0;

				io.BackendRendererName = nativeName;
				_managedAllocations.Add((IntPtr)nativeName);
			}
		}
	}
}