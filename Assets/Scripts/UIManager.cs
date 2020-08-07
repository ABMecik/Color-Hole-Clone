using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{

    #region sinleton
    public static UIManager instance;
    void Awake()
    {
        if(instance==null){instance=this;}
    }

    #endregion

    [Header("UI Manager")]
    [SerializeField] int sceneOffset;
    [SerializeField] TMP_Text currentLevelText;
    [SerializeField] TMP_Text nextLevelText;
    [SerializeField] Image part0ProgressBar;
    [SerializeField] Image part1ProgressBar;
    [SerializeField] public GameObject startUI;

    GameManager gm;
    LevelManager levelManager;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        levelManager = FindObjectOfType<LevelManager>();
        part0ProgressBar.fillAmount = 0f;
        part1ProgressBar.fillAmount = 0f;

        currentLevelText.text = gm.level.ToString();
        nextLevelText.text = (gm.level+1).ToString();
    }

    public void updateBar(){
        part0ProgressBar.fillAmount = 0f;
        part1ProgressBar.fillAmount = 0f;

        currentLevelText.text = gm.level.ToString();
        nextLevelText.text = (gm.level+1).ToString();
    }

    public void updatePB(int part){
        if(part == 0){
            part0ProgressBar.fillAmount = 1f - ((float)levelManager.part0Current / levelManager.part0Total);
        }else{
            part1ProgressBar.fillAmount = 1f - ((float)levelManager.part1Current / levelManager.part1Total);
        }
    }
    
}
