using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampManager : MonoBehaviour
{
    public List<Lamp> lamps = new List<Lamp>();
    public float breakInterval = 2f; // time between breaking lamps

    private int currentLampIndex = 0;

    void Start()
    {
        StartCoroutine(BreakLampsRoutine());
    }

    IEnumerator BreakLampsRoutine()
    {
        yield return new WaitForSeconds(breakInterval); // wait before breaking the first lamp

        while (currentLampIndex < lamps.Count)
        {
            lamps[currentLampIndex].Break();
            currentLampIndex++;

            yield return new WaitForSeconds(breakInterval);
        }
    }
}
