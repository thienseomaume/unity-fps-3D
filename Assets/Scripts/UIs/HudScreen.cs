using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HudScreen : MonoBehaviour, IScreen
{
    [SerializeField]
    public EnumScreen screenType; 
    public Image healthBar;
    public TMP_Text txtMagazine;
    public Button buttonPause;
    public Button inventoryButton;
    public GameObject magazineUi;
    public int selectedGun;
    public SkillUI LeftClick;
    public SkillUI ESkill;
    public SkillUI QSkill;
    public SkillUI RSkill;
    // Start is called before the first frame update
    private void Awake()
    {
        
    }
    
    void Start()
    {
        
    }
    

    // Update is called once per frame
    void Update()
    {

    }

    EnumScreen IScreen.GetScreenType()
    {
        return screenType;
    }

    public void Initialize()
    {
        
    }
    public void HideAllSkillUI()
    {
        LeftClick.gameObject.SetActive(false);
        ESkill.gameObject.SetActive(false);
        QSkill.gameObject.SetActive(false);
        RSkill.gameObject.SetActive(false);
    }
}