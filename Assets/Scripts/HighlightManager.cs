using Mirror.Examples.Common;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HighlightManager : MonoBehaviour
{
    public PickableObjects _pickableObjects;
    List<string> _objectsToHighlight;
    GameObject[] _highlightInScene;
    GameObject highlightedObject = null;

    public bool CanInteract = false;

    static HighlightManager instance = null;
    public static HighlightManager Instance => instance;
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

        _objectsToHighlight = _pickableObjects.PickableObjectsTagName;
    }

    public void HighlightObject(Camera playerCamera)
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 2f))
        {
            GameObject targetObject = hit.collider.gameObject;
            if (highlightedObject != targetObject)
            {
                if (highlightedObject != null)
                {
                    var _highlightOutline = highlightedObject.GetComponent<Outline>();
                    if (_highlightOutline != null)
                    {
                        CanInteract = false;
                        _highlightOutline.enabled = false;
                    }
                }

                var outline = targetObject.GetComponent<Outline>();
                if (outline != null)
                {
                    CanInteract = true;
                    outline.enabled = true;
                }
                highlightedObject = targetObject;
            }
        }
        else
        {
            if (highlightedObject != null && highlightedObject.GetComponent<Outline>() != null)
            {
                CanInteract = false;
                highlightedObject.GetComponent<Outline>().enabled = false;
            }
            highlightedObject = null;
        }

    }

    public void DisableOutlineForTag()
    {
        foreach (string tag in _objectsToHighlight)
        {
            _highlightInScene = GameObject.FindGameObjectsWithTag(tag);

            foreach (GameObject obj in _highlightInScene)
            {
                var outline = obj.GetComponent<Outline>();
                if (outline != null)
                {
                    outline.enabled = false;
                }
            }
        }
        _highlightInScene = null;
    }

    bool CompareTags(GameObject _object)
    {
        foreach(string tag in _objectsToHighlight)
        {
            if (_object.CompareTag(tag)) return true;
        }
        return false;
    }

    public string GetObjectTag()
    {
        if (highlightedObject != null)
        {
            return highlightedObject.tag;
        }
        return null;
    }

    public string GetObjectName()
    {
        if (highlightedObject != null)
        {
            return highlightedObject.name;
        }
        return null;
    }
}
