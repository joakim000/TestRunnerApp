using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Lib
{
    /***************************************************************************************

    Support classes for deserialising objects with abstract classes, by Andreas Niedermair
    Ref. https://stackoverflow.com/questions/20995865/deserializing-json-to-abstract-class

    ***************************************************************************************/

    /*

    public class BaseSpecifiedConcreteClassConverter : DefaultContractResolver
    {
        protected override JsonConverter ResolveContractConverter(Type objectType)
        {
            if (typeof(MediaItem).IsAssignableFrom(objectType) && !objectType.IsAbstract)
                return null; // pretend TableSortRuleConvert is not specified (thus avoiding a stack overflow)
            return base.ResolveContractConverter(objectType);
        }
    }

    public class BaseConverter : JsonConverter
    {
        static JsonSerializerSettings SpecifiedSubclassConversion = new JsonSerializerSettings() { ContractResolver = new BaseSpecifiedConcreteClassConverter() };

        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(MediaItem));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = new JObject();
            try
            {
                jo = JObject.Load(reader);
            }
            catch (Exception jsonException)
            {
                System.Diagnostics.Debug.WriteLine($"Failure on reading json, exception: {jsonException}");
            }

            try
            {
                switch (jo["type"].Value<string>())
                {
                    case "Music":
                        return JsonConvert.DeserializeObject<AudioItem>(jo.ToString(), SpecifiedSubclassConversion);
                    case "Book":
                        return JsonConvert.DeserializeObject<BookItem>(jo.ToString(), SpecifiedSubclassConversion);
                    case "Film":
                        return JsonConvert.DeserializeObject<FilmItem>(jo.ToString(), SpecifiedSubclassConversion);
                    case "Game":
                        return JsonConvert.DeserializeObject<GameItem>(jo.ToString(), SpecifiedSubclassConversion);
                    default:
                        throw new Exception();
                }
            }
            catch (Exception jsonException)
            {
                System.Diagnostics.Debug.WriteLine($"Failure on deserialize, exception: {jsonException}");
            }
            //throw new NotImplementedException();
            //return "Failure on deserialize.";
            return new AudioItem();
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException(); // won't be called because CanWrite returns false
        }
    }
*/
}
