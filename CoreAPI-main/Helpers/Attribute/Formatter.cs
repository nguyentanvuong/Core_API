using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using WebApi.Helpers.Utils;

namespace WebApi.Helpers
{
    public class CustomXmlOutputFormatter : TextOutputFormatter
    {
        public string ContentType { get; }

        public CustomXmlOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/xml"));
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            try
            {
                var serviceProvider = context.HttpContext.RequestServices;
                var response = context.HttpContext.Response;

                string content = JsonConvert.SerializeObject(context.Object);
                //var result = JsonConvert.DeserializeXmlNode(content, "Response");
                XmlDocument doc = new XmlDocument();

                using (var reader = JsonReaderWriterFactory.CreateJsonReader(Encoding.UTF8.GetBytes(content), XmlDictionaryReaderQuotas.Max))
                {
                    XElement xml = XElement.Load(reader);
                    doc.LoadXml(xml.ToString());
                }
                XmlDocument docNew = new XmlDocument();
                XmlDeclaration declaration = docNew.CreateXmlDeclaration("1.0", "UTF-8", null);
                docNew.AppendChild(declaration);
                XmlElement newRoot = docNew.CreateElement("Response");
                docNew.AppendChild(newRoot);
                newRoot.InnerXml = doc.DocumentElement.InnerXml;
                return response.WriteAsync(docNew.InnerXml);
            }
            catch (Exception ex)
            {
                throw new InputFormatterException("Error when processing", ex);
            }

        }
    }

    public class CustomXmlInputFormatter : TextInputFormatter
    {
        #region ctor
        public CustomXmlInputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/xml"));
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }
        #endregion

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            var request = context.HttpContext.Request;
            try
            {
                var reader = new StreamReader(request.Body, encoding);
                var modelType = context.ModelType;
                var requestBody = await reader.ReadToEndAsync();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(requestBody);

                if (doc.FirstChild.NodeType == XmlNodeType.XmlDeclaration)
                    doc.RemoveChild(doc.FirstChild);

                if (!(doc.FirstChild.LocalName.Equals(modelType.Name) || doc.FirstChild.LocalName.Equals("Request")))
                {
                    throw new InputFormatterException("Error when processing. Please check your format message");
                }
                string json = XmlUtils.XmlToJsonString(doc.InnerXml, false);
                object model = JsonConvert.DeserializeObject(json, modelType);
                return InputFormatterResult.Success(model);
            }

            catch (Exception exception)
            {
                throw new InputFormatterException("Error when processing. Please check your format message", exception);
            }

        }

        private async Task<string> ReadLineAsync(string expectedText, StreamReader reader, InputFormatterContext context)
        {
            var line = await reader.ReadLineAsync();
            if (!line.StartsWith(expectedText))
            {
                var errorMessage = $"Looked for '{expectedText}' and got '{line}'";
                context.ModelState.TryAddModelError(context.ModelName, errorMessage);
                throw new Exception(errorMessage);
            }
            return line;
        }
    }
}
