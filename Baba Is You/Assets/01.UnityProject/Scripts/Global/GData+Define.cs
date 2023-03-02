using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class GData
{
    public const string SCENE_NAME_TITLE = "01.TitleScene";
    public const string SCENE_NAME_STAGE_1 = "02.PlayScene";
    public const string SCENE_NAME_STAGE_2 = "03.PlayScene_Stage2";

    public const string OBJ_NAME_CURRENT_LEVEL = "Level_1";
    public static List<int> OBJ_ID_TILING = new List<int> {3, 5, 6, 10};
}

public class ObjectData
{
    public int id;
    public string name;
    public string otype;
    public string ttype;
    public string sprite_name;
    public string sprite_path;
    public string animator_name;
    public string animator_path;
    public string basecolor;
    public string activatecolor;
    public string deactivatecolor;
    public int tag;
}
public class AttributeData
{
    public int id;
    public string name;
}
public class ColorData
{
    public string name;
    public float r;
    public float g;
    public float b;
}

[System.Serializable]
public class TempStageData
{
    public int id;
    public string name;
    public int row;
    public int col;
    public int[,] cells;
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
        sp.objs = new string[row, col];
        string temp = cells;
        List<char> charToRemove = new List<char>() {'[', ']', '{', '}', ' '};
        
        foreach(char c in charToRemove)
        {
            temp = temp.Replace(c.ToString(), string.Empty);
        }
        string[] sWord = temp.Split(",");
        int idx = 0;
        string LogLine = string.Empty;
        for(int y = 0; y < row; y++)
        {
            for(int x = 0; x < col; x++)
            {
                sp.objs[y,x] = sWord[idx];
                LogLine += sWord[idx] + " ";
                idx++;
            }
            GFunc.Log($"{LogLine}");
            LogLine = string.Empty;
        }
        return sp;
    }
    public void Convert_IntArrayToString(int[,] arr)
    {
        string newCellString = string.Empty;

        newCellString += "[";
        for(int y = 0; y < arr.GetLength(0); y++)
        {
            newCellString += "{";
            for(int x = 0; x < arr.GetLength(1); x++)
            {
                newCellString += arr[y,x].ToString();
                if(x < arr.GetLength(1) - 1)
                    newCellString += ", ";
                else
                    newCellString += "}";
            }
            if(y < arr.GetLength(0) - 1)
            {
                newCellString += ", ";
            }
            else
            {
                newCellString += "]";
            }
        }
        cells = newCellString;
    }
}
public class StageProperty
{
    public int id;
    public string name;
    public int row;
    public int col;
    public string[,] objs;
}
