using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public  class ObjectPool : MonoBehaviour
{
    private Dictionary<string, Queue<GameObject>> _poolDictionary;
    [SerializeField]
    private List<GameObject> _prefabs;

    private static ObjectPool _instance;
    public static ObjectPool Instance { get => _instance; } 

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        _poolDictionary = new Dictionary<string, Queue<GameObject>>();

        Queue<GameObject> queue;

        foreach (GameObject _prefab in _prefabs)
        {
            queue = new Queue<GameObject>();
            _poolDictionary.Add(_prefab.name, queue);
        }
    }

    public GameObject[] GetHighlightPaths(int _quantity, string _name)
    {
        GameObject[] _paths = new GameObject[_quantity];

        for(int i = 0; i < _quantity; i++)
        {
            if (_poolDictionary[_name].Count > 0)
            {
                _paths[i] = _poolDictionary[_name].Dequeue();
                _paths[i].SetActive(true);
            }
            else
            {
                _paths[i]= Instantiate(_prefabs.Where(obj => obj.name == _name).SingleOrDefault(), transform.parent);
            }
        }

        return _paths;
    }

    public GameObject GetHighlightPath(string _name)
    {
        GameObject _path;
        if (_poolDictionary[_name].Count > 0)
        {
            _path=_poolDictionary[_name].Dequeue();
            _path.SetActive(true);
        }
        else
        {
            _path = Instantiate(_prefabs.Where(obj => obj.name == _name).SingleOrDefault(), transform.parent);
        }

        return _path;
    }

    public void RemoveHighlightPath(PathPiece _path)
    {
        _poolDictionary[_path.Name].Enqueue(_path.gameObject);
        _path.gameObject.SetActive(false);
    }
}
