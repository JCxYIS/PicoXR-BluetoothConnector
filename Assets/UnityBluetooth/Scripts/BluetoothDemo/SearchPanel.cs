using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JC.BluetoothUnity.Demo
{
    public class SearchPanel : MonoBehaviour
    {    
        [SerializeField] SearchResult _resultTemplate;
        [SerializeField] Button _startSearchButt;
        [SerializeField] InputField _pinInputField;

        List<SearchResult> _populatedResult = new List<SearchResult>();

        // Start is called before the first frame update
        void Start()
        {
            _resultTemplate.gameObject.SetActive(false);
            _startSearchButt.onClick.AddListener(StartSearching);
            
            PinButtons.OnButtonClicked += OnPinButtonClicked;
        }

        /// <summary>
        /// This function is called when the MonoBehaviour will be destroyed.
        /// </summary>
        void OnDestroy()
        {
            PinButtons.OnButtonClicked -= OnPinButtonClicked;            
        }

        void OnPinButtonClicked(PinButtons.ButtonAction action)
        {
            switch(action)
            {
                case PinButtons.ButtonAction.CLR:
                    _pinInputField.text = "";
                    break;
                case PinButtons.ButtonAction.BACK:
                    _pinInputField.text = _pinInputField.text.Substring(0, _pinInputField.text.Length-1);
                    break;
                default:
                    _pinInputField.text += (int)action;
                    break;
            }
        }

        public void StartSearching()
        {
            BluetoothManager.StartDiscovery();
            BluetoothManager.Toast("Start Searching...");
            StopAllCoroutines();
            StartCoroutine(PopulateListAsync());
        }

        IEnumerator PopulateListAsync()
        {
            while(true)
            {
                // Get List
                var deviceList = BluetoothManager.GetAvailableDevices();

                // Delete Old List
                _populatedResult.ForEach(r => Destroy(r.gameObject));
                _populatedResult.Clear();

                // Populate List
                foreach(var device in deviceList)
                {
                    var newResult = Instantiate(_resultTemplate, _resultTemplate.transform.parent);
                    newResult.gameObject.SetActive(true);
                    newResult.Init(device.name, device.mac, _pinInputField);
                    _populatedResult.Add(newResult);
                }

                // update every 1s
                // print("Updated device list. count="+deviceList.Length);
                yield return new WaitForSecondsRealtime(1);
            }
        }
    }
}