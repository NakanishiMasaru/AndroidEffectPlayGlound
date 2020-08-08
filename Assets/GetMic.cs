using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetMic : MonoBehaviour
{
    [SerializeField] private List<string> mic;
    
    // Start is called before the first frame update
    void Start()
    {
        var dr = GetComponent<Dropdown>();
        mic.Clear();
        foreach (var str in Microphone.devices)
        {
            mic.Add(str);
            dr.options.Add(new Dropdown.OptionData { text = str });
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
