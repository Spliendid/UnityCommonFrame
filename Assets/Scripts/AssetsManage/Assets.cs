using UnityEngine;
using System.Collections;
[System.Serializable]
public class Asset 
{
    public E_AssetsType assetsType;//资源类型
    public string AssetsName;
    public string Path;
   // [System.NonSerialized]
  //  public AssetBundle ab;
   
}

[System.Serializable]
public class AssetManager 
{
    private Object assetObj;   
    public  Asset asset;
    public AssetManager() 
    {

    }

    public AssetManager(Asset asset) 
    {
        this.asset = asset;
    }
    public string Name {
        get {
            return this.asset.AssetsName;
        }
    }

    //加载Asset
    public IEnumerator IE_GetAsset() 
    {
          string LoadPath = "file://" + Application.streamingAssetsPath + "/" + asset.Path;
         WWW www = new WWW(LoadPath);
            yield return www;
            //asset.ab = www.assetBundle;
            Debug.Log(www.assetBundle.mainAsset.name);
            assetObj = www.assetBundle.mainAsset;
          www.assetBundle.Unload(false);
        
    }
    public Object GetAsset() 
    {
        if (null!=assetObj)
        {
            return assetObj;
        }
        Debug.Log(string.Format("<color=red>{0}:\tLoadAssetError</color>",asset.AssetsName));
        return null;
    }
}
