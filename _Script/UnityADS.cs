﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class UnityADS : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{

    private string gameId = "2883787";//★ Window > Services 설정 테스트 바꿀것 (test용 1486550) //2883787
    public int soundck;
    public GameObject ad_obj, radio_ani, adBtn_obj;


	int sG,mG;
    int sG2, mG2;
   
    Color color;
    public GameObject Toast_obj;

    //스프라이트 이미지로 변경
    public Sprite radioMove1_spr, radioMove2_spr;

    public int ad_i, adChecker_i;

    //광고준비중
    public GameObject watingAds_obj, watingAdsHelp_obj, watingAdsNoise_obj, watingAdsShow_obj, chAds_obj;
    public Sprite watingAdsNoise_spr1, watingAdsNoise_spr2;
    public Sprite[] watingAdspr;
    int noise_i = 0;
    int rand_i = 0;

    // Use this for initialization
    void Start () {
        color = new Color(1f, 1f, 1f);
        StartCoroutine("radioImageChange");
        StopCoroutine("adTimeFlow2");
        StopCoroutine("adAniTime2");
        StopCoroutine("adTimeFlow");
        StopCoroutine("adAniTime");
        
        if (PlayerPrefs.GetInt("place", 0) == 0)
        {
            StartCoroutine("adTimeFlow");
            StartCoroutine("adAniTime");
        }
        else if(PlayerPrefs.GetInt("outtrip", 0) == 0)
        {
            StartCoroutine("adTimeFlow2");
            StartCoroutine("adAniTime2");
        }
        else if (PlayerPrefs.GetInt("outtrip", 0) == 2)
        {
            StartCoroutine("adTimeFlow2");
            StartCoroutine("adAniTime2");
        }
        else
        {
            StartCoroutine("adTimeFlow");
            StartCoroutine("adAniTime");
        }
        
        if (Advertisement.isSupported)
          {
              Advertisement.Initialize(gameId, false);//꼭 false로 해줄것
          }

        LoadAd();
      }

    // Update is called once per frame

    public void LoadAd()
    {
        Advertisement.Load("rewardedVideo", this);
    }

    public void ShowRewardedAd()
    {
        PlayerPrefs.SetInt("wait",1);
        if (Advertisement.IsReady("rewardedVideo"))
        {
            ShowOptions options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show("rewardedVideo", this);
            //PlayerPrefs.SetInt("secf", 240);
            //광고 20초 시
            StartCoroutine("AdTimeCheck");
        }
        else
        {
            //StartCoroutine("ToastImgFadeOut");
            Wating();
            PlayerPrefs.SetInt("wait", 2);
        }
    }

    public void Wating()
    {
        watingAds_obj.SetActive(true);
        rand_i = Random.Range(0, 15);
        watingAdsShow_obj.GetComponent<Image>().sprite = watingAdspr[rand_i];
        chAds_obj.SetActive(false);
    }

    //광고준비중
    public void WatingAdColse()
    {
        watingAds_obj.SetActive(false);
    }
    public void WatingAdHelp()
    {
        if (watingAdsHelp_obj.activeSelf == true)
        {
            watingAdsHelp_obj.SetActive(false);
        }
        else
        {
            watingAdsHelp_obj.SetActive(true);
        }
    }

    void noise()
    {
        if (noise_i == 0)
        {
            watingAdsNoise_obj.GetComponent<Image>().sprite = watingAdsNoise_spr1;
            noise_i = 1;
        }
        else
        {
            watingAdsNoise_obj.GetComponent<Image>().sprite = watingAdsNoise_spr2;
            noise_i = 0;
        }
    }
    public void WaitAdshow()
    {
        if (PlayerPrefs.GetInt("wait", 0) == 2)
        {
            ad_obj.SetActive(true);
        }
    }



    public void adYN()
    {
        PlayerPrefs.SetInt("adrunout", 0);
        ad_obj.SetActive(true);
        watingAds_obj.SetActive(false);
    }
    public void closeAdYN()
    {
        ad_i = 0;
        adChecker_i = 0;
        StopCoroutine("AdTimeCheck");
        ad_obj.SetActive(false);
    }
    public void adYes()
    {
        ad_i = 0;
        adChecker_i = 0;
        StopCoroutine("AdTimeCheck");
        ShowRewardedAd();
        ad_obj.SetActive(false);
    }


    private void HandleShowResult(ShowResult result)
    {
        ad_i = 0;
        adChecker_i = 0;
        StopCoroutine("AdTimeCheck");
        if (result == ShowResult.Finished)
        {
            GetRewardCall();
        }
    }

    public void Admob()
    {
        radio_ani.SetActive(false);
        adBtn_obj.SetActive(false);
        StopCoroutine("adTimeFlow");
        StopCoroutine("adAniTime");
        StartCoroutine("adTimeFlow");
        StartCoroutine("adAniTime");
        PlayerPrefs.SetInt("talk", 5);
        PlayerPrefs.Save();
        if (PlayerPrefs.GetInt("talk", 5) >= 5)
        {
            PlayerPrefs.SetInt("secf", 240);
        }
    }

    

	IEnumerator adTimeFlow(){
		while (mG>-1) {

			sG = PlayerPrefs.GetInt("secf", 240);
            //Debug.Log("testtime"+sG);
            mG = (int)(sG / 60);
			sG = sG-(sG / 60)*60;
			if (sG < 0) {
				sG = 0;
				mG = 0;
            } else {
                radio_ani.SetActive(false);
                adBtn_obj.SetActive(false);
            }
			sG = PlayerPrefs.GetInt("secf", 240);
			sG = sG - 1;
			if (sG < 0) {
				sG = -1;
			}
			PlayerPrefs.SetInt("secf",sG);
            noise();
			yield return new WaitForSeconds(1f);
            //Debug.Log("sg" + sG);
        }
	}
    IEnumerator adAniTime()
    {
        int w = 0;
        while (w == 0)
        {
            if (sG < 0)
            {
                if (PlayerPrefs.GetInt("outtrip", 0) == 1)
                {
                    radio_ani.SetActive(true);
                    adBtn_obj.SetActive(true);
                }
                else
                {
                    if (PlayerPrefs.GetInt("front", 0) == 1)
                    {
                        radio_ani.SetActive(true);
                        adBtn_obj.SetActive(true);
                    }
                    else
                    {
                        radio_ani.SetActive(false);
                        adBtn_obj.SetActive(false);
                    }
                }
            }
            yield return null;
        }

    }

    //이미지 1초마다 바꿔주기
    IEnumerator radioImageChange()
    {
        int rd = 0;
        int rdc = 0;
        while (rd == 0)
        {
            if (rdc == 0)
            {
                radio_ani.GetComponent<Image>().sprite = radioMove1_spr;
                rdc = 1;
            }
            else
            {
                radio_ani.GetComponent<Image>().sprite = radioMove2_spr;
                rdc = 0;
            }
            yield return new WaitForSeconds(1f);
        }
    }


    IEnumerator adTimeFlow2()
    {
        while (mG2 > -1)
        {
            sG2 = PlayerPrefs.GetInt("secf2", 240);
            //Debug.Log(sG);
            mG2= (int)(sG2 / 60);
            sG2 = sG2 - (sG2 / 60) * 60;
            if (sG2 < 0)
            {
                sG2 = 0;
                mG2 = 0;
            }
            else
            {
                radio_ani.SetActive(false);
                adBtn_obj.SetActive(false);
            }
            sG2 = PlayerPrefs.GetInt("secf2", 240);
            sG2 = sG2 - 1;
            if (sG2 < 0)
            {
                sG2 = -1;
            }
            PlayerPrefs.SetInt("secf2", sG2);
            //Debug.Log("sg2" + sG2);
            noise();
            yield return new WaitForSeconds(1f);
        }
    }
    IEnumerator adAniTime2()
    {
        int w = 0;
        while (w == 0)
        {
            if (sG2 < 0)
            {
                if (PlayerPrefs.GetInt("outtrip", 0) == 1)
                {
                } else if (PlayerPrefs.GetInt("front", 0) == 1)
                    {
                        radio_ani.SetActive(true);
                        adBtn_obj.SetActive(true);
                    }
                    else
                    {
                        radio_ani.SetActive(false);
                        adBtn_obj.SetActive(false);
                    }
                    
            }

            yield return null;
        }

    }



    IEnumerator ToastImgFadeOut()
    {
        color.a = Mathf.Lerp(0f, 1f, 1f);
        Toast_obj.GetComponent<Image>().color = color;
        Toast_obj.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        for (float i = 1f; i > 0f; i -= 0.05f)
        {
            color.a = Mathf.Lerp(0f, 1f, i);
            Toast_obj.GetComponent<Image>().color = color;
            yield return null;
        }
        Toast_obj.SetActive(false);
    }

    /// <summary>
    /// 20초 카운트
    /// </summary>
    /// <returns></returns>
    IEnumerator AdTimeCheck()
    {
        while (ad_i < 21)
        {
            ad_i++;
            yield return new WaitForSeconds(1f);
        }
        adChecker_i = 1;
        ad_i = 0;

        StopCoroutine("AdTimeCheck");
    }
    

    public void OnUnityAdsAdLoaded(string placementId)
    {
        //Debug.Log("테스트트1");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
       // Debug.Log("테스트2");
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        //Debug.Log("테스트3");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        //Debug.Log("테스트4");
    }

    public void OnUnityAdsShowClick(string placementId)
    {
       // Debug.Log("테스트5");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        //닫기가 실행될때 체크
        if (adChecker_i == 1)
        {
            StopCoroutine("AdTimeCheck");
            ad_i = 0;
            adChecker_i = 0;
            GetRewardCall();
            LoadAd();
        }
        else
        {
            StopCoroutine("AdTimeCheck");
            ad_i = 0;
            adChecker_i = 0;

                if (showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
                {
                    GetRewardCall();
                    LoadAd();
                }
            
        }
    }

    void GetRewardCall()
    {
        if (PlayerPrefs.GetInt("place", 0) == 0)
        {
            radio_ani.SetActive(false);
            adBtn_obj.SetActive(false);
            StopCoroutine("adTimeFlow");
            StopCoroutine("adAniTime");
            StartCoroutine("adTimeFlow");
            StartCoroutine("adAniTime");
            PlayerPrefs.SetInt("talk", 5);
            PlayerPrefs.Save();
            if (PlayerPrefs.GetInt("talk", 5) >= 5)
            {
                PlayerPrefs.SetInt("secf", 240);
            }
        }
        else
        {
            PlayerPrefs.SetInt("talk", 5);
            PlayerPrefs.Save();
            if (PlayerPrefs.GetInt("talk", 5) >= 5)
            {
                PlayerPrefs.SetInt("secf2", 240);
            }
        }
    }

}
