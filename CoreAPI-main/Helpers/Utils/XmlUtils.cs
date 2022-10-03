using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace WebApi.Helpers.Utils
{
    // ThaoTLH: tạm thời chỉ support các loại đơn giản. Sẽ cải tiến sau
    public static class XmlUtils
    {
        private static readonly ILogger _logger = Log.ForContext(typeof(XmlUtils));
        public static string XmlToJsonString(string xml, bool docroot, bool islowercase = false)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            return XmlToJsonString(doc, docroot, islowercase);
        }

        public static JObject XmlToJsonObject(string xml, bool docroot, bool islowercase = false)
        {
            string json = XmlToJsonString(xml, docroot, islowercase);
            JObject result = JObject.Parse(json);
            return result;
        }

        private static string XmlToJsonString(XmlDocument xmlDoc, bool docroot, bool islowercase = false)
        {
            StringBuilder sbJSON = new StringBuilder();
            if (docroot) sbJSON.Append("{");
            XmlToJSONnode(sbJSON, xmlDoc.DocumentElement, docroot, islowercase);
            if (docroot) sbJSON.Append("}");
            return sbJSON.ToString();
        }

        public static XmlDocument JSONToXmlVer1(string json)
        {
            XmlDocument document = new XmlDocument();
            using (var reader = JsonReaderWriterFactory.CreateJsonReader(Encoding.UTF8.GetBytes(json), XmlDictionaryReaderQuotas.Max))
            {
                XElement xml = XElement.Load(reader);
                document.LoadXml(xml.ToString());
            }
            return document;
        }

        public static XmlDocument JSONToXmlVer2(string json)
        {
            XmlDocument document = JsonConvert.DeserializeXmlNode(json);
            return document;
        }

        public static XmlDocument AddXmlDeclaration(XmlDocument xmlDocument)
        {
            XmlDocument docNew = new XmlDocument();
            XmlDeclaration declaration = docNew.CreateXmlDeclaration("1.0", "UTF-8", null);
            docNew.AppendChild(declaration);
            docNew.InnerXml = xmlDocument.DocumentElement.InnerXml;
            return docNew;
        }

        public static XmlDocument RemoveXmlDeclaration(XmlDocument xmlDocument)
        {
            if (xmlDocument.FirstChild.NodeType == XmlNodeType.XmlDeclaration)
                xmlDocument.RemoveChild(xmlDocument.FirstChild);
            return xmlDocument;
        }

        public static XmlDocument GenerateXML<T>(Object request, bool addPrefix = false, string prefixName = "", string prefixUrl = "")
        {
            var serializer = new XmlSerializer(typeof(T));
            XmlDocument document = new XmlDocument();
            if (addPrefix)
            {
                var xmlns = new XmlSerializerNamespaces();
                xmlns.Add(prefixName, prefixUrl);
                var sb = new StringBuilder();
                serializer.Serialize(new StringWriter(sb), request, xmlns);
                document.LoadXml(sb.ToString());
                if (document.FirstChild.NodeType == XmlNodeType.XmlDeclaration)
                    document.RemoveChild(document.FirstChild);
            }
            else
            {
                var sb = new StringBuilder();
                serializer.Serialize(new StringWriter(sb), request);
                document.LoadXml(sb.ToString());
                if (document.FirstChild.NodeType == XmlNodeType.XmlDeclaration)
                    document.RemoveChild(document.FirstChild);
            }
            return document;
        }

        public static XmlDocument AddAMLElement(XmlDocument request)
        {
            var soapPrefix = "soapenv";
            var soapNs = "http://schemas.xmlsoap.org/soap/envelope/";
            var xmlDoc = new XmlDocument();
            var envelope = xmlDoc.CreateElement(soapPrefix, "Envelope", soapNs);
            xmlDoc.AppendChild(envelope);
            var header = xmlDoc.CreateElement(soapPrefix, "Header", soapNs);
            envelope.AppendChild(header);
            var body = xmlDoc.CreateElement(soapPrefix, "Body", soapNs);
            envelope.AppendChild(body);
            body.InnerXml = request.InnerXml;
            return xmlDoc;
        }

        public static XmlDocument RemoveAMLElement(XmlDocument request, string firstRemove)
        {
            var xmlDoc = new XmlDocument();
            if (request.DocumentElement.FirstChild.Name.Equals(firstRemove))
            {
                xmlDoc.InnerXml = request.DocumentElement.FirstChild.InnerXml;
                return xmlDoc;
            }
            return request;
        }

        public static XmlDocument GetXmlNode(XmlDocument request, string position)
        {
            var xmlDoc = new XmlDocument();
            XmlNode nodes = request.GetElementsByTagName(position).Item(0);
            xmlDoc.InnerXml = nodes.InnerXml;
            return request;
        }

        public static T GenerateObject<T>(string request) where T : class
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (TextReader reader = new StringReader(request))
            {
                return (T)serializer.Deserialize(reader);
            }
        }

        private static void XmlToJSONnode(StringBuilder sbJSON, XmlElement node, bool showNodeName, bool islowercase = false)
        {
            bool isArray = false;
            if (showNodeName)
                if (islowercase)
                {
                    sbJSON.Append("\"" + SafeJSON(node.Name.ToLower()) + "\": ");
                }
                else
                {
                    sbJSON.Append("\"" + SafeJSON(node.Name) + "\": ");
                }
            sbJSON.Append("{");

            SortedList<string, object> childNodeNames = new SortedList<string, object>();

            ////  thêm node attribute. Tạm thời disable
            //if (node.Attributes != null)
            //    foreach (XmlAttribute attr in node.Attributes)
            //        StoreChildNode(childNodeNames, attr.Name, attr.InnerText);

            foreach (XmlNode cnode in node.ChildNodes)
            {
                if (cnode is XmlText)
                    StoreChildNode(childNodeNames, "value", cnode.InnerText);
                else if (cnode is XmlElement)
                    if (islowercase)
                    {
                        StoreChildNode(childNodeNames, cnode.Name.ToLower(), cnode);
                    }
                    else
                    {
                        StoreChildNode(childNodeNames, cnode.Name, cnode);
                    }
            }

            foreach (string childname in childNodeNames.Keys)
            {
                List<object> alChild = (List<object>)childNodeNames[childname];
                if (alChild.Count == 1)
                    OutputNode(childname, alChild[0], sbJSON, true, islowercase);
                else
                {
                    if (alChild[0].GetType() == typeof(XmlNode))
                    {
                        XmlNode test = (XmlNode)alChild[0];
                        if (childname.Equals("item") && test.ParentNode.ChildNodes.Count == alChild.Count)
                        {
                            int length = 5 + test.ParentNode.Name.Length;
                            sbJSON.Remove(sbJSON.Length - length, length);
                            if (islowercase)
                            {
                                sbJSON.Append(" \"" + SafeJSON(test.ParentNode.Name.ToLower()) + "\": [ ");
                            }
                            else
                            {
                                sbJSON.Append(" \"" + SafeJSON(test.ParentNode.Name) + "\": [ ");
                            }
                            foreach (object Child in alChild)
                                if (islowercase)
                                {
                                    OutputNode(test.ParentNode.Name.ToLower(), Child, sbJSON, false, islowercase);
                                }
                                else
                                {
                                    OutputNode(test.ParentNode.Name, Child, sbJSON, false, islowercase);
                                }
                            sbJSON.Remove(sbJSON.Length - 2, 2);
                            sbJSON.Append(" ], ");
                            isArray = true;
                        }
                        else
                        {
                            sbJSON.Append(" \"" + SafeJSON(childname) + "\": [ ");
                            foreach (object Child in alChild)
                                OutputNode(childname, Child, sbJSON, false, islowercase);
                            sbJSON.Remove(sbJSON.Length - 2, 2);
                            sbJSON.Append(" ], ");
                        }
                    }
                    else
                    {
                        sbJSON.Append(" \"" + SafeJSON(childname) + "\": [ ");
                        foreach (object Child in alChild)
                            OutputNode(childname, Child, sbJSON, false, islowercase);
                        sbJSON.Remove(sbJSON.Length - 2, 2);
                        sbJSON.Append(" ], ");
                    }
                }
            }
            sbJSON.Remove(sbJSON.Length - 2, 2);
            if (!isArray) sbJSON.Append(" }");
        }

        private static void StoreChildNode(SortedList<string, object> childNodeNames, string nodeName, object nodeValue)
        {
            if (nodeValue is XmlElement)
            {
                XmlNode cnode = (XmlNode)nodeValue;
                if (cnode.Attributes.Count == 0)
                {
                    XmlNodeList children = cnode.ChildNodes;
                    if (children.Count == 0)
                        nodeValue = null;
                    else if (children.Count == 1 && (children[0] is XmlText))
                        nodeValue = ((XmlText)(children[0])).InnerText;
                }
            }
            List<object> ValuesAL;

            if (childNodeNames.ContainsKey(nodeName))
            {
                ValuesAL = (List<object>)childNodeNames[nodeName];
            }
            else
            {
                ValuesAL = new List<object>();
                childNodeNames[nodeName] = ValuesAL;
            }
            ValuesAL.Add(nodeValue);
        }

        private static void OutputNode(string childname, object alChild, StringBuilder sbJSON, bool showNodeName, bool islowercase = false)
        {
            if (alChild == null)
            {
                if (showNodeName)
                    sbJSON.Append("\"" + SafeJSON(childname) + "\": ");
                sbJSON.Append("null");
            }
            else if (alChild is string)
            {
                if (showNodeName)
                    sbJSON.Append("\"" + SafeJSON(childname) + "\": ");
                string sChild = (string)alChild;
                sChild = sChild.Trim();
                sbJSON.Append("\"" + SafeJSON(sChild) + "\"");
            }
            else
            {
                XmlElement test = (XmlElement)alChild;
                if (test.ChildNodes.Count == 1 && test.ChildNodes.Item(0).NodeType == XmlNodeType.Text)
                {
                    if (showNodeName)
                        sbJSON.Append("\"" + SafeJSON(childname) + "\": ");
                    string sChild = test.InnerText;
                    sChild = sChild.Trim();
                    sbJSON.Append("\"" + SafeJSON(sChild) + "\"");
                }
                else if (test.ChildNodes.Count == 0)
                {
                    if (showNodeName)
                        sbJSON.Append("\"" + SafeJSON(childname) + "\": ");
                    string sChild = test.InnerText;
                    sChild = sChild.Trim();

                    if (test.Attributes.Count > 0)
                    {
                        var a = test.Attributes.GetNamedItem("type");
                        if (a != null && a.InnerText.Equals("array")) sbJSON.Append("[" + sChild + "]");
                        else sbJSON.Append("\"" + sChild + "\"");
                    }
                    else
                    {
                        sbJSON.Append("\"" + sChild + "\"");
                    }
                }
                else
                {
                    XmlToJSONnode(sbJSON, (XmlElement)alChild, showNodeName, islowercase);
                }
            }
            sbJSON.Append(", ");
        }
        // loại bỏ các spec char nguy hiểm
        private static string SafeJSON(string sIn)
        {
            StringBuilder sbOut = new StringBuilder(sIn.Length);
            foreach (char ch in sIn)
            {
                if (Char.IsControl(ch) || ch == '\'')
                {
                    int ich = (int)ch;
                    sbOut.Append(@"\u" + ich.ToString("x4"));
                    continue;
                }
                else if (ch == '\"' || ch == '\\' || ch == '/')
                {
                    sbOut.Append('\\');
                }
                sbOut.Append(ch);
            }
            return sbOut.ToString();
        }

        public static void AnalyzeXMLNode(JObject jObject, XmlNode node)
        {
            try
            {
                while (node != null)
                {
                    if (node.HasChildNodes && node.FirstChild.NodeType != XmlNodeType.Text)
                    {
                        AnalyzeXMLNode(jObject, node.FirstChild);
                    }
                    else
                    {
                        jObject.Add(node.Name.ToLower(), node.InnerText);
                    }
                    node = node.NextSibling;
                }

            }
            catch
            {

            }
        }

        public static XmlNode GetXMLNodeByPath(XmlNode node, string pathget, string currentpath = "")
        {
            try
            {
                string pathnew = "";
                if (string.IsNullOrEmpty(currentpath))
                {
                    currentpath = node.Name;
                }
                while (node != null)
                {
                    if (node.HasChildNodes && node.FirstChild.NodeType != XmlNodeType.Text)
                    {
                        currentpath += "/" + node.Name;
                        if (currentpath.Substring(currentpath.Length - pathget.Length) == pathget)
                        {
                            return node;
                        }
                        return GetXMLNodeByPath(node.FirstChild, pathget, currentpath);
                    }
                    node = node.NextSibling;
                    if (node != null)
                    {
                        pathnew = currentpath + "/" + node.Name;
                        if (pathnew.Substring(pathnew.Length - pathget.Length) == pathget)
                        {
                            return node;
                        }
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public static bool IsValidXml(string xml)
        {
            try
            {
                new XmlDocument().LoadXml(xml);
                return true;
            }
            catch(Exception ex)
            {
                _logger.Error(ex.ToString());
                return false;
            }
        }

        public static JObject ConvertXMLToJSON(XmlNode xmlNode, bool isIgnoredRootNode = false)
        {
            JObject reponse = new JObject();
            try
            {
                if (isIgnoredRootNode)
                {
                    string jsonConvertText = JsonConvert.SerializeXmlNode(xmlNode, Newtonsoft.Json.Formatting.None, isIgnoredRootNode);
                    reponse = JObject.Parse(jsonConvertText);
                    return reponse;
                }
                else
                {
                    string jsonConvertText = JsonConvert.SerializeXmlNode(xmlNode);
                    reponse = JObject.Parse(jsonConvertText);
                    return reponse;
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex.ToString());
                return reponse;
            }
        }

        public static string Beautify(this XmlDocument doc)
        {
            string result = "";
            Encoding utf8noBOM = new UTF8Encoding(false);
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                Encoding = utf8noBOM
            };
            using (MemoryStream output = new MemoryStream())
            {
                using (XmlWriter writer = XmlWriter.Create(output, settings))
                {
                    doc.Save(writer);
                }
                result = Encoding.UTF8.GetString(output.GetBuffer());
            }
            return result;
        }

        public static string FormatEscapeXMLContent(string content)
        {
            if (string.IsNullOrEmpty(content)) return content;
            return content.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("&#34;", "&quot;").Replace("'", "&apos;");
        }
    }
}
