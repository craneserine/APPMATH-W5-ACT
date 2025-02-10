using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;


public static class TweenUtils 
{
   // Ease In: Accelerates from 0 to 1
   public static float EaseIn(float t)
   {
       return t * t;
   }
   
   // Ease Out: Decelerates from 1 to 0
   public static float EaseOut(float t)
   {
       return 1 - (1 - t) * (1 - t);
   }

   // Normalize Time: Ensures that time is clamped between 0 and 1 based on maxTime
   public static float NormalizeTime(float t, float maxTime)
   {
       return Mathf.Clamp01(t / maxTime);
   }

   // Ease In and Out: Combines EaseIn and EaseOut based on the time value
   public static float EaseInOut(float t)
   {
       return t < 0.5f ? EaseIn(t) : EaseOut(t);
   }

   // Custom Ease In: Applies a custom exponent to the easing function
   public static float EaseInCustom(float t, int exponent)
   {
       return Mathf.Pow(t, exponent);
   }
}
