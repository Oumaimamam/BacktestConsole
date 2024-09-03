using System;
using System.Collections.Generic;

public static class DictionaryUtils
{
    public static double ScalarProduct(Dictionary<string, double> dict1, Dictionary<string, double> dict2)
    {
        double product = 0.0;

        foreach (var key in dict1.Keys)
        {
            if (dict2.ContainsKey(key))
            {
                product += dict1[key] * dict2[key];
            }
        }

        return product;
    }
}