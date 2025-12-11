using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using Whisper;

public class WhisperMicStream : MonoBehaviour
{
    [SerializeField]
    public BattleManager battleManager;
    [SerializeField]
    public GameObject whisperPanel;

    System.Random random = new System.Random();

    [Header("References")]
    public WhisperManager whisperManager;

    [Header("Mic Settings")]
    public string micDevice;
    public int sampleRate = 16000;
    public int recordDurationSec = 2;

    private bool isRecording = false;
    private AudioClip micClip;

    private void Awake()
    {
        whisperPanel.SetActive(true);

        if (Microphone.devices.Length == 0)
        {
            Debug.LogError("No microphone devices found!");
            return;
        }

        micDevice = Microphone.devices[0];
        Debug.Log($"Using microphone: {micDevice}");
    }

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            if (!isRecording)
                StartCoroutine(RecordAndTranscribe());
        }
    }

    IEnumerator RecordAndTranscribe()
    {
        whisperPanel.SetActive(false);

        isRecording = true;
        Debug.Log("Starting 2-second recording...");

        // Start recording for 2 seconds
        micClip = Microphone.Start(micDevice, false, recordDurationSec, sampleRate);

        // Wait until recording finishes
        yield return new WaitForSeconds(recordDurationSec);

        // Stop the mic and transcribe
        Microphone.End(micDevice);
        Debug.Log("Recording complete. Transcribing...");

        yield return TranscribeClip(micClip);

        isRecording = false;

    }


    private async Task TranscribeClip(AudioClip clip)
    {
        Debug.Log("Recording complete. Transcribing...");

        //Debug.Log("Before TextAsync");
        var result = await whisperManager.GetTextAsync(clip);
        //Debug.Log("After TextAsync");
        if (result == null)
        {
            Debug.LogWarning("[DEBUG] Transcription result was null.");
            return;
        }

        // Combine all segment texts into one string
        string finalText = string.Join(" ", result.Segments.Select(s => s.Text));
        Debug.Log($"Transcribed Text: {finalText}");

        // Clean the text for easier matching
        string cleanedText = new string(finalText
            .Where(c => !char.IsPunctuation(c) && !char.IsWhiteSpace(c))
            .ToArray())
            .ToLower();

        Debug.Log($"[DEBUG] Cleaned text: {cleanedText}");

        switch (cleanedText)
        {
            case "attack":
                battleManager.ownedPet.isAttacking = true;
                StartCoroutine(battleManager.RunTurn());
                whisperPanel.SetActive(true);
                return;
            case "defend":
                battleManager.ownedPet.isDefending = true;
                StartCoroutine(battleManager.RunTurn());
                whisperPanel.SetActive(true);
                return;
            case "dodge":
                battleManager.ownedPet.isDodging = true;
                StartCoroutine(battleManager.RunTurn());
                whisperPanel.SetActive(true);
                return;
            case "switch":
                battleManager.team.SaveMembers();
                Debug.Log("Team Members Saved Successfully");
                battleManager.SwitchMember();
                StartCoroutine(battleManager.RunTurn());
                whisperPanel.SetActive(true);
                return;
            default:
                whisperPanel.SetActive(true);
                return;
        }
    }

}
