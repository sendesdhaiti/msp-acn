using System;
using System.IO;
using System.Net;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Hosting;

namespace ACTIONS.STATICFILES
{
    public class XMLClass
    {
        [XmlElement("SMTPServer")]
        public XMLVariables<string>? SMTPServer { get; set; }

        [XmlElement("SMTPUsername")]
        public XMLVariables<string>? SMTPUsername { get; set; }

        [XmlElement("SMTPPassword")]
        public XMLVariables<string>? SMTPPassword { get; set; }

    }
	public class XMLVariables<T>
	{
        private IWebHostEnvironment _hostingEnvironment;
        public XMLVariables(T t, IWebHostEnvironment h) {
            _hostingEnvironment = h;
            Value = t;
        }

        [XmlAttribute]
        public T Value { get; set; }

        protected void serializer_UnknownNode(object sender, XmlNodeEventArgs e)
        {
            Console.WriteLine("Unknown Node:" + e.Name + "\t" + $"'{e.Text}'");
        }

        protected void serializer_UnknownAttribute(object sender, XmlAttributeEventArgs e)
        {
            System.Xml.XmlAttribute attr = e.Attr;
            Console.WriteLine("Unknown attribute " +
            attr.Name + "='" + attr.Value + "'");
        }

        public string[] GetXML_EmailVariables()
		{
            string[] a = new string[3];
            // A FileStream is needed to read the XML document.
            var path = _hostingEnvironment.ContentRootPath;
            Console.WriteLine(path);

            using (FileStream fs = new FileStream(path + "/Content/Variables.xml", FileMode.OpenOrCreate))
			{
                XmlSerializer serializer = new XmlSerializer(typeof(XMLClass));
                // If the XML document has been altered with unknown
                // nodes or attributes, handles them with the
                // UnknownNode and UnknownAttribute events.
                serializer.UnknownNode += new
                    XmlNodeEventHandler(serializer_UnknownNode);
                serializer.UnknownAttribute += new
                    XmlAttributeEventHandler(serializer_UnknownAttribute);

                // Declares an object variable of the type to be deserialized.
                XMLClass? po;
                // Uses the Deserialize method to restore the object's state
                // with data from the XML document. */
                po = (XMLClass?)serializer.Deserialize(fs);

                // Reads the shipping address.
                a[0] = po?.SMTPServer?.Value ?? "";
                a[1] = po?.SMTPUsername?.Value ?? "";
                a[2] = po?.SMTPPassword?.Value ?? "";
                
            }
            return a;
		}
	}
}

