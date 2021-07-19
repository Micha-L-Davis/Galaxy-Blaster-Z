using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Rendering.PostProcessing;

public class GammaCorrection : MonoBehaviour
{
    float _gammaCorrection;
    [SerializeField]
    Slider _slider;
    [SerializeField]
    PostProcessProfile _profile;

    private void Start()
    {
        _slider.value = PlayerPrefs.GetFloat("GammaCorrection", 0f);
    }

    public void SetLevel (float sliderValue)
    {
        Debug.Log(sliderValue);
        _profile.GetSetting<ColorGrading>().gamma.value = new Vector4(1f, 1f, 1f, sliderValue);
        PlayerPrefs.SetFloat("GammaCorrection", sliderValue);

    }

}
