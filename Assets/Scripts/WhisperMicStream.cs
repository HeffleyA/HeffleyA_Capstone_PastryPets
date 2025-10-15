using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using Whisper;

public class WhisperMicStream : MonoBehaviour
{
    [Header("References")]
    public WhisperManager whisperManager;

    [Header("Mic Settings")]
    public string micDevice;
    public int sampleRate = 16000;
    public int bufferLengthSec = 10;
    public int chunkSize = 16000; // 1 second chunks

    private AudioClip micClip;
    private float[] sampleBuffer;
    private int lastSamplePos = 0;

    private List<float> pendingSamples = new List<float>();
    private bool isRecording = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Microphone.devices.Length == 0)
        {
            Debug.LogError("No microphone devices found!");
            return;
        }

        micDevice = Microphone.devices[0];
        Debug.Log($"Using microphone: {micDevice}");
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Debug.Log("SPACE pressed!");
            if (!isRecording) StartCoroutine(StreamMic());
            else StopMic();
        }
    }

    IEnumerator StreamMic()
    {
        isRecording = true;
        micClip = Microphone.Start(micDevice, true, bufferLengthSec, sampleRate);
        sampleBuffer = new float[micClip.samples * micClip.channels];
        Debug.Log("Microphone started.");

        while (isRecording)
        {
            int currentPos = Microphone.GetPosition(micDevice);
            micClip.GetData(sampleBuffer, 0);

            if (currentPos < lastSamplePos)
            {
                //Looping case
                AddSamples(sampleBuffer, lastSamplePos, sampleBuffer.Length - lastSamplePos);
                AddSamples(sampleBuffer, 0, currentPos);
            }
            else
            {
                AddSamples(sampleBuffer, lastSamplePos, currentPos - lastSamplePos);
            }

            lastSamplePos = currentPos;

            //Process chunks
            while (pendingSamples.Count >= chunkSize)
            {
                float[] chunk = pendingSamples.GetRange(0, chunkSize).ToArray();
                pendingSamples.RemoveRange(0, chunkSize);
                _ = ProcessAudioChunk(chunk);
            }

            yield return null;
        }
    }

    void StopMic()
    {
        isRecording = false;
        Microphone.End(micDevice);
        Debug.Log("Microphone stopped.");
    }

    void AddSamples(float[] src, int start, int length)
    {
        if (length <= 0) return;
        for (int i = 0; i < length; i++)
        {
            pendingSamples.Add(src[start + i]);
        }
    }

    async Task ProcessAudioChunk(float[] chunk)
    {
        try
        {
            var text = await whisperManager.GetTextAsync(micClip);
            Debug.Log($"Transcribed Text: {text}");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error processing audio chunk: {ex.Message}");
        }
    }
}
