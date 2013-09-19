using UnityEngine;
using UnityEngine.SocialPlatforms;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System;
using System.Net.Security;
using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.social;
using com.shephertz.app42.paas.sdk.csharp.game;
using com.shephertz.app42.paas.sdk.csharp.storage;
using System.Security.Cryptography.X509Certificates;
using SimpleJSON;
using AssemblyCSharp;
using System.Threading;



public class App42Console : MonoBehaviour,App42CallBack {
	
	// Use this for initialization
	public static List<object> fList = new List<object> ();
	Dictionary<string ,object> mDict = new Dictionary<string, object>();
	ServiceAPI sp =null;
	ScoreBoardService scoreService = null;
	StorageService storageService = null;
	string text= null;
	private bool isLoded= false;
	SaveCallback callback = new SaveCallback();
	AppConstant constants = new AppConstant();
	public string name;
		
	void Start () {
	
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
	void OnGUI()
    {
		if(AppConstant.GetSaved()){
			AppConstant.SetSaved(false);
			
			SocialConnectWithApp42(FB.UserId,"");
			fList = new List<object>();
			
		}
	}
	
	public void SaveUserScore(string userId, string score)
	{
		
		storageService.FindDocumentByKeyValue(AppConstant.DBName, AppConstant.CollectionName,"userName",AppConstant.GetUserName(),callback);
		scoreService.SaveUserScore(constants.GameName, userId, Convert.ToDouble(score), callback);
	}
	
	public void SocialConnectWithApp42(string userId, string fbAccessToken)
	{
	 sp = AppConstant.GetServce();
	 scoreService = AppConstant.GetScoreService(sp);
	 storageService = AppConstant.GetStorageService(sp);
	 storageService.FindAllDocuments(AppConstant.DBName,AppConstant.CollectionName,this);
	 AppConstant.GetInstance().ExecuteGet("https://graph.facebook.com/"+userId);	
	}
	
	public void OnSuccess (object response)
	{
		if (response is Game){
		Game gameObj = (Game)response;
		IList<Game.Score> scoreList = gameObj.GetScoreList();
			for (int i=0 ;i< scoreList.Count;i++){
			if(mDict.ContainsKey(scoreList[i].GetUserName())){
				string str = scoreList[i].GetUserName();
				Dictionary<string ,object> findObj = (Dictionary<string ,object>)mDict[str];
				IList<string> list = new List<string>();
				string rank = (i+1).ToString();
				string userName = findObj["userName"].ToString();
				list.Add(rank);
				list.Add(userName);
				list.Add(scoreList[i].GetValue().ToString());
				fList.Add(list);
			}
		}
		}
		if(response is Storage){
			scoreService.GetTopNRankers(constants.GameName,10,this);
			Storage storageObj = (Storage)response;
			IList<Storage.JSONDocument> docs = storageObj.GetJsonDocList();
			for (int i=0 ;i< docs.Count;i++){
				JObject jObj = JSON.Parse(docs[i].GetJsonDoc());
				Dictionary<string ,object> dict = new Dictionary<string, object>();
				string uName = jObj["userName"];
				dict.Add("userId",jObj["userId"]);
				dict.Add("userName",uName);
				mDict.Add(jObj["userId"], dict);
				
			}
		}
		
		
	}
	
	public static IList<object> GetFList(){
		return fList;
	}
	
	public void OnException (Exception e)
	{
			Debug.Log("Exception #####"+e.ToString());
	}
}
