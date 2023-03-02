using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonBase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    protected Image outLayeredImage = default;
    protected Image inLayeredImage = default;
    protected bool isLocked = default;

    public Color outLayeredColor = default;
    public Color inLayeredColor = default;
    protected Color outLayeredColor_Locked = default;
    protected Color inLayeredColor_Locked = default;
    // Start is called before the first frame update
    void Awake()
    {
        outLayeredImage = gameObject.FindChildObj("Layered1").GetComponentMust<Image>();
        inLayeredImage = gameObject.FindChildObj("Layered2").GetComponentMust<Image>();
    }
    public virtual void Start()
    {
        outLayeredColor.a = 255;
        inLayeredColor.a = 255;
        outLayeredImage.color = outLayeredColor;
        inLayeredImage.color = inLayeredColor;

        outLayeredColor_Locked = new Color(116 / 255f, 116 / 255f, 116 / 255f, 1);
        inLayeredColor_Locked = new Color(52 / 255f, 52 / 255f, 52 / 255f, 1);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if(isLocked)
            return;
        GFunc.Log("Pointer Enter");
        inLayeredImage.color = outLayeredColor;
    }
    
    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if(isLocked)
            return;
        GFunc.Log("Pointer Exit");
        inLayeredImage.color = inLayeredColor;
    }
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        /* Do Nothing */
    }
    public virtual void Lock()
    {
        isLocked = true;
        outLayeredImage.color = outLayeredColor_Locked;
        inLayeredImage.color = inLayeredColor_Locked;
    }
    public virtual void UnLock()
    {
        if(!isLocked)
            return;

        isLocked = false;
        outLayeredImage.color = outLayeredColor;
        inLayeredImage.color = inLayeredColor;
    }
}
