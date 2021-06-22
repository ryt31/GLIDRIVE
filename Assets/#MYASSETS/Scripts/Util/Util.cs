using UnityEngine;

namespace Glidrive.Util
{
    public class Util
    {
        /// <summary>
        /// 渡された数値をある範囲から別の範囲に変換
        /// </summary>
        /// <param name="value">変換する入力値</param>
        /// <param name="min1">現在の範囲の下限</param>
        /// <param name="max1">現在の範囲の上限</param>
        /// <param name="min2">変換する範囲の下限</param>
        /// <param name="max2">変換する範囲の上限</param>
        /// <returns>変換後の値</returns>
        public static float Map(float value, float min1, float max1, float min2, float max2)
        {
            return min2 + (max2 - min2) * ((value - min1) / (max1 - min1));
        }

        public static float RemapNumberClamped(float num, float low1, float high1, float low2, float high2)
        {
            return Mathf.Clamp(RemapNumber(num, low1, high1, low2, high2), Mathf.Min(low2, high2), Mathf.Max(low2, high2));
        }

        public static float RemapNumber(float num, float low1, float high1, float low2, float high2)
        {
            return low2 + (num - low1) * (high2 - low2) / (high1 - low1);
        }
    }
}