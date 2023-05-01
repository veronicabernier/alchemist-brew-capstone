using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

    public class BrewSearchItem : MonoBehaviour
{
    public TextMeshProUGUI userIdText;
    public TextMeshProUGUI tagText;
    public TextMeshProUGUI beanTypeText;
    public TextMeshProUGUI brandText;
    public TextMeshProUGUI brewMethodText;
    public TextMeshProUGUI coffeeWeightText;
/*    public string date;*/
    public TextMeshProUGUI extTimeText;
    public TextMeshProUGUI extWeightText;
    public TextMeshProUGUI grindSettingText;
    public TextMeshProUGUI innerSectionText;
    public TextMeshProUGUI notesText;
    public TextMeshProUGUI roastText;


    public void SetBrew(BrewData brew)
    {
        beanTypeText.text = "Bean Type: " + brew.bean_type;
        brandText.text = "Brand: " + brew.brand;
        brewMethodText.text = "Brew Method: " + brew.brew_method;
        coffeeWeightText.text = "Coffee Weight: " + brew.coffee_weight.ToString();
        extTimeText.text = "Time: " + brew.ext_time.ToString();
        extWeightText.text ="Weight: " +  brew.ext_weight.ToString();
        grindSettingText.text = "Grind Setting: " + brew.grind_setting.ToString();
        innerSectionText.text ="Flavor: " + brew.inner_section;
        notesText.text = "Notes: " + brew.notes;
        roastText.text = "Roast: " + brew.roast;

        userIdText.text = "User id: " + brew.userid;
        tagText.text = "Tag: " + brew.tagid;
    }
}
