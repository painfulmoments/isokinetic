// SettingsController.cs
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using ZTools;

public class SettingsController : MonoBehaviour
{
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private AudioMixer audioMixer;

    private const string VOLUME_KEY = "MasterVolume";

    private void Awake()
    {
        // 确保 Slider 已绑定回调
        volumeSlider.onValueChanged.RemoveAllListeners();
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    private void Start()
    {
        // 如果第一次运行，没有存过值，就初始化为 1 并保存
        float savedVol;
        if (!PlayerPrefs.HasKey(VOLUME_KEY))
        {
            savedVol = 1f;
            PlayerPrefs.SetFloat(VOLUME_KEY, savedVol);
            PlayerPrefs.Save();
            ZLog.Log("SettingsController: No saved volume, default to 1");
        }
        else
        {
            savedVol = PlayerPrefs.GetFloat(VOLUME_KEY);
            ZLog.Log("SettingsController: Loaded saved volume " + savedVol);
        }

        // 给 Slider 赋值时不触发回调
        volumeSlider.onValueChanged.RemoveAllListeners();
        volumeSlider.value = savedVol;
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);

        ApplyVolume(savedVol);
    }

    public void OnVolumeChanged(float volume)
    {
        ZLog.Log("SettingsController: OnVolumeChanged -> " + volume);
        ApplyVolume(volume);
        PlayerPrefs.SetFloat(VOLUME_KEY, volume);
        PlayerPrefs.Save();
    }

    private void ApplyVolume(float volume)
    {
        // 1. AudioMixer 中的 Volume 参数（dB）
        float dB = (volume <= 0.0001f) ? -80f : Mathf.Log10(volume) * 20f;
        if (!audioMixer.SetFloat("Volume", dB))
            Debug.LogWarning("SettingsController: Mixer parameter 'Volume' not found");

        // 2. 全局 AudioListener 音量
        AudioListener.volume = volume;

        ZLog.Log($"SettingsController: Applied volume. Mixer dB={dB}, Listener={AudioListener.volume}");
    }

    public void OnBackClicked()
    {
        ZLog.Log("SettingsController: OnBackClicked");
        settingsPanel.SetActive(false);
    }
}
