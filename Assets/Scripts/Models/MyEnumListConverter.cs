using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;

public class MyEnumListConverter<T> : JsonConverter<List<T>> where T :struct,Enum
    {
    public override List<T> ReadJson(JsonReader reader, Type objectType, List<T> existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        List<T> listEnum = new List<T>();

        List<string> listString = serializer.Deserialize<List<string>>(reader);
        foreach(string stringEnum in listString)
        {
            if(Enum.TryParse(stringEnum,true,out T enumValue)){
                listEnum.Add(enumValue);
            }
            else
            {
                throw new JsonSerializationException($"cannot convert {stringEnum} ");
            }
        }
        return listEnum;
    }

    public override void WriteJson(JsonWriter writer, List<T> value, JsonSerializer serializer)
    {
        List<string> listString = value.ConvertAll(e => e.ToString());
        serializer.Serialize(writer, listString);
    }
}
