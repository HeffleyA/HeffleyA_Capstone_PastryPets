using System;
using System.Runtime.InteropServices;
using UnityEngine;

public static class WhisperWrapper
{
    [DllImport("whisper", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr whisper_init_from_file(string modelPath);

    [DllImport("whisper", CallingConvention = CallingConvention.Cdecl)]
    public static extern void whisper_free(IntPtr ctx);

}
