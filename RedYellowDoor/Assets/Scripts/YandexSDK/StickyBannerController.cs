using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YandexMobileAds;
using YandexMobileAds.Base;

public class StickyBannerController : MonoBehaviour
{
    private Banner banner;

    private int GetScreenWidthDp()
    {
        int screenWidth = (int)Screen.safeArea.width;
        return ScreenUtils.ConvertPixelsToDp(screenWidth);
    }

    private void Awake()
    {
        RequestStickyBanner();
    }

    private void RequestStickyBanner()
    {   
        string adUnitId = "R-M-15261148-1"; // замените на "R-M-XXXXXX-Y"
        BannerAdSize bannerMaxSize = BannerAdSize.StickySize(GetScreenWidthDp());
        banner = new Banner(adUnitId, bannerMaxSize, AdPosition.BottomCenter);
        
        AdRequest request = new AdRequest.Builder().Build();
        banner.LoadAd(request);
        
        // Вызывается, когда реклама с вознаграждением была загружена
        banner.OnAdLoaded += HandleAdLoaded;

        // Вызывается, если во время загрузки произошла ошибка
        banner.OnAdFailedToLoad += HandleAdFailedToLoad;
    
        // Вызывается, когда приложение становится неактивным, так как пользователь кликнул на рекламу и сейчас перейдет в другое приложение (например, браузер).
        banner.OnLeftApplication += HandleLeftApplication;

        // Вызывается, когда пользователь возвращается в приложение после клика
        banner.OnReturnedToApplication += HandleReturnedToApplication;
    
        // Вызывается, когда пользователь кликнул на рекламу
        banner.OnAdClicked += HandleAdClicked;
    
        // Вызывается, когда зарегистрирован показ
        banner.OnImpression += HandleImpression;
    }
    
    private void HandleAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("AdLoaded event received");
        banner.Show();
    }

    private void HandleAdFailedToLoad(object sender, AdFailureEventArgs args)
    {
        Debug.Log($"AdFailedToLoad event received with message: {args.Message}");
        // Настоятельно не рекомендуется пытаться загрузить новое объявление с помощью этого метода
    }

    private void HandleLeftApplication(object sender, EventArgs args)
    {
        Debug.Log("LeftApplication event received");
    }

    private void HandleReturnedToApplication(object sender, EventArgs args)
    {
        Debug.Log("ReturnedToApplication event received");
    }

    private void HandleAdClicked(object sender, EventArgs args)
    {
        Debug.Log("AdClicked event received");
    }

    private void HandleImpression(object sender, ImpressionData impressionData)
    {
        var data = impressionData == null ? "null" : impressionData.rawData;
        Debug.Log($"HandleImpression event received with data: {data}");
    }
}
