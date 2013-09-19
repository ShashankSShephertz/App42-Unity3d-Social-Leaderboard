using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SimpleJSON;
using System.Text.RegularExpressions;

public sealed class InteractiveConsole : MonoBehaviour
{
    #region FB.Init() example

    private bool isInit = false;
	private bool app42 = true;
	private string fbResponse = null;
	private string userName = null;
	List<GUIContent> items = new List<GUIContent> ();
	App42Console app42Connect = new App42Console();
	public string score;
    Vector2 scrollVector;
	private bool save = false;
	private string loadingData = "Click Connect ...";
	
    private void CallFBInit()
    {
        FB.Init(OnInitComplete, OnHideUnity);
	}

    private void OnInitComplete()
    {
         isInit = true;
		GUI.enabled = isInit && !FB.IsLoggedIn;
        CallFBLogin();
		save = true;
		status = "Login called";
		loadingData = "Loading Data Please Wait ...";
      
    }
	
    private void OnHideUnity(bool isGameShown)
    {
        Debug.Log("Is game showing? " + isGameShown);
    }
	
//	void AuthChangeDelegate(){
//		if(FB.IsLoggedIn) {
//		  string url = "https://graph.facebook.com/696700173";
//		  WWW www = new WWW(url);
//		  yield return www;
//		  fbResponse = www.text;
//		}
//	}

    #endregion

    #region FB.Login() example

    private void CallFBLogin()
    {
        FB.Login("email");
    }

    #endregion

//	#region FB.Canvas.Pay() example
//
//    public string PayProduct = "";
//
//    private void CallFBPay()
//    {
//        FB.Canvas.Pay(PayProduct);
//    }
//
//    #endregion

//    #region FB.API() example
//
//    public string ApiQuery = "";
//
//    private void CallFBAPI()
//    {
//        FB.API(ApiQuery, Facebook.HttpMethod.GET, Callback, null);
//    }
//
//    #endregion

    #region GUI

    private string status = "Ready";
    private string rank,name,scoreValue;

    private string lastResponse = "";
    public GUIStyle textStyle = new GUIStyle();
	
    private Vector2 scrollPosition = Vector2.zero;
#if UNITY_IOS || UNITY_ANDROID
    int buttonHeight = 60;
    int mainWindowWidth = 610;
    int mainWindowFullWidth = 640;
#else
    int buttonHeight = 24;
    int mainWindowWidth = 500;
    int mainWindowFullWidth = 750;
#endif

    private int TextWindowHeight
    {
        get
        {
#if UNITY_IOS || UNITY_ANDROID
            return IsHorizontalLayout() ? Screen.height : 20;
#else
        return Screen.height;
#endif

        }
    }

    void Awake()
    {
        textStyle.alignment = TextAnchor.UpperLeft;
        textStyle.wordWrap = true;
       // textStyle.padding = new RectOffset(10, 10, 10, 10);
        textStyle.stretchHeight = true;
        textStyle.stretchWidth = false;
	}
	
	void OnGUI()
    {
        
		var nxtLine = System.Environment.NewLine;
		
		if (IsHorizontalLayout())
        {
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
        }

        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.MinWidth(mainWindowFullWidth));
        GUILayout.BeginVertical();
        GUI.enabled = !isInit;
        if (Button("Connect"))
        {
            CallFBInit();
        }


        GUI.enabled = FB.IsLoggedIn;
        GUILayout.BeginHorizontal();

        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
        GUILayout.EndScrollView();

        if (IsHorizontalLayout())
        {
            GUILayout.EndVertical();
        }
        GUI.enabled = true;
       
		if(FB.AccessToken !="" && app42){
		    app42 = false;
			app42Connect.SocialConnectWithApp42(FB.UserId,FB.AccessToken);
		}

        if (IsHorizontalLayout())
        {
            GUILayout.EndHorizontal();
        }
		
		GUILayout.BeginHorizontal(GUI.skin.box);
		
        scrollVector = GUILayout.BeginScrollView(scrollVector, GUILayout.Height (300));
		GUI.enabled = isInit;
		GUILayout.BeginHorizontal();
		GUILayout.Space(130);
		GUILayout.Label(new GUIContent("Rank"));
		GUILayout.Label(new GUIContent("Name"));
		GUILayout.Label(new GUIContent("Score"));
		GUILayout.EndHorizontal();
		if(App42Console.GetFList().Count() !=0){
		    for (int i = 0; i < App42Console.GetFList().Count(); i++)
		
		    {   
				IList<string> list = (IList<string>)App42Console.GetFList()[i];
				GUILayout.BeginHorizontal();
				GUILayout.Space(140);
				GUILayout.Label(new GUIContent(list[0]),GUILayout.MaxWidth(200));
				GUILayout.Space(-30);
				GUILayout.Label(new GUIContent(list[1]),GUILayout.MaxWidth(200));
				GUILayout.Space(30);
				GUILayout.Label(new GUIContent(list[2]),GUILayout.MaxWidth(200));	
				//GUILayout.Space(30);
				GUILayout.EndHorizontal();
				
		        if (GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
		
		        {
		
		            // Handle events here
		
		        }
		
			}
		}else{
			GUILayout.Space(100);
			GUILayout.BeginHorizontal();
			GUILayout.Space(310);
			GUILayout.Label(new GUIContent(loadingData));	
			GUILayout.EndHorizontal();
		}

	    GUILayout.EndScrollView();
		GUILayout.EndHorizontal();
		score = GUILayout.TextField(score,4,GUILayout.MinHeight(buttonHeight));
		score = Regex.Replace(score, @"[^0-9]", "");
		GUI.enabled = save;
		if(GUILayout.Button("Save Your Score", GUILayout.MinHeight(buttonHeight))){
			app42Connect.SaveUserScore(FB.UserId,score);
			save = false;
		}
				
    }

   private bool Button(string label)
    {
        return GUILayout.Button(label, GUILayout.MinHeight(buttonHeight));
    }

    private void LabelAndTextField(string label, ref string text)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(label, GUILayout.Width(150));
        text = GUILayout.TextField(text, GUILayout.MinWidth(300));
        GUILayout.EndHorizontal();
    }

    private bool IsHorizontalLayout()
    {
#if UNITY_IOS || UNITY_ANDROID
        return Screen.orientation == ScreenOrientation.Landscape;
#else
        return true;
#endif
    }

    #endregion
}
