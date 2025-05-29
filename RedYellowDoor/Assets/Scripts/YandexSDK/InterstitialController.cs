using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YandexMobileAds;
using YandexMobileAds.Base;

public class InterstitialController : MonoBehaviour
{
    private InterstitialAdLoader interstitialAdLoader;
    private Interstitial interstitial;
    private Button button;
    
    private int roomsPassed = 0;
    private Action onAdClosedCallback;

    private void Awake()
    {
        SetupLoader();
        RequestInterstitial();
        DontDestroyOnLoad(gameObject);
    }

    private void SetupLoader()
    {
        interstitialAdLoader = new InterstitialAdLoader();
        interstitialAdLoader.OnAdLoaded += HandleInterstitialLoaded;
        interstitialAdLoader.OnAdFailedToLoad += HandleInterstitialFailedToLoad;
    }

    private void RequestInterstitial()
    {
        string adUnitId = "R-M-15261148-2"; // замените на "R-M-XXXXXX-Y"
        AdRequestConfiguration adRequestConfiguration = new AdRequestConfiguration.Builder(adUnitId).Build();
        interstitialAdLoader.LoadAd(adRequestConfiguration);
    }

    private void ShowInterstitial()
    {
        if (interstitial != null)
        {
            interstitial.Show();
        }
    }

    public void HandleInterstitialLoaded(object sender, InterstitialAdLoadedEventArgs args)
    {
        // The ad was loaded successfully. Now you can handle it.
        interstitial = args.Interstitial;

        // Add events handlers for ad actions
        interstitial.OnAdClicked += HandleAdClicked;
        interstitial.OnAdShown += HandleInterstitialShown;
        interstitial.OnAdFailedToShow += HandleInterstitialFailedToShow;
        interstitial.OnAdImpression += HandleImpression;
        interstitial.OnAdDismissed += HandleInterstitialDismissed;
    }

    public void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        // Ad {args.AdUnitId} failed for to load with {args.Message}
        // Attempting to load a new ad from the OnAdFailedToLoad event is strongly discouraged.
    }

    public void HandleInterstitialDismissed(object sender, EventArgs args)
    {
        DestroyInterstitial();
        RequestInterstitial();
    }

    public void HandleInterstitialFailedToShow(object sender, EventArgs args)
    {
        DestroyInterstitial();
        RequestInterstitial();

        // Continue even if ad failed
        onAdClosedCallback?.Invoke();
        onAdClosedCallback = null;
    }

    public void HandleAdClicked(object sender, EventArgs args)
    {
        // Called when a click is recorded for an ad.
    }

    public void HandleInterstitialShown(object sender, EventArgs args)
    {
        // Called when ad is shown.
    }

    public void HandleImpression(object sender, ImpressionData impressionData)
    {
        // Called when an impression is recorded for an ad.
    }

    public void DestroyInterstitial()
    {
        if (interstitial != null)
        {
            interstitial.Destroy();
            interstitial = null;
        }
    }
    
    public void OnRoomPassed(Action onNextLevelLoad)
    {
        roomsPassed++;

        if (roomsPassed >= 3)
        {
            roomsPassed = 0; // Reset counter
            if (interstitial != null)
            {
                ShowInterstitial();
            }
        }
    }
}
