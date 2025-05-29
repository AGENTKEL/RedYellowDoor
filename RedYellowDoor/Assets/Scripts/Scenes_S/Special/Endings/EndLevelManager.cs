using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevelManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
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

    public void LoadEndingScene()
    {
        SceneManager.LoadScene("Final");
    }
}
