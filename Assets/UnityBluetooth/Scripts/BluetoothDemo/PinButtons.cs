using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace JC.BluetoothUnity.Demo
{
    public class PinButtons : MonoBehaviour
    {
        public static UnityAction<ButtonAction> OnButtonClicked;
        [SerializeField] Button[] _buttons;  // 0-9, CLR, BACK 
        public enum ButtonAction {Num0=0, Num1, Num2, Num3, Num4, Num5, Num6, Num7, Num8, Num9, CLR, BACK};

        // Start is called before the first frame update
        void Start()
        {
            for(int i = 0; i < _buttons.Length; i++)
            {
                ButtonAction action = (ButtonAction)i;
                _buttons[i].transform.GetComponentInChildren<Text>().text = action.ToString().Replace("Num", "");
                _buttons[i].onClick.AddListener(()=>OnButtonClicked?.Invoke(action));
            }
        }
    }
}