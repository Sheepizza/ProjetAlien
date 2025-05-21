using Mirror;
using UnityEngine;

public class LDObjectManager : NetworkBehaviour
{
    public LDObjectDatas _LDObjectDatas;
    public LDTargetDatas _LDTargetDatas;
    public UniqueDoorOpen _uniqueDoorOpen;

    public GameObject objectInHand;
    public string objectName;
    public string targetName;
    public string objectTag;
    public string targetTag;

    public string ResultName;
    public GameObject ResultObject;

    PickUpManager pickUpManager;

    private void Start()
    {
        pickUpManager = GetComponent<PickUpManager>();
    }

    public void GiveObjectTag(GameObject _takenObject)
    {
        objectInHand = _takenObject;
        objectName = objectInHand.name;
        for (int i = 0; i < _LDObjectDatas.LDOBjectList.Count; i++)
        {
            if (_LDObjectDatas.LDOBjectList[i].name == objectName)
            {
                objectTag = _LDObjectDatas.LDOBjectList[i].Tag;
            }
        }
    }

    public bool IsKey(GameObject _gameObject)
    {
        objectName = _gameObject.name;
        for (int i = 0; i < _LDObjectDatas.LDOBjectList.Count; i++)
        {
            if (_LDObjectDatas.LDOBjectList[i].name == objectName)
            {
                return true;
            }
        }
        return false;
    }

    public void CheckInteraction(string _targetObjectName)
    {

        targetName = _targetObjectName;

        for (int i = 0; i < _LDTargetDatas.LDTargetList.Count; i++)
        {
            if (_LDTargetDatas.LDTargetList[i].name == targetName)
            {
                targetTag = _LDTargetDatas.LDTargetList[i].Tag;
                ResultName = _LDTargetDatas.LDTargetList[i].ResultName;
            }
        }
        if (targetTag == null)
        {
            targetName = null;
        }

        else
        {
            foreach (var key in GameManager.Instance.Keys)
            {
                foreach (var obj in _LDObjectDatas.LDOBjectList)
                {
                    if (obj.name == key)
                    {
                        objectTag = obj.Tag;
                    }
                }
            }

            if (targetTag == objectTag)
            {
                InteractionPass();
            }
        }

        /*else if (targetTag == objectTag)
        {
            InteractionPass();
        }*/




    }

    public void DropObject()
    {
        objectInHand = null;
        objectName = null;
        objectTag = null;
        targetName = null;
        targetTag = null;
    }

    public void InteractionPass()
    {
        Debug.Log("Interaction Pass");
        //Ajouter ici un choix de différentes intéractions (La c'est pour une porte mais ça pourrait être autre chose à activer avec un autre objet)
        ResultObject = GameObject.Find(ResultName);
        _uniqueDoorOpen = ResultObject.GetComponent<UniqueDoorOpen>();
        _uniqueDoorOpen.OpenDoor(ResultObject);
        DestroyObject();
        objectName = null;
        objectTag = null;
        targetName = null;
        targetTag = null;
        ResultObject = null;
        _uniqueDoorOpen = null;
        ResultName = null;
    }

    [Command(requiresAuthority = false)]
    public void DestroyObject()
    {
        Destroy(objectInHand);
    }
}
