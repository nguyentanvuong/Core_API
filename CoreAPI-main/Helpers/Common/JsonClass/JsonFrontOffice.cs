using Newtonsoft.Json.Linq;
using WebApi.Helpers.Utils;

namespace WebApi.Helpers.Common
{
    public class JsonFrontOffice : JsonHeader
    {
        public JObject TXBODY { get; set; }
        public JsonPosting POSTING { get; set; }

        public JObject IFCFEE { get; set; }
        public JObject RESULT { get; set; }
        public JObject IBRET { get; set; }
        public JObject DATASET { get; set; }

        public JsonFrontOffice()
        {

        }
        public JsonFrontOffice(JsonFrontOfficeMapping jsonFrontOfficeMapping)
        {
            if (jsonFrontOfficeMapping != null)
            {
                TXCODE = jsonFrontOfficeMapping.A;
                TXDT = jsonFrontOfficeMapping.B;
                TXREFID = jsonFrontOfficeMapping.C;
                VALUEDT = jsonFrontOfficeMapping.D;
                BRANCHID = jsonFrontOfficeMapping.E;
                USRID = jsonFrontOfficeMapping.F;
                LANG = jsonFrontOfficeMapping.G;
                USRWS = jsonFrontOfficeMapping.H;
                APUSER = jsonFrontOfficeMapping.I;
                APUSRIP = jsonFrontOfficeMapping.J;
                APUSRWS = jsonFrontOfficeMapping.K;
                APDT = jsonFrontOfficeMapping.L;
                STATUS = jsonFrontOfficeMapping.M;
                ISREVERSE = jsonFrontOfficeMapping.N;
                HBRANCHID = jsonFrontOfficeMapping.O;
                RBRANCHID = jsonFrontOfficeMapping.P;
                APREASON = jsonFrontOfficeMapping.Q;
                PRN = jsonFrontOfficeMapping.R;
                TXBODY = ConvertMappingToOriginal(jsonFrontOfficeMapping.A, jsonFrontOfficeMapping.S);
                if (jsonFrontOfficeMapping.T != null) POSTING = new JsonPosting(jsonFrontOfficeMapping.T);
                RESULT = jsonFrontOfficeMapping.V;
                DATASET = jsonFrontOfficeMapping.W;
                ID = jsonFrontOfficeMapping.ID;
                IBRET = jsonFrontOfficeMapping.IBRET;
            }
        }

        public JObject ConvertMappingToOriginal(string txcode, JObject txbody)
        {
            if (txcode == null || !txcode.Equals("")) return null;
            JObject jsMapField = O9Utils.GetMapFieldFrontOffice(txcode);
            if (jsMapField != null && jsMapField.Count > 0)
            {
                JObject jsOriginal = new JObject();
                bool isHasItem = false;

                foreach (var jsValue in txbody)
                {
                    isHasItem = false;
                    foreach (var jsMap in jsMapField)
                    {
                        if (jsMap.Value != null && jsMap.Value.GetType() == typeof(JValue) && ((JValue)jsMap.Value).Value.Equals(jsValue.Key)){
                            jsOriginal.Add(jsMap.Key, jsValue.Value);
                            isHasItem = true;
                            break;
                        }
                    }
                    if (!isHasItem) jsOriginal.Add(jsValue.Key, jsValue.Value);
                }
                return jsOriginal;
            }
            return txbody;
        }
    }


    public class JsonFrontOfficeMapping : JsonHeaderMapping
    {
        public JObject S { get; set; }
        public JsonPostingMapping T { get; set; }
        public JObject U { get; set; }
        public JObject V { get; set; }
        public JObject W { get; set; }
        public JObject IBRET { get; set; }


        public JsonFrontOfficeMapping()
        {

        }
        public JsonFrontOfficeMapping(JsonFrontOffice jsonFrontOffice)
        {
            if (jsonFrontOffice != null)
            {
                A = jsonFrontOffice.TXCODE;
                B = jsonFrontOffice.TXDT;
                C = jsonFrontOffice.TXREFID;
                D = jsonFrontOffice.VALUEDT;
                E = jsonFrontOffice.BRANCHID;
                F = jsonFrontOffice.USRID;
                G = jsonFrontOffice.LANG;
                H = jsonFrontOffice.USRWS;
                I = jsonFrontOffice.APUSER;
                J = jsonFrontOffice.APUSRIP;
                K = jsonFrontOffice.APUSRWS;
                L = jsonFrontOffice.APDT;
                M = jsonFrontOffice.STATUS;
                N = jsonFrontOffice.ISREVERSE;
                O = jsonFrontOffice.HBRANCHID;
                P = jsonFrontOffice.RBRANCHID;
                Q = jsonFrontOffice.APREASON;
                R = jsonFrontOffice.PRN;
                S = ConvertOriginalToMapping(jsonFrontOffice.TXCODE, jsonFrontOffice.TXBODY);
                if (jsonFrontOffice.POSTING != null) T = new JsonPostingMapping(jsonFrontOffice.POSTING);
                V = jsonFrontOffice.RESULT;
                W = jsonFrontOffice.DATASET;
                ID = jsonFrontOffice.ID;
                IBRET = jsonFrontOffice.IBRET;
            }
        }

        private JObject ConvertOriginalToMapping (string txcode, JObject txbody)
        {
            if (string.IsNullOrEmpty(txcode)) return null;
            JObject jsMapField = O9Utils.GetMapFieldFrontOffice(txcode);
            if (jsMapField != null && jsMapField.Count > 0)
            {
                JObject jsMapping = new JObject();
                foreach (var jsValue in txbody)
                {
                    if (O9Utils.JsonContains(jsMapField, jsValue.Key))
                    {
                        jsMapping.Add( ((JValue)jsMapField.SelectToken(jsValue.Key)).Value.ToString(), jsValue.Value);
                    }
                    else
                    {
                        jsMapping.Add(jsValue.Key, jsValue.Value);
                    }
                }
                return jsMapping;
            }
            return txbody;
        }
    }

    public class JsonFrontOfficeResponse
    {
        public EnmResultResponse EnmResultResp { get; set; } = EnmResultResponse.NOT_SUCCESS;
        public string strReason { get; set; }
        public JsonFrontOffice clsJsonFrontOffice { get; set; }

    }

}