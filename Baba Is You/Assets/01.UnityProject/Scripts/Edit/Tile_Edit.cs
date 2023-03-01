using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;

[ExecuteAlways]
public class Tile_Edit : MonoBehaviour, IMouse
{
    private Image image;
    private Sprite sprite;
    private Sprite defaultSprite;
    private int objectId;
    public int objectID
    {
        get
        {
            return objectId;
        }
    }
    GridController_Edit gridController_ = default;
    private bool isMouseOver = default;
    public delegate void EventHandler();
    public delegate void EventHandler_Texture(Texture2D texture);
    public delegate void EventHandler_Int_Texture(int id, Texture2D texture);
    public EventHandler onMouseOver;
    public EventHandler_Texture onSetupTexture;
    public EventHandler_Int_Texture onSetupObject;
    // Start is called before the first frame update
    void Start()
    {
        GFunc.Log("Image component setup");
        GameObject gObjs = GFunc.GetRootObj("GameObjs");
        gridController_ = gObjs.FindChildObj("Grid").GetComponentMust<GridController_Edit>();
        image = gameObject.GetComponentMust<Image>();
        onSetupTexture = new EventHandler_Texture(SetSprite);
        onMouseOver = new EventHandler(OnPointing);
        onSetupObject = new EventHandler_Int_Texture(SetObject);

        defaultSprite = default;
        objectId = -1;
    }

    // Update is called once per frame
    void Update()
    {

    }
    void SetImage()
    {
        image.sprite = sprite;
        GFunc.Log("Set Image");
    }
    void SetSprite(Texture2D texture2D)
    {
        // prevSprite = sprite;
        sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
        SetImage();
    }

    public void OnPointing()
    {
        if(ObjectListWindow.selectedTexture == null || ObjectListWindow.selectedTexture == default)
        {
            GFunc.Log("Texture is null");
            return;
        }
        if(isMouseOver)
            return;
        gridController_.onPointingTile();
        isMouseOver = true;
        onSetupTexture(ObjectListWindow.selectedTexture);
    }
    public void OutPointing()
    {
        if(objectId >= 0)
            return;
        if(!isMouseOver)
            return;
        
        isMouseOver = false;
        sprite = default;
        SetImage();
    }
    public void SetObject(int id, Texture2D texture)
    {
        objectId = id;
        onSetupTexture(texture);
    }
    public void MouseEnter()
    {
        // 이미지 바꾸기
        Texture2D texture = ObjectListWindow.selectedTexture;
        if(texture == null || texture == default)
            return;
        Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        image.sprite = newSprite;
        GFunc.Log("Editor Tile Mouse Enter");
    }
    public void MouseExit()
    {
        image.sprite = defaultSprite;
        GFunc.Log("Editor Tile Mouse Exit");
    }
    public void MouseClick_L()
    {
        objectId = ObjectListWindow.selectedObjId;
        Texture2D texture = ObjectListWindow.selectedTexture;
        defaultSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        image.sprite = defaultSprite;
    }
    public void MouseClick_R()
    {
        defaultSprite = default;
        image.sprite = defaultSprite;
    }
}
#endif