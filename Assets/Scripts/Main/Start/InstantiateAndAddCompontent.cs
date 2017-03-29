
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class InstantiateManager:Singleton<InstantiateManager>
{
    private List<InstantObj> instantObjList;

    #region 获取instantObjList
    
    #endregion

    #region 实例化InstantObj
    //根据InstantObj内的信息实例化AssetsLoader内已经加载好的资源
    private IEnumerator IE_InstantObj(InstantObj item) 
    {
       
          
            GameObject go = GameObject.Instantiate(AssetsLoader.GetInstance().GetAssetObj(item.Name)) as GameObject;
            yield return go;
            if (null!=item.FatherName&&item.FatherName!=""&&item.FatherName!="null")
            {
                go.transform.SetParent(PublicData.MainObjDic[item.FatherName].transform);
            }
            if (item.isResetTransform)
            {
                go.transform.localPosition = item.localPos;
                go.transform.localRotation = Quaternion.Euler(item.LocalEuler);
            }
            PublicData.MainObjDic.Add(item.Name,go);
       
        yield return null;
    }
    #endregion

    #region 外部调用
    //实例化并获取obj
    public IEnumerator IE_InstantiateObjMain() 
    {
        foreach (var item in instantObjList)
        {
            if (null==item) continue;
            ToolsScripts.StartCoroutine(IE_InstantObj(item));//可等待也可不等待
        }
        yield return null;
    }
    //添加组件 并获取
    public IEnumerator IE_AddComponent() 
    {
      PublicData._MainSceneObjScript =   PublicData.MainObjDic[ConstantClass.MianSecneObjName].AddComponent<MainSceneObjScript>();

        yield return null;
    }
    #endregion
}
