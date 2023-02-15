using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class GData
{
    public const string SCENE_NAME_TITLE = "01.TitleScene";
    public const string SCENE_NAME_STAGE_1 = "02.PlayScene_Stage1";
    public const string SCENE_NAME_STAGE_2 = "03.PlayScene_Stage2";

    public const string OBJ_NAME_CURRENT_LEVEL = "Level_1";
}

public class ObjectData
{
    public int id;
    public string name;
    public string sprite_name;
    public string animator_name;
    public string animator_path;
}

public class StageData
{
    public int id;
    public string name;
    public int row;
    public int col;
    public string cells;

    public StageProperty Convert()
    {
        StageProperty sp = new StageProperty();
        sp.id = this.id;
        sp.name = this.name;
        sp.row = this.row;
        sp.col = this.col;
        string str = cells;

        

        return sp;
    }
}
public class StageProperty
{
    public int id;
    public string name;
    public int row;
    public int col;
    public int[,] objs;
}
