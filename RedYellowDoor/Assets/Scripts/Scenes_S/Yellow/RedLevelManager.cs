using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedLevelManager : MonoBehaviour
{
    [Header("List of Red Room Prefabs (disabled in scene)")]
    public List<GameObject> redRoomPrefabs;

    public GameObject manInBlack;
    public float manInBlackSpawnTime = 40f;
    public AudioSource _audioSource;
    public AudioClip spawnSound;

    private void Start()
    {
        Game_Manager.instance.SetLastScene("Red");
        ActivateRandomRedLevel();
        StartCoroutine(SpawnManInBlack());
    }

    void ActivateRandomRedLevel()
    {
        int? lastIndex = Game_Manager.instance.lastRedRoomIndex;

        if (lastIndex.HasValue && lastIndex.Value >= 0 && lastIndex.Value < redRoomPrefabs.Count)
        {
            redRoomPrefabs[lastIndex.Value].SetActive(true);
            Debug.Log($"Reactivating last Red Room prefab index: {lastIndex.Value}");
            return;
        }

        List<int> availableIndexes = new List<int>();

        for (int i = 0; i < redRoomPrefabs.Count; i++)
        {
            if (!Game_Manager.instance.redRoomsUsed.Contains(i))
            {
                availableIndexes.Add(i);
            }
        }

        if (availableIndexes.Count == 0)
        {
            Debug.LogWarning("No more unique Red levels left. Using fallback (first prefab).");
            redRoomPrefabs[0].SetActive(true);
            Game_Manager.instance.lastRedRoomIndex = 0;
            return;
        }

        int randomIndex = availableIndexes[Random.Range(0, availableIndexes.Count)];
        redRoomPrefabs[randomIndex].SetActive(true);

        Game_Manager.instance.redRoomsUsed.Add(randomIndex);
        Game_Manager.instance.lastRedRoomIndex = randomIndex;

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
