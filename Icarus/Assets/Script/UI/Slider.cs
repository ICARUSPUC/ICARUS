using UnityEngine;
using UnityEngine.UI;
public class SliderControl : MonoBehaviour
{

    [Header("UI Componentes")]
    [Tooltip("Arraste o objeto Slider da cena aqui.")]
    public Slider pointsSlider;

   

    void Start()
    {
        if (GameManager.Mestre != null)
        {
            UpdateSliderUI(GameManager.Mestre.chronospontos);
        }

        
        pointsSlider.interactable = false;
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