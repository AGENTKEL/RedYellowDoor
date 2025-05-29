using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candle : MonoBehaviour
{
    private bool isExtinguished = false;
    [SerializeField] private GameObject candleEffect;
    [SerializeField] private CandleManager candleManager;
    [SerializeField] private float candleRelitTime = 15f;

    public void Interact()
    {
        if (!isExtinguished)
        {
            StartCoroutine(ExtinguishAndRelight());
        }
    }

    private IEnumerator ExtinguishAndRelight()
    {
        isExtinguished = true;
        candleEffect.SetActive(false);

        if (candleManager != null)
        {
            candleManager.OnCandleExtinguished(this);
        }

        yield return new WaitForSeconds(candleRelitTime);

        candleEffect.SetActive(true);
        isExtinguished = false;

        if (candleManager != null)
        {
            candleManager.OnCandleRelit(this);
        }
    }
}
