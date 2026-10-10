using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrashTextScript : MonoBehaviour
{
    public static int TrashItemCount = 0;
    Text text;
    void Start()
    {
        text = GetComponent<Text>();
        // El texto con "/meta" es mas largo y el cuadro era justo: sin esto Unity parte la linea
        // y esconde el "0/10". Dejamos que se estire en vez de cortarse.
        text.horizontalOverflow = HorizontalWrapMode.Overflow;
        text.verticalOverflow = VerticalWrapMode.Overflow;
        TrashItemCount = 0;
    }

    void Update()
    {
        text.text = $"Basura eliminada: {TrashItemCount}/{GoalManager.Goal}";
    }
}