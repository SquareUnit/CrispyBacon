using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RiderInfoUI : MonoBehaviour
{
    public RiderController riderController;

    public List<Text> UITextElements;
    public List<string> textPrefix;
    public List<string> textBody;


    private void Update()
    {
        float temp = Truncate(riderController.inputSystem.LeftStick.x, 2);
        textBody[0] = temp.ToString();
        temp = Truncate(riderController.MovementSpeed, 2);
        textBody[1] = temp.ToString();
        temp = Truncate(riderController.RotationSpeed, 2);
        textBody[2] = temp.ToString();
        temp = Truncate(riderController.chargeMeter, 2);
        textBody[3] = temp.ToString();

        for (int i = 0; i < UITextElements.Count; i++)
        {
            UITextElements[i].text = $"{textPrefix[i]}{textBody[i]}";
        }
    }

    // Move to another extension class, ask Gevrai.
    public float Truncate(float value, int digits)
    {
        double mult = Math.Pow(10.0, digits);
        double result = Math.Truncate(mult * value) / mult;
        return (float)result;
    }

}

/*
public static float Test(this float value, int digits)
{
    double mult = Math.Pow(10.0, digits);
    double result = Math.Truncate(mult * value) / mult;
    return (float)result;
}
*/