using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class processDisplay : MonoBehaviour
{
    [SerializeField]
    TMP_Text text;

    public void setProcNum(string s)
    {
        text.text = s;
    }

}
