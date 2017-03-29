using UnityEngine;
using System.Collections;
using LitJson;
using System.Collections.Generic;
using System.IO;
public class JsonTest : MonoBehaviour {
    string Path;
	// Use this for initialization
	void Start () {
        Path = Application.dataPath+"/Assets_Json.txt";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    [SerializeField]
    List<AssetManager> list;
    [ContextMenu("生成Json")]
    private void WriteJson() 
    {
        StartCoroutine(CreatJson());
    }
    private IEnumerator CreatJson()
    {
        Asset asset1 = new Asset();
        asset1.Path = "Assetbundle/fangzi.assetbudle";
        asset1.AssetsName = "fangzi";
        Asset asset2 = new Asset();
        asset2.Path = "Assetbundle/004_360-panoramas.assetbudle";
        asset2.AssetsName = "004_360-panoramas";

        AssetManager manager1 = new AssetManager(asset1);
        AssetManager manager2 = new AssetManager(asset2);
       list= new List<AssetManager> { manager1,manager2};
        string jsonstr = JsonMapper.ToJson(list);
        yield return jsonstr;
        Debug.Log(jsonstr);
        byte[] data = System.Text.Encoding.UTF8.GetBytes(jsonstr);
        yield return data;
        File.WriteAllBytes(Path,data);
    }

    [ContextMenu("获取Json内容")]
    private void GetJson() 
    {
        string json = File.ReadAllText(Path);
        Debug.Log(json);
        list = JsonMapper.ToObject<List<AssetManager>>(json);
        //StartCoroutine(IE_GetJson());
    }
    private IEnumerator IE_GetJson() 
    {
        string json = File.ReadAllText(Path);
        yield return json;
        list = JsonMapper.ToObject<List<AssetManager>>(json);
        yield return list;
    }
    [ContextMenu("加载Asset")]
private void  GetAsset()
{
    AssetsLoader.GetInstance().L_assetManager = list;
    StartCoroutine(AssetsLoader.GetInstance().IE_LoadAsset());
}
}
