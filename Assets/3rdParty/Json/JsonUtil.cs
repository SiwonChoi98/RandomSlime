using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public static class JsonUtil
{
    private static string RemoveTypeRow(string jsonText)
    {
        JObject specData = JObject.Parse(jsonText);
        foreach (JProperty jProperty in specData.Properties())
        {
            JToken array = jProperty.FirstOrDefault();
            if (array == null) continue;

            JToken firstLine = array.FirstOrDefault();
            if (firstLine == null) continue;

            if (firstLine.Value<string>("id") == null) continue;

            string typeLineKey = firstLine.Value<string>("id");
            bool hasTypeInfo = typeLineKey == "0" || typeLineKey == "int";

            if (hasTypeInfo)
            {
                (array as JArray).RemoveAt(0);
            }
        }
        return specData.ToString();
    }

    public static TValue Deserialize<TValue>(string jsonText, bool removeTypeRow = false)
    {
        //  TODO : DEV일때만 동작하도록 처리 필요
        if (removeTypeRow)
        {
            jsonText = RemoveTypeRow(jsonText);
        }

        return JsonConvert.DeserializeObject<TValue>(jsonText);
    }

    public static void Deserialize<TValue>(string jsonText, bool removeTypeRow, Action<TValue> onComplte)
    {
        UnityEngine.Debug.Log("1");
        //  TODO : DEV일때만 동작하도록 처리 필요
        if (removeTypeRow)
        {
            jsonText = RemoveTypeRow(jsonText);
        }

        UnityEngine.Debug.Log("2");
        onComplte.Invoke(JsonConvert.DeserializeObject<TValue>(jsonText));
        UnityEngine.Debug.Log("3");
    }
}