using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MovePanel : MonoBehaviour
{
    [SerializeField] private Slider moveSlider;
    [SerializeField] private Button moveBtn;
    [SerializeField] private Button cancelBtn;
    [SerializeField] private InputField selectedUnitsInputField;


    void Start(){
        // add event listener to cancelBtn
        cancelBtn.onClick.AddListener(cancelBtnClic);
        // on value change on moveSlider change the value of selectedUnitsInputField et inversement
        selectedUnitsInputField.onValueChanged.AddListener(inputChange);
        moveSlider.onValueChanged.AddListener(sliderChange);
    }

    void cancelBtnClic(){
        gameObject.SetActive(false);
    }


    void inputChange(string value){
        if (value == ""){
            moveSlider.value = 0;
        } else {
            moveSlider.value = int.Parse(value);
        }
    }

    void sliderChange(float value){
        selectedUnitsInputField.text = value.ToString();
    }

}
