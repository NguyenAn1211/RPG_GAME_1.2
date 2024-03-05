using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

[XmlRoot("dictionary")]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
{
    public XmlSchema GetSchema() { return null; }

    public void ReadXml(XmlReader reader)
    {
        reader.Read();

        while (reader.NodeType != XmlNodeType.EndElement)
        {
            reader.ReadStartElement("item");

            reader.ReadStartElement("key");
            TKey key = (TKey)new XmlSerializer(typeof(TKey)).Deserialize(reader);
            reader.ReadEndElement();

            reader.ReadStartElement("value");
            TValue value = (TValue)new XmlSerializer(typeof(TValue)).Deserialize(reader);
            reader.ReadEndElement();

            reader.ReadEndElement();

            this.Add(key, value);
            reader.MoveToContent();
        }

        reader.ReadEndElement();
    }

    public void WriteXml(XmlWriter writer)
    {
        foreach (TKey key in this.Keys)
        {
            writer.WriteStartElement("item");

            writer.WriteStartElement("key");
            new XmlSerializer(typeof(TKey)).Serialize(writer, key);
            writer.WriteEndElement();

            writer.WriteStartElement("value");
            new XmlSerializer(typeof(TValue)).Serialize(writer, this[key]);
            writer.WriteEndElement();

            writer.WriteEndElement();
        }
    }
}
