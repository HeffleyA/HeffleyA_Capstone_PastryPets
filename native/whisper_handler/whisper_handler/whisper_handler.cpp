
#include <string>
#include <mutex>
#include <cstdlib>

// Global whisper context
static struct whisper_context* g_ctx = nullptr;
static std::mutex g_mutex;

extern "C"
{
    #include "whisper.h"
    #include "ggml.h"

    __declspec(dllexport) bool whisper_init_handler(const char* model_path)
    {
        std::lock_guard<std::mutex> lock(g_mutex);
        if (g_ctx != nullptr) return true;

        g_ctx = whisper_init_from_file(model_path);
        return g_ctx != nullptr;
    }

    __declspec(dllexport) const char* whisper_transcribe(const float* samples, int num_samples)
    {
        std::lock_guard<std::mutex> lock(g_mutex);

        if (!g_ctx)
            return strdup("[error: model not loaded]");

        whisper_full_params params = whisper_full_default_params(WHISPER_SAMPLING_GREEDY);
        params.print_progress = false;
        params.print_realtime = false;
        params.print_timestamps = false;

        if (whisper_full(g_ctx, params, samples, num_samples) != 0)
        {
            return strdup("[error: transcription failed]");
        }

        std::string result;
        int n_segments = whisper_full_n_segments(g_ctx);
        for (int i = 0; i < n_segments; ++i)
        {
            result += whisper_full_get_segment_text(g_ctx, i);
        }

        return strdup(result.c_str());
    }

    __declspec(dllexport) void whisper_free_string(const char* ptr)
    {
        free((void*)ptr);
    }

    __declspec(dllexport) void whisper_cleanup()
    {
        std::lock_guard<std::mutex> lock(g_mutex);
        if (g_ctx)
        {
            whisper_free(g_ctx);
            g_ctx = nullptr;
        }
    }
}
