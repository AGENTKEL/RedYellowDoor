using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YellowLevelManager : MonoBehaviour
{
    [Header("List of Yellow Room Prefabs (disabled in scene)")]
    public List<GameObject> yellowRoomPrefabs;

    public GameObject manInBlack;
    public float manInBlackSpawnTime = 40f;
    public AudioSource _audioSource;
    public AudioClip spawnSound;

    private void Start()
    {
        Game_Manager.instance.SetLastScene("Yellow");
        ActivateRandomYellowLevel();
        StartCoroutine(SpawnManInBlack());
    }

    void ActivateRandomYellowLevel()
    {
        int? lastIndex = Game_Manager.instance.lastYellowRoomIndex;

        if (lastIndex.HasValue && lastIndex.Value >= 0 && lastIndex.Value < yellowRoomPrefabs.Count)
        {
            // Re-activate the same level if reloading
            yellowRoomPrefabs[lastIndex.Value].SetActive(true);
            Debug.Log($"Reactivating last Yellow Room prefab index: {lastIndex.Value}");
            return;
        }

        // Pick a new random level
        List<int> availableIndexes = new List<int>();
        for (int i = 0; i < yellowRoomPrefabs.Count; i++)
        {
            if (!Game_Manager.instance.yellowRoomsUsed.Contains(i))
            {
                availableIndexes.Add(i);
            }
        }

        if (availableIndexes.Count == 0)
        {
            Debug.LogWarning("No more unique Yellow levels left. Using fallback (first prefab).");
            yellowRoomPrefabs[0].SetActive(true);
            Game_Manager.instance.lastYellowRoomIndex = 0;
        }
        else
        {
            int randomIndex = availableIndexes[Random.Range(0, availableIndexes.Count)];
            yellowRoomPrefabs[randomIndex].SetActive(true);
            Game_Manager.instance.yellowRoomsUsed.Add(randomIndex);
            Game_Manager.instance.lastYellowRoomIndex = randomIndex; // ✅ Save it
        }

        Game_Manager.instance.AdjustGameObjectsForSoundSettings();
        Game_Manager.instance.AdjustGameObjectsForLocalization();
        Game_Manager.instance.AdjustGameObjectsForSubtitlesSettings();
        StartCoroutine(RefreshSettingsCE());
    }

    private IEnumerator RefreshSettingsCE()
    {
        yield return new WaitForSeconds(1f);
        Game_Manager.instance.AdjustGameObjectsForSoundSettings();
        Game_Manager.instance.AdjustGameObjectsForLocalization();
        Game_Manager.instance.AdjustGameObjectsForSubtitlesSettings();
    }
    
    private IEnumerator SpawnManInBlack()
    {
        yield return new WaitForSeconds(manInBlackSpawnTime);
        manInBlack.SetActive(true);
        _audioSource.PlayOneShot(spawnSound);
    }
}
