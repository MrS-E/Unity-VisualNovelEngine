using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    
    
    void Start()
    {
        InputDecoder.ReadScript("Script/script");
    }

    void Update()
    {
        if (Input.GetKeyDown("h"))
        {
            if (InputDecoder.InterfaceElements.activeInHierarchy)
            {
                InputDecoder.InterfaceElements.SetActive(false);
            }
            else
            {
                InputDecoder.InterfaceElements.SetActive(true);
            }
        }

        if (InputDecoder.Commands[InputDecoder.CommandLine]!= InputDecoder.lastCommand && InputDecoder.CommandLine < InputDecoder.Commands.Count - 1)
        {
            InputDecoder.lastCommand = InputDecoder.Commands[InputDecoder.CommandLine];
            InputDecoder.PausedHere = false;
            InputDecoder.ParseInputLine(InputDecoder.Commands[InputDecoder.CommandLine]);
        }

        if(InputDecoder.CommandLine < InputDecoder.Commands.Count - 1 && !InputDecoder.atQuestion && (((Input.GetMouseButtonDown(0)||Input.GetKeyDown("space")) && (InputDecoder.PausedHere) || (!InputDecoder.PausedHere))))
        {
            InputDecoder.CommandLine++;
        }
    }
}
