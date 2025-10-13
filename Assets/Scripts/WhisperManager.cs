using System;
using System.IO;
using UnityEngine;

public class WhisperManager : MonoBehaviour
{
    private IntPtr ctx;

    void Start()
    {
        string modelPath = Path.Combine(Application.streamingAssetsPath, "whisper", "ggml-base.en.bin");

        if (!File.Exists(modelPath))
        {
            Debug.LogError("Whisper model not found at: " + modelPath);
            return;
        }

        ctx = WhisperWrapper.whisper_init_from_file(modelPath);
        Debug.Log("Whisper model loaded successfully!");
    }

    void OnDestroy()
    {
        if (ctx != IntPtr.Zero)
        {
            WhisperWrapper.whisper_free(ctx);
            Debug.Log("Whisper resources released.");
        }
    }
}
