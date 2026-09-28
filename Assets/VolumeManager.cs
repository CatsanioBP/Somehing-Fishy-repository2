using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeManager : MonoBehaviour
{
    public static VolumeManager Instance;

    [Header("Audio Sources")]
    [SerializeField] AudioSource soundSource; // For SFX
    [SerializeField] AudioSource fishingSource; 
    [SerializeField] AudioSource shopMusicSource;
    [SerializeField] AudioSource shopAmbientSource;

    [Header("Audio Clips")]
    [SerializeField] AudioClip FishingStartMusic;
    [SerializeField] AudioClip FishingLoopMusic;
    [SerializeField] AudioClip ShopAmbient;
    [SerializeField] AudioClip ShopSoundtreck;

    [SerializeField] float musicFadeTime = 2f;
    Coroutine currentMusicTransition;

    void Awake()
    {
        // Setup Singleton in Awake so it's ready before other scripts call Start()
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartFishingMusic()
    {
        StartCoroutine(StartFishingMusicRoutine());
    }

    IEnumerator StartFishingMusicRoutine()
    {
        fishingSource.Stop();

        fishingSource.clip = FishingStartMusic;
        fishingSource.loop = false;
        fishingSource.volume = 1f;
        fishingSource.pitch = 1f;

        fishingSource.Play();

        yield return new WaitForSeconds(FishingStartMusic.length);

        fishingSource.clip = FishingLoopMusic;
        fishingSource.loop = true;
        fishingSource.Play();
    }

    public void EnterShopMusic()
    {
        if (currentMusicTransition != null)
            StopCoroutine(currentMusicTransition);

        currentMusicTransition = StartCoroutine(FadeToShop());
    }

    IEnumerator FadeToShop()
    {
        shopMusicSource.clip = ShopSoundtreck;
        shopAmbientSource.clip = ShopAmbient;

        shopMusicSource.loop = true;
        shopAmbientSource.loop = true;

        shopMusicSource.volume = 0;
        shopAmbientSource.volume = 0;

        shopMusicSource.Play();
        shopAmbientSource.Play();

        float timer = 0;
        float fishStartVol = fishingSource.volume;
        float fishStartPitch = fishingSource.pitch;

        while (timer < musicFadeTime)
        {
            timer += Time.deltaTime;
            float t = timer / musicFadeTime;

            fishingSource.volume = Mathf.Lerp(fishStartVol, 0f, t);
            fishingSource.pitch = Mathf.Lerp(fishStartPitch, 0.5f, t);

            shopMusicSource.volume = Mathf.Lerp(0f, 1f, t);
            shopAmbientSource.volume = Mathf.Lerp(0f, 1f, t);

            yield return null;
        }

        fishingSource.Stop();
        fishingSource.volume = 1f;
        fishingSource.pitch = 1f;
    }

    public void LeaveShopMusic()
    {
        if (currentMusicTransition != null)
            StopCoroutine(currentMusicTransition);

        currentMusicTransition = StartCoroutine(FadeToFishing());
    }

    IEnumerator FadeToFishing()
    {
        fishingSource.clip = FishingLoopMusic;
        fishingSource.loop = true;

        fishingSource.volume = 0;
        fishingSource.pitch = 0.5f;

        fishingSource.Play();

        float timer = 0;

        while (timer < musicFadeTime)
        {
            timer += Time.deltaTime;
            float t = timer / musicFadeTime;

            fishingSource.volume = Mathf.Lerp(0f, 1f, t);
            fishingSource.pitch = Mathf.Lerp(0.5f, 1f, t);

            shopMusicSource.volume = Mathf.Lerp(1f, 0f, t);
            shopAmbientSource.volume = Mathf.Lerp(1f, 0f, t);

            yield return null;
        }

        shopMusicSource.Stop();
        shopAmbientSource.Stop();

        shopMusicSource.volume = 1f;
        shopAmbientSource.volume = 1f;
    }

    public void PlaySoundClip(AudioClip clip, Transform spawnTransform, float volume)
    {
        //spawn in gameObject
        AudioSource audioSource = Instantiate(soundSource, spawnTransform.position, Quaternion.identity);

        //assign the audioClip
        audioSource.clip = clip;

        //assign volume
        audioSource.volume = volume;

        //play sound
        audioSource.Play();

        //get length of sound FX clip
        float clipLength = audioSource.clip.length;

        // Optional: Destroy the temporary audio source after it finishes playing
        Destroy(audioSource.gameObject, clipLength);
    }
}

