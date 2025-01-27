using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    private static UIManager instance = null;
    public static UIManager Instance => instance;

    public RectTransform greenZone;
    public RectTransform redRectangle;
    public RectTransform cursor;
    public GameObject PickUpText;
    public GameObject PlaceText;
    public GameObject RepairText;
    public GameObject RepairPanel;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
		
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
