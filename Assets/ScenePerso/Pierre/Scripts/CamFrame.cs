using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CamFrame : MonoBehaviour
{
    public float frameRateStop = 0.25f;

    public RenderTexture _renderTexture;

    public Camera _camera;

    private Coroutine coroutineInstance;

    // Start is called before the first frame update
    void Start()
    {
        _camera = gameObject.GetComponent<Camera>();
        _renderTexture = _camera.targetTexture; 
    }

    // Update is called once per frame
    void Update()
    {
        if(coroutineInstance == null)
        {
            Debug.Log("Je fais 0");
            coroutineInstance = StartCoroutine(CameraFramebyFrame());
        }
    }

    public IEnumerator CameraFramebyFrame()
    {
        Debug.Log("Je fais 1");
        _camera.enabled = false;
        yield return new WaitForSeconds(frameRateStop);
        _camera.enabled = true;

        coroutineInstance = null;
    }

}


/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class CamFrame : MonoBehaviour
{
    public float frameRateStop = 1;

    public RenderTexture _renderTexture;
    public Camera _camera;

    public Texture2D tmpTexture;

    private Coroutine coroutineInstance;
    private Material displayMaterial;
    private bool shouldCaptureFrame = false;

    // Start is called before the first frame update
    void Start()
    {
        _camera = gameObject.GetComponent<Camera>();
        _renderTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
        tmpTexture = new Texture2D(_renderTexture.width, _renderTexture.height, TextureFormat.RGB24, false);

        if (displayMaterial != null)
        {
            displayMaterial.mainTexture = tmpTexture;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(coroutineInstance == null)
        {
            Debug.Log("Je fais 0");
            coroutineInstance = StartCoroutine(CameraFramebyFrame());
        }
    }

    public IEnumerator CameraFramebyFrame()
    {
        Debug.Log("Je fais 1");

        shouldCaptureFrame = true;

        yield return new WaitForSeconds(frameRateStop);


        coroutineInstance = null;
    }

        private void OnPostRender()
    {
        if (shouldCaptureFrame)
        {
            RenderTexture.active = _renderTexture;
            tmpTexture.ReadPixels(new Rect(0, 0, _renderTexture.width, _renderTexture.height), 0, 0);
            tmpTexture.Apply();
            RenderTexture.active = null;

            // Réinitialiser le flag pour ne pas capturer à chaque frame
            shouldCaptureFrame = false;
        }
    }

}*/