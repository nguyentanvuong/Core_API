using System.Xml;

namespace WebApi.Models.FASTPublic
{
    public class SOAPReponse
    {
        public static XmlDocument GetResult(string cdata)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement element = doc.CreateElement("return");
            XmlCDataSection cdataxml = doc.CreateCDataSection(cdata);
            element.AppendChild(cdataxml);
            doc.AppendChild(element);
            return doc;
        }

        public static XmlDocument GetError(string error)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement element = doc.CreateElement("return");
            element.InnerText = error;
            doc.AppendChild(element);
            return doc;
        }
    }

}
