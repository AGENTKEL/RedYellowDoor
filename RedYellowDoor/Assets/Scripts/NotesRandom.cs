using System.Collections.Generic;
using UnityEngine;

public class NotesRandom : MonoBehaviour
{
    public List<GameObject> prefabsToActivate;

    private const string UsedPrefabsKey = "UsedPrefabs";
    private HashSet<int> usedPrefabIndices = new HashSet<int>();

    void Start()
    {
        LoadUsedPrefabs();

        int prefabIndex = GetNextRandomIndex();
        if (prefabIndex != -1)
        {
            ActivatePrefab(prefabIndex);
            SaveUsedPrefab(prefabIndex);
        }
    }

    private void ActivatePrefab(int index)
    {
        if (index < 0 || index >= prefabsToActivate.Count)
        {
            Debug.LogWarning("Prefab index out of range.");
            return;
        }

        for (int i = 0; i < prefabsToActivate.Count; i++)
        {
            prefabsToActivate[i].SetActive(i == index);
        }

        Debug.Log($"Activated prefab index: {index}");
    }

    private int GetNextRandomIndex()
    {
        List<int> availableIndices = new List<int>();

        for (int i = 0; i < prefabsToActivate.Count; i++)
        {
            if (!usedPrefabIndices.Contains(i))
                availableIndices.Add(i);
        }

        // If all prefabs were used, reset
        if (availableIndices.Count == 0)
        {
            usedPrefabIndices.Clear();
            SaveUsedPrefabs(); // Reset saved data

            for (int i = 0; i < prefabsToActivate.Count; i++)
            {
                availableIndices.Add(i);
            }
        }

        int randomIndex = availableIndices[Random.Range(0, availableIndices.Count)];
        return randomIndex;
    }

    private void LoadUsedPrefabs()
    {
        string saved = PlayerPrefs.GetString(UsedPrefabsKey, "");
        if (!string.IsNullOrEmpty(saved))
        {
            string[] parts = saved.Split(',');
            foreach (string part in parts)
            {
                if (int.TryParse(part, out int index))
                    usedPrefabIndices.Add(index);
            }
        }
    }

    private void SaveUsedPrefab(int index)
    {
        usedPrefabIndices.Add(index);
        SaveUsedPrefabs();
    }

    private void SaveUsedPrefabs()
    {
        string saveString = string.Join(",", usedPrefabIndices);
        PlayerPrefs.SetString(UsedPrefabsKey, saveString);
        PlayerPrefs.Save();
    }

    // Optional: Reset progress (e.g., via button)
    public void ResetUsedPrefabs()
    {
        PlayerPrefs.DeleteKey(UsedPrefabsKey);
        PlayerPrefs.Save();
    }
}
