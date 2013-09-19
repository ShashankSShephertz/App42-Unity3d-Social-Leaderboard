using UnityEngine;
using System.Collections;
using SimpleJSON;
using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.social;
using com.shephertz.app42.paas.sdk.csharp.game;
using com.shephertz.app42.paas.sdk.csharp.storage;
using System.Security.Cryptography.X509Certificates;
using AssemblyCSharp;


public class AppConstant : MonoBehaviour {
	
	public static string API_KEY = "d794ed6fd8fa49da69e8cb6f3e19ac4a63a22f92d19f1aa7e658ba1d09b645be";
	public static string SECRET_KEY = "3421b54ec141f0a7605662577a6aea355ba3b97f4d7143697888fa606f7a852b";
	public string GameName = "Unity3dLeaderboard";
	public static string DBName = "Knock";
	public static string CollectionName = "LeaderboardCollection";
	private static AppConstant con = null;
	private static string userName;
	public static bool isSaved = false;
	
	public static bool GetSaved(){
		return isSaved;
	}
	
	public static void SetSaved(bool saved){
		isSaved = saved;
	}
	
	public static ServiceAPI GetServce(){
		return new ServiceAPI(API_KEY,SECRET_KEY);
	}
	
	public static ScoreBoardService GetScoreService(ServiceAPI sp){
		return sp.BuildScoreBoardService();
	}
	
	public static StorageService GetStorageService(ServiceAPI sp){
		return sp.BuildStorageService();
	}
	
	public void callBack(string response){
		JObject data = JSON.Parse(response);
		userName = data["name"];
	}
	
	public static string GetUserName(){
		return userName;
	}
	
	public static AppConstant GetInstance ()
		{
			if (con == null) {
				con = (new GameObject ("AppConstant")).AddComponent<AppConstant> ();
				return con;
			} else {
				return con;
			}

		}
	
	 IEnumerator WaitForRequest (string uri)
	{
		IEnumerator e = execute (uri);
		while (e.MoveNext()) {
			yield return e.Current;

		}
		
	}
	
	public string ExecuteGet (string url)
	{
		string responseFromServer = null;
		StartCoroutine (WaitForRequest (url));
		return responseFromServer;
	}
	
	void Awake ()
		{
			// First we check if there are any other instances conflicting
			if (con != null && con != this) {
				// If that is the case, we destroy other instances
				Destroy (gameObject);
			}
 
			// Here we save our singleton instance
			con = this;
 
			// Furthermore we make sure that we don't destroy between scenes (this is optional)
			DontDestroyOnLoad (gameObject);
		}
	
	IEnumerator execute (string url)
	{
		WWW www = new WWW (url);
		while (!www.isDone) {
				
				yield return null;  
			}
			if (www.isDone) {
		   callBack(www.text);
		}
	}
	
	
}
