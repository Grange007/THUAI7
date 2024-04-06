using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    public Slider healthSlider1, healthSlider2;
    public TextMeshProUGUI healthText1, healthText2, ecoText1, ecoText2;
    BaseControl base1, base2;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!base1)
            base1 = GameObject.Find("Base1").GetComponent<BaseControl>();
        if (!base2)
            base2 = GameObject.Find("Base2").GetComponent<BaseControl>();
        healthText1.text = "Health: " + base1.messageOfBase.hp.ToString();
        healthSlider1.value = base1.messageOfBase.hp / 24000f;
        ecoText1.text = "Eco: " + base1.messageOfBase.economy.ToString();
        healthText2.text = "Health: " + base2.messageOfBase.hp.ToString();
        healthSlider2.value = base2.messageOfBase.hp / 24000f;
        ecoText2.text = "Eco: " + base2.messageOfBase.economy.ToString();
    }
    public void BuildCivilship()
    {
        if (PlayerControl.GetInstance().enabledInteract.Contains(InteractControl.InteractOption.BuildCivil))
            PlayerControl.GetInstance().selectedOption = InteractControl.InteractOption.BuildCivil;
    }
    public void BuildMilitaryship()
    {
        if (PlayerControl.GetInstance().enabledInteract.Contains(InteractControl.InteractOption.BuildMilitary))
            PlayerControl.GetInstance().selectedOption = InteractControl.InteractOption.BuildMilitary;
    }
    public void BuildFlagship()
    {
        if (PlayerControl.GetInstance().enabledInteract.Contains(InteractControl.InteractOption.BuildFlag))
            PlayerControl.GetInstance().selectedOption = InteractControl.InteractOption.BuildFlag;
    }
}
