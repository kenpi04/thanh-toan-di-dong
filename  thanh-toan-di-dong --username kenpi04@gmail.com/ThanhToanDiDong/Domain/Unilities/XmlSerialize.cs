using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
/// <summary>
/// Summary description for XmlSerialize
/// </summary>
public class XmlSerializeLib
{
    public XmlSerializeLib()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public static string XmlSerializeCopy(object ObjectSerialized)
    {
        try
        {
            if (ObjectSerialized == null)
            {
                throw new Exception("Object parameter is null.");
            }
            //XmlSerializer ser = CreateOverrider(ObjectSerialized.GetType().BaseType);
            XmlSerializer ser = new XmlSerializer(ObjectSerialized.GetType());
            MemoryStream mem = new MemoryStream();
            ser.Serialize(mem, ObjectSerialized);
            mem.Flush();
            mem.Close();
            string xml = Encoding.UTF8.GetString(mem.GetBuffer()).Trim('\0');
            mem.Dispose();
            mem = null;
            return xml;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <author>Chu.Pham</author>
    /// <summary>
    /// This function is used to serialize object to string
    /// </summary>
    /// <param name="serializingObject"></param>
    /// <returns></returns>
    public static string XmlSerialize(object serializingObject)
    {
        try
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Indent = true;
            settings.NewLineHandling = NewLineHandling.Replace;
            settings.NewLineChars = "\n";

            StringWriter strWriter = new StringWriter();
            XmlWriter writer = XmlWriter.Create(strWriter, settings);
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            XmlSerializer serializer = new XmlSerializer(serializingObject.GetType());

            serializer.Serialize(writer, serializingObject, namespaces);
            return strWriter.ToString();
        }
        catch (Exception ex)
        {
			throw ex;
        }
    }

    /// <author>Chu.Pham</author>
    /// <summary>
    /// This function is used to deseiralize xml to object
    /// </summary>
    /// <param name="xml">string xml</param>
    /// <param name="type">Type of object to deserialize</param>
    /// <returns>New object</returns>
    public static object Deserialize(string xml, Type type)
    {
        try
        {
            StringReader rd = new StringReader(xml);
            XmlSerializer srz = new XmlSerializer(type);
            return srz.Deserialize(rd);
        }
        catch (Exception ex)
        {
			throw ex;
        }
    }
}
