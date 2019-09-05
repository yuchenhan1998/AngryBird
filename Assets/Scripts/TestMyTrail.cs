using UnityEngine;
using System.Collections;

public class TestMyTrail : MonoBehaviour
{

    public WeaponTrail myTrail;

    private float t = 0.033f;
    private float tempT = 0;
    private float animationIncrement = 0.003f;
    void Start()
    {
        // set no trail by default
        myTrail.SetTime(0.0f, 0.0f, 1.0f);
    }

    public void StartTrails()
    {
        //set time
        myTrail.SetTime(2.0f, 0.0f, 1.0f);
        //start trail
        myTrail.StartTrail(0.5f, 0.4f);
    }

    public void ClearTrails()
    {
        //delete trail
        myTrail.ClearTrail();
    }

    void LateUpdate()
    {
        t = Mathf.Clamp(Time.deltaTime, 0, 0.066f);

        if (t > 0)
        {
            while (tempT < t)
            {
                tempT += animationIncrement;

                if (myTrail.time > 0)
                {
                    myTrail.Itterate(Time.time - t + tempT);
                }
                else
                {
                    myTrail.ClearTrail();
                }
            }

            tempT -= t;

            if (myTrail.time > 0)
            {
                myTrail.UpdateTrail(Time.time, t);
            }
        }
    }
}