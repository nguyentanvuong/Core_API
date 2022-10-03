using Newtonsoft.Json.Linq;
using System.Data;
using Newtonsoft.Json;
using System;
using System.Globalization;

namespace WebApi.Helpers.Common.JsonClass
{
    public class JsonDataSet
    {
        public JArray DATASET { get; set; }
        public JsonDataSet()
        {
            DATASET = new JArray();
        }
    }

    public class JsonDataSetConverter
    {
        private JObject JsDataSet { get; set; }
        private DataSet DATASET { get; set; } = null;


        public DataSet ConvertToDataSet(JObject jsDataSet)
        {
            JsDataSet = jsDataSet;
            DATASET = ConvertToDataSet();
            return DATASET;
        }

        public bool CheckValueIsDate(string value,out DateTime dt)
        {
            if(DateTime.TryParseExact(value, GlobalVariable.FORMAT_SHORT_DATE, CultureInfo.InvariantCulture, DateTimeStyles.None,out dt))
            {
                return true;
            }
            return false;
        }

        private DataSet ConvertToDataSet()
        {
            int max = 0;
            JObject jsObj = null;
            JsonDataSet clsJsonDataSet = new JsonDataSet();
            DataSet ds = null;
            int count = 0;
            DateTime dt;

            if (JsDataSet != null && JsDataSet.Count > 0)
            {
                foreach (JProperty p in JsDataSet.Properties())
                {
                    if (p.Value.GetType() == typeof(JArray))
                    {
                        JArray array = (JArray)p.Value;
                        if (array.Count > 0)
                        {
                            max = array.Count;
                            break;
                        }
                    }
                }
            }

            for (int i = max - 1; i >= 0; i--)
            {
                count = 0;
                jsObj = null;
                jsObj = new JObject();
                foreach (JProperty p in JsDataSet.Properties())
                {
                    if (p.Value.GetType() == typeof(JArray))
                    {
                        JArray array = (JArray)p.Value;
                        if (array.Count > 0)
                        {
                            JValue str = (JValue)array[i];
                            if (CheckValueIsDate(str.Value.ToString(),out dt))
                            {
                                if (!string.IsNullOrEmpty(p.Name)) jsObj.Add(p.Name, new JValue(dt));
                                else jsObj.Add(count.ToString(), new JValue(dt));
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(p.Name)) jsObj.Add(p.Name, str);
                                else jsObj.Add(count.ToString(), str);
                            }
                        }
                    }
                    count += 1;
                }
                clsJsonDataSet.DATASET.Add(jsObj);
            }
            ds = JsonConvert.DeserializeObject<DataSet>(JsonConvert.SerializeObject(clsJsonDataSet));
            clsJsonDataSet = null;
            return ds;
        }
    }
}
