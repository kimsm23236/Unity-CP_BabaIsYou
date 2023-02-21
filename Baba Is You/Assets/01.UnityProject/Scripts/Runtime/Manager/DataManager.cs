using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

public class DataManager
{
    private static DataManager instance_;

    public Dictionary<int, ObjectData> dicObjData;
    public Dictionary<int, AttributeData> dicAtrData;
    public Dictionary<string, RuntimeAnimatorController> dicObjAnimData;
    public Dictionary<int, StageData> dicStgData;
    public RuntimeAnimatorController toAnimData;

    private DataManager()
    {
        dicObjData = new Dictionary<int, ObjectData>();
        dicAtrData = new Dictionary<int, AttributeData>();
        dicObjAnimData = new Dictionary<string, RuntimeAnimatorController>();
        dicStgData = new Dictionary<int, StageData>();
        LoadObjsDatas();
        LoadAtrsDatas();
        LoadAnimDatas();
        LoadStageDatas();
    }
    public static DataManager Instance
    {
        get
        {
            if(instance_ == null)
            {
                DataManager.instance_ = new DataManager();
            }
            return DataManager.instance_;
        }
    }

    public void LoadObjsDatas()
    {
        var json = Resources.Load<TextAsset>("Datas/ObjsData_v4").text;
        var arrObjDatas = JsonConvert.DeserializeObject<ObjectData[]>(json);
        this.dicObjData = arrObjDatas.ToDictionary(x => x.id);
    }
    public void LoadAtrsDatas()
    {
        var json = Resources.Load<TextAsset>("Datas/AtrData").text;
        var arrAtrDatas = JsonConvert.DeserializeObject<AttributeData[]>(json);
        this.dicAtrData = arrAtrDatas.ToDictionary(x => x.id);
    }
    public void LoadAnimDatas()
    {
        for(int i = 0 ; i < dicObjData.Count; i++)
        {
            ObjectData od = dicObjData[i];
            string animatorPath = dicObjData[i].animator_path;
            if(od.otype == "o")
            {
                GFunc.Log($"path is [{animatorPath}]");
                RuntimeAnimatorController anim = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(Resources.Load(animatorPath, typeof(RuntimeAnimatorController)));
    
                dicObjAnimData.Add(od.name, anim);
            }
        }
        toAnimData = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(Resources.Load("Animations/Text/textAnim", typeof(RuntimeAnimatorController)));
    }
    public void LoadStageDatas()
    {
        var json = Resources.Load<TextAsset>("Datas/temp").text;
        var arrStgDatas = JsonConvert.DeserializeObject<StageData[]>(json);
        this.dicStgData = arrStgDatas.ToDictionary(x => x.id);
    }
}
