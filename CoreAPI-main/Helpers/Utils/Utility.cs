using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using WebApi.Helpers.Common;

namespace WebApi.Helpers.Utils
{
    public static class Utility
    {
        public static string GenPassword(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%+";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, length)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return result;
        }

        public static string GenAuthenticataCode(int length)
        {
            var chars = "0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, length)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return result;
        }

        public static DateTime ConvertStringToDateTime(string datetimeString)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            DateTime dateTime;
            try
            {
                dateTime = DateTime.ParseExact(datetimeString, GlobalVariable.DatetimeFormat, provider);
            }
            catch 
            { 
                dateTime = DateTime.Parse(datetimeString);
            }
            return dateTime;
        }

        public static string ConvertDateTimeToString(DateTime datetime)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            return datetime.ToString("dd/MM/yyyy", provider);
        }

        public static string ConvertDateTimeToStringDatetime(DateTime datetime, string format = "dd/MM/yyyy HH:mm:ss")
        {
            if (string.IsNullOrEmpty(format)) format = "dd/MM/yyyy HH:mm:ss";
            CultureInfo provider = CultureInfo.InvariantCulture;
            return datetime.ToString(format, provider);
        }

        public static bool IsDate(string tempDate)
        {
            if (DateTime.TryParseExact(tempDate, GlobalVariable.DatetimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        ///Sửa chuỗi theo format Captalize
        /// </summary>
        public static string MakeCaptalizeCase(this string input)
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException("String input is null or empty");
            return input.First().ToString().ToUpper() + input[1..].ToLower();
        }

        /// <summary>
        ///Lấy dữ liệu từ JObject đưa vào Entity
        /// </summary>
        public static object SetValueObject(JObject jObject, object obj)
        {
            var properties = obj.GetType().GetProperties();
            foreach (KeyValuePair<string, JToken> keyValuePair in jObject)
            {
                string key = keyValuePair.Key;
                var objproperty = properties.SingleOrDefault(x => x.Name.ToLower() == key.ToLower());
                if (objproperty != null)
                {
                    JToken value = keyValuePair.Value;
                    Type typeprop = objproperty.PropertyType;
                    if (typeprop == typeof(DateTime) || typeprop == typeof(DateTime?))
                    {
                        if (typeprop == typeof(DateTime?) && value.Type == JTokenType.Null) continue;
                        if (!Utility.IsDate(value.ToString()) && value.Type != JTokenType.Date)
                        {
                            throw new FormatException("Wrong format datetime in field [" + objproperty.Name + "]");
                        }
                        else if (value.Type == JTokenType.Date)
                        {
                            objproperty.SetValue(obj, string.IsNullOrEmpty(value.ToString()) ? null : DateTime.Parse(value.ToString()));
                        }
                        else
                        {
                            objproperty.SetValue(obj, string.IsNullOrEmpty(value.ToString()) ? null : Utility.ConvertStringToDateTime(value.ToString()));
                        }
                    }
                    else if (typeprop == typeof(string))
                    {
                        objproperty.SetValue(obj, string.IsNullOrEmpty(value.ToString()) ? "" : Convert.ChangeType(value, typeprop), null);
                    }
                    else
                    {
                        objproperty.SetValue(obj, string.IsNullOrEmpty(value.ToString()) ? null : Convert.ChangeType(value, typeprop), null);
                    }
                }
                else
                {
                    throw new FormatException("No have key [" + key + "] in destination object.");
                }
            }
            return obj;
        }

        /// <summary>
        ///Format lại JOject để trả kết quả
        /// </summary>
        public static JObject FromatJObject(object obj, string[] removefields = null)
        {
            try
            {
                var properties = obj.GetType().GetProperties();
                JObject result = JObject.FromObject(obj);
                foreach (var prop in properties)
                {
                    Type typeprop = prop.PropertyType;
                    if (typeprop == typeof(DateTime) || typeprop == typeof(DateTime?))
                    {
                        object valueprop = prop.GetValue(obj);
                        if (valueprop != null)
                        {
                            string nameprop = prop.Name;
                            result[nameprop] = Utility.ConvertDateTimeToStringDatetime((DateTime)valueprop);
                        }
                    }

                }
                if (removefields != null)
                {
                    foreach (string field in removefields)
                    {
                        result.Remove(field);
                    }
                }
                return result;
            }
            catch
            {
                return null;
            }
        }

        public static JArray FormatKeyJArray(JArray jArray)
        {
            if (jArray.Count == 0) return jArray;
            JArray result = new JArray();
            foreach (var item in jArray.Children())
            {
                JObject jObject = JObject.FromObject(item);
                JObject obj = new JObject();
                foreach (KeyValuePair<string, JToken> keyValuePair in jObject)
                {
                    string key = keyValuePair.Key;
                    JToken value = keyValuePair.Value;
                    obj.Add(key.ToLower(), value);
                }
                result.Add(obj);
            }
            return result;
        }

        /// <summary>
        ///Lấy giá trị của phần tử trong JObject
        /// </summary>
        public static string GetValueJObjectByKey(this JObject jObject, string keyName)
        {
            return jObject.GetValue(keyName, StringComparison.OrdinalIgnoreCase)?.Value<string>();
        }

        public static object GetValueObjectFromName(object obj, string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name) || obj == null)
                {
                    return null;
                }
                string[] names = name.Split(".");
                if (names.Length > 1)
                {
                    object child = obj.GetType().GetProperty(names[0]).GetValue(obj, null);
                    for (int i = 1; i < names.Length; i++)
                    {
                        child = child.GetType().GetProperty(names[i]).GetValue(child, null);
                    }
                    return child;
                }
                else
                {
                    return obj.GetType().GetProperty(name).GetValue(obj, null);
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Tạo 1 object mới từ tên của nó
        /// </summary>
        /// <param name="strFullyQualifiedName"></param>
        /// <returns>object</returns>
        public static object GetInstance(string strFullyQualifiedName)
        {
            Type type = Type.GetType(strFullyQualifiedName);
            if (type != null)
                return Activator.CreateInstance(type);
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = asm.GetType(strFullyQualifiedName);
                if (type != null)
                    return Activator.CreateInstance(type);
            }
            return null;
        }

    }
}
