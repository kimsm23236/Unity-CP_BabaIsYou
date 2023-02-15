using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

public class DataManager
{
    private static DataManager instance_;

    public Dictionary<int, ObjectData> dicObjData;
    public Dictionary<string, RuntimeAnimatorController> dicObjAnimData;
    public Dictionary<int, StageData> dicStgData;
    public RuntimeAnimatorController toAnimData;

    private DataManager()
    {
        dicObjData = new Dictionary<int, ObjectData>();
        dicObjAnimData = new Dictionary<string, RuntimeAnimatorController>();
        dicStgData = new Dictionary<int, StageData>();
        LoadObjsDatas();
        LoadAnimDatas();
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
        var json = Resources.Load<TextAsset>("Datas/ObjsData").text;
        var arrObjDatas = JsonConvert.DeserializeObject<ObjectData[]>(json);
        this.dicObjData = arrObjDatas.ToDictionary(x => x.id);
    }
    public void LoadAnimDatas()
    {
        for(int i = 0 ; i < dicObjData.Count; i++)
        {
            ObjectData od = dicObjData[i];
            if(od.name[0] == 'o')
            {
                RuntimeAnimatorController anim = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(Resources.Load("Animations/Objects/baba/baba_0_1", typeof(RuntimeAnimatorController)));
    
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
