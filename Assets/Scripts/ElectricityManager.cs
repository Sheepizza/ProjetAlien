using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityManager : MonoBehaviour
{
    private static ElectricityManager instance = null;
    public static ElectricityManager Instance => instance;
    [SerializeField]
    int _mawPower;
    [SerializeField]
    [Range(0, 5)]
    int _powerReserve;
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

    public void IncreaseActivePower()
    {
        if (_powerReserve != _mawPower)
        {
            _powerReserve += 1;
        }
    }

    public void DecreaseActivePower()
    {
        _powerReserve -= 1;
    }

    public void UpgradeMaxPower(int _reserve)
    {
        _mawPower += _reserve;
    }

    public bool CompareActivePower()
    {
        return _powerReserve < _mawPower;
    }
}
