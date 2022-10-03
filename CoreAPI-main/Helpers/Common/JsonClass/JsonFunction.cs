using Newtonsoft.Json.Linq;

namespace WebApi.Helpers.Common
{
    public class JsonFunction
    {
        public string TXCODE { get; set; }
        public string TXPROC { get; set; }
        public JObject TXBODY { get; set; }
        public JObject RESULT { get; set; }

        public JsonFunction()
        {

        }

        public JsonFunction(JsonFunctionMapping jsonFunctionMapping)
        {
            if (jsonFunctionMapping != null)
            {
                TXCODE = jsonFunctionMapping.A;
                TXPROC = jsonFunctionMapping.B;
                TXBODY = jsonFunctionMapping.S;
                RESULT = jsonFunctionMapping.V;
            }
        }
    }

    public class JsonFunctionMapping
    {
        public string A { get; set; }
        public string B { get; set; }
        public JObject S { get; set; }
        public JObject V { get; set; }

        public JsonFunctionMapping()
        {

        }

        public JsonFunctionMapping(JsonFunction jsonFunction)
        {
            if (jsonFunction != null)
            {
                A = jsonFunction.TXCODE;
                B = jsonFunction.TXPROC;
                S = jsonFunction.TXBODY;
                V = jsonFunction.RESULT;
            }
        }
    }
}
