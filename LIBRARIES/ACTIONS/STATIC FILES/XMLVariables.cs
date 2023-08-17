using System;
using System.IO;
using System.Net;
using System.Xml.Serialization;

namespace ACTIONS.STATICFILES
{
    public class XMLClass
    {
        [XmlElement("SMTPServer")]
        public XMLVariables<string> SMTPServer { get; set; }

        [XmlElement("SMTPUsername")]
        public XMLVariables<string> SMTPUsername { get; set; }

        [XmlElement("SMTPPassword")]
        public XMLVariables<string> SMTPPassword { get; set; }

    }
	public class XMLVariables<T>
	{

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
            var path = Directory.GetCurrentDirectory();
            var path2 = Environment.CurrentDirectory;
            Console.WriteLine(path);
            Console.WriteLine(path2);

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
                XMLClass po;
                // Uses the Deserialize method to restore the object's state
                // with data from the XML document. */
                po = (XMLClass)serializer.Deserialize(fs);

                // Reads the order date.
                //Console.WriteLine("SMTPServer: " + po.SMTPServer + $" and {po.SMTPServer.Value}");
                //Console.WriteLine("SMTPUsername: " + po.SMTPUsername + $" and {po.SMTPUsername.Value}");
                //Console.WriteLine("SMTPPassword: " + po.SMTPPassword + $" and {po.SMTPPassword.Value}");

                // Reads the shipping address.
                a[0] = po.SMTPServer.Value;
                a[1] = po.SMTPUsername.Value;
                a[2] = po.SMTPPassword.Value;
                
            }
            return a;
		}
	}
}

