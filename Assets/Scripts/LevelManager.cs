using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    #region sinleton
    public static LevelManager instance;
    void Awake()
    {
        if(instance==null){instance=this;}
    }

    #endregion
    
    [HideInInspector] public int objectsInPart0;
	[HideInInspector] public int totalObjectsInPart0;
    [HideInInspector] public int objectsInPart1;
	[HideInInspector] public int totalObjectsInPart1;

    [Header("Level Settings")]
    [SerializeField] private GameObject[] levels;
    [SerializeField] public GameObject level;
    GameManager gm;

    GameObject part0;
    GameObject part1;

    [Header("Level Progress")]
    public int part0Total = 0;
    public int part0Current = 0;

    public int part1Total = 0;
    public int part1Current = 0;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        loadLevel();
    }

    public int maxLevel(){
        Debug.Log(levels.Length);
        return levels.Length-1;
    }

    public void loadLevel(){
        level.SetActive(false);
        Destroy(level);
        level = Instantiate(levels[gm.level]);
        part0 = level.transform.Find("Part0").gameObject;
        part1 = level.transform.Find("Part1").gameObject;

        part0Total = part0.transform.Find("Objects").childCount;
        part1Total = part1.transform.Find("Objects").childCount;

        part0Current = part0Total;
        part1Current = part1Total;
    }
}
