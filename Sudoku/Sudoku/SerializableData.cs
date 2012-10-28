using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;

namespace Sudoku
{
    public class SerializableData : IXmlSerializable
    {
        public String txt = "";
        public SerializableData()
        {
            
        }

        private const string SerializationNamespace = @"http://schemas.microsoft.com/2003/10/Serialization/Arrays";

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            txt = reader.ReadElementContentAsString();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("root", txt);
        }
    }
}
