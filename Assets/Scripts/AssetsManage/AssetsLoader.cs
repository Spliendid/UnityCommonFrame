using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AssetsLoader : Singleton<AssetsLoader>
{

    public uint AssetNumber = 0;
    public List<AssetManager> L_assetManager ;
    private Dictionary<string, Object> assetsDic = new Dictionary<string,Object>();//将加载的资源存到字典中
  
    
    #region 外部根据名字获取资源
    public T GetAssetObj<T>(string assetName) where T : Object
    {
        try
        {
            return assetsDic[assetName] as T;
        }
        catch (System.Exception)
        {

            Debug.Log("<color=red>GetAssetObjError</color>");
        }
        return null;
    }
    public Object GetAssetObj(string assetName)
    {
        try
        {
            return assetsDic[assetName];
        }
        catch (System.Exception)
        {

            Debug.Log("<color=red>GetAssetObjError</color>");
        }
        return null;
    }
    #endregion
   

    private void GetAssetList() 
    {

    }


    #region 初始化
    private IEnumerator Init()
    {
        //TODO获取L_assetManager
        string jsonPath = Application.streamingAssetsPath +"/"+ConstantClass.JsonDataPath;
        string json = System.IO.File.ReadAllText(jsonPath);
        L_assetManager = ToolsScripts.GetJsonObj<List<AssetManager>>(json);
        yield return L_assetManager;
    }
    #endregion

    #region 加载Asset

    #region 单独加载某个资源时
    
    #endregion

    //分为多个携程同时加载
    private bool isAssetSceneObjLoadComplete = false;//场景主物体加载（耗费时间大）
    private bool isUIAssetLoadComplete = false;//UIobj加载
    private bool isOtherAssetLoadComplete = false;//其他资源加载
    private bool isOther2AssetLoadComplete = false;

    private IEnumerator loadSingleAsset(string Name) 
    {
        foreach (var item in L_assetManager)
        {
            if (item.asset.AssetsName == Name)
            {
                yield return ToolsScripts.StartCoroutine(item.IE_GetAsset());
                if (!assetsDic.ContainsKey(item.Name))
                {
                    assetsDic.Add(item.Name, item.GetAsset());
                }
            }
        }
        yield return null;
    }
    private IEnumerator loadAsset(AssetManager item) 
    {
        yield return ToolsScripts.StartCoroutine(item.IE_GetAsset());
        assetsDic.Add(item.Name, item.GetAsset());
        GameObject go = GameObject.Instantiate(item.GetAsset()) as GameObject;
        AssetNumber++;
        Debug.Log(Time.time);
        yield return go;
    }
    //加载主场景物体
    private IEnumerator loadSceneObjAsset() 
    {
        foreach (var item in L_assetManager)
        {
            if (item.asset.assetsType == E_AssetsType.SceneObj)
            {
                yield return ToolsScripts.StartCoroutine(item.IE_GetAsset());
                if (!assetsDic.ContainsKey(item.Name))
                {
                    assetsDic.Add(item.Name, item.GetAsset());
                }
            }
        }
        isAssetSceneObjLoadComplete = true;
        yield return null;
    }

    private IEnumerator loadUIAsset()
    {
        foreach (var item in L_assetManager)
        {

            if (item.asset.assetsType == (E_AssetsType)2)
            {
                yield return ToolsScripts.StartCoroutine(item.IE_GetAsset());
                if (!assetsDic.ContainsKey(item.Name))
                {
                    assetsDic.Add(item.Name, item.GetAsset());
                }
            }
            AssetNumber++;
        }
        isUIAssetLoadComplete = true;
    }

    private IEnumerator loadOtherAsset()
    {
        foreach (var item in L_assetManager)
        {

            if (item.asset.assetsType == (E_AssetsType)3)
            {
                yield return ToolsScripts.StartCoroutine(item.IE_GetAsset());
                if (!assetsDic.ContainsKey(item.Name))
                {
                    assetsDic.Add(item.Name, item.GetAsset());
                }
            }
            AssetNumber++;
        }
        isOtherAssetLoadComplete = true;
    }
    private IEnumerator loadOther2Asset()
    {
        foreach (var item in L_assetManager)
        {

            if (item.asset.assetsType == (E_AssetsType)4)
            {
                yield return ToolsScripts.StartCoroutine(item.IE_GetAsset());
                if (!assetsDic.ContainsKey(item.Name))
                {
                    assetsDic.Add(item.Name, item.GetAsset());
                }
            }

            AssetNumber++;
        }
        isOther2AssetLoadComplete = true;
    }
    private IEnumerator loadAssetMain() 
    {
        ToolsScripts.StartCoroutine(loadSceneObjAsset());
        ToolsScripts.StartCoroutine(loadUIAsset());
        ToolsScripts.StartCoroutine(loadOtherAsset());
        ToolsScripts.StartCoroutine(loadOther2Asset());
        yield return new WaitUntil(() => { return isAssetSceneObjLoadComplete && isUIAssetLoadComplete && isOtherAssetLoadComplete && isOther2AssetLoadComplete; });
        Debug.Log(Time.time);
    }
    #endregion

    #region 外部调用加载资源
    //开始时加载资源
    public IEnumerator IE_LoadAsset()
    {
        yield return ToolsScripts.StartCoroutine(Init());//初始化
        yield return ToolsScripts.StartCoroutine(loadAssetMain());//加载资源完成
        yield return null;
    }

    //加载单独资源
    public IEnumerator IE_LoadSingleAsset(string Name) 
    {
        ToolsScripts.StartCoroutine(IE_LoadSingleAsset(Name));
        yield return null;
    }
    #endregion
 
}

