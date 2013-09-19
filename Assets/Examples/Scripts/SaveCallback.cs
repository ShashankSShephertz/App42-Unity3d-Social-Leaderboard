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

public class SaveCallback : MonoBehaviour,App42CallBack {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void OnSuccess (object response)
	{
		AppConstant.SetSaved(true);
	}
	
	public void OnException (Exception e)
	{
		ServiceAPI sp = AppConstant.GetServce();
		JSONClass json = new JSONClass();
		json.Add("userId",FB.UserId);
		json.Add("userName",AppConstant.GetUserName());
		AppConstant.GetStorageService(sp).InsertJSONDocument(AppConstant.DBName, AppConstant.CollectionName,json,this);
		
	}
}
