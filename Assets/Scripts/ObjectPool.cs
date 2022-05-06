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
        GameObject createdObject;

        foreach (GameObject _prefab in _prefabs)
        {
            queue = new Queue<GameObject>();

            for(int i = 0; i < 20; i++)
            {
                createdObject = Instantiate(_prefab, transform.parent);
                createdObject.SetActive(false);
                queue.Enqueue(_prefab);
            }

            _poolDictionary.Add(_prefab.name, queue);
        }
    }

    public GameObject[] GetHighlightPaths(int quantity)
    {
        GameObject[] _paths = new GameObject[quantity];

        for(int i = 0; i < quantity; i++)
        {
            if (_poolDictionary["HighlightPath"].Count > 0)
            {
                _paths[i] = _poolDictionary["HighlightPath"].Dequeue();
                _paths[i].SetActive(true);
            }
            else
            {
                _paths[i]= Instantiate(_prefabs.Where(obj => obj.name == "HighlightPath").SingleOrDefault(), transform.parent);
            }
        }

        return _paths;
    }

}
