using UnityEngine;
using UnityEngine.UI;
public class SliderControl : MonoBehaviour
{

    [Header("UI Componentes")]
    [Tooltip("Arraste o objeto Slider da cena aqui.")]
    public Slider pointsSlider;
    public string sliderObjectName = "SeuSliderDePontos";

    void Start()
    {
        if (GameManager.Mestre != null)
        {
            UpdateSliderUI(GameManager.Mestre.chronospontos);
        }

        
        pointsSlider.interactable = false;
    }
    private void FixedUpdate()
    {
        // 1. Tenta encontrar o GameObject pelo nome na cena.
        GameObject sliderObject = GameObject.Find(sliderObjectName);

        if (sliderObject != null)
        {
            // 2. Tenta obter o componente Slider a partir desse GameObject.
            pointsSlider = sliderObject.GetComponent<Slider>();
        }


        if (pointsSlider == null)
        {
            Debug.LogError($"Componente Slider não encontrado no GameObject chamado '{sliderObjectName}'. Verifique o nome e se o componente está anexado.");
        }
    }
    private void Update()
    {
      
        if (GameManager.Mestre != null)
        {
         
            int currentChronosPoints = GameManager.Mestre.chronospontos;

         
            UpdateSliderUI(currentChronosPoints);
        }
    }

  
    private void UpdateSliderUI(int points)
    {
        pointsSlider.value = points;
    }

 
}