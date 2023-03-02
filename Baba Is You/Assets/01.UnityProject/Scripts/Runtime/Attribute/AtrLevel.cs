using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AtrLevel : Attribute
{
    private TMP_Text addText = default;
    private GridController gridController = default;
    public AtrLevel()
    {
        AttributeData atrData = DataManager.Instance.dicAtrData[9];
        property_ = new AttributeProperty();
        property_.id = atrData.id;
        property_.name = atrData.name;
        property_.priority = 0;
    }
    public override void Attached(GameObject gObj_)
    {
        base.Attached(gObj_);
        GameObject gameObj = GFunc.GetRootObj("GameObjs");
        gridController = gameObj.FindChildObj("Grid").GetComponentMust<GridController>();
        GameObject uiObj = GFunc.GetRootObj("UiObjs");

        GameObject text = new GameObject();
        text.transform.SetParent(gObj_.transform);
        text.SetLocalPos(Vector3.zero);
        
        addText = text.AddComponent<TextMeshProUGUI>();
        addText.text = ownerOpc.TagId.ToString("D2");
        addText.alignment = TextAlignmentOptions.Center;

    }
}
