using System.Collections;
using System.Collections.Generic;
using DoorScript;
using UnityEngine;

public class CandleManager : MonoBehaviour
{
    [SerializeField] private Door _door;
    [SerializeField] private DoorBlack _doorBlack;
    public List<Candle> allCandles = new List<Candle>();
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clip;
    
    private int extinguishedCount = 0;
    private void Start()
    {
        StartCoroutine(FindDoorCE());
        
        if (allCandles.Count == 0)
        {
            Candle[] candlesInScene = FindObjectsOfType<Candle>();
            allCandles.AddRange(candlesInScene);
        }
    }

    private IEnumerator FindDoorCE()
    {
        yield return new WaitForSeconds(0.2f);
        
        _door = FindFirstObjectByType<Door>();
        _door.isLocked = true;
    }
    
    public void OnCandleExtinguished(Candle candle)
    {
        extinguishedCount++;

        if (extinguishedCount >= allCandles.Count)
        {
            Debug.Log("All candles extinguished. Unlocking door.");
            audioSource.PlayOneShot(clip);
            if (_door != null)
            {
                _door.isLocked = false;
                _doorBlack.UnlockBlackDoor();
            }
        }
    }

    public void OnCandleRelit(Candle candle)
    {
        extinguishedCount--;

        if (_door != null && _door.isLocked)
        {
            Debug.Log("A candle relit. Keeping the door locked.");
        }
    }
}
