using UnityEngine;
using System.Collections;

public class BrowserOpener : MonoBehaviour
{
    public GameObject mall;
    public GameObject favcartCounter;
    public string pageToOpen = "https://www.google.com";

    bool IsInAppBrowserOpened = true;
    // check readme file to find out how to change title, colors etc.
    public void reopenBrowser(string url) {
        pageToOpen = url;

      
        InAppBrowser.DisplayOptions options = new InAppBrowser.DisplayOptions();
        options.displayURLAsPageTitle = false;
        options.hidesTopBar = false;
       
        options.backButtonText = "رجوع";
        InAppBrowser.OpenURL(pageToOpen, options);
        IsInAppBrowserOpened = true;
    }

	public void OnClearCacheClicked() {
		InAppBrowser.ClearCache();
    }
    public void close()
    {
        mall.SetActive(true);
        IsInAppBrowserOpened = false;
        favcartCounter.SetActive(false);
        favcartCounter.SetActive(true);
        print("close browser");
        InAppBrowser.CloseBrowser();
    }
}
