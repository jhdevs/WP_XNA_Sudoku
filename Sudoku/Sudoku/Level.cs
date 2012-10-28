using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sudoku
{
    public class Level : IXmlSerializable
    {
        Field[] fields;
        Rectangle boardBounds;
        float boardIter;
        SpriteFont fieldfont;
        SpriteFont fieldfontB;
        public String txt = "txt";

        public Level(Rectangle boardBounds, float boardIter,SpriteFont fieldfont, SpriteFont fieldfontB)
        {
            this.boardBounds = boardBounds;
            this.boardIter = boardIter;
            this.fieldfont = fieldfont;
            this.fieldfontB = fieldfontB;
            fields = new Field[81];
            for (int i = 0; i < 81; i++)
            {
                fields[i] = new Field(i, boardBounds, boardIter);
            }
        }

        #region IXmlSerializable part

        private const string SerializationNamespace = @"http://schemas.microsoft.com/2003/10/Serialization/Arrays";

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            /*
            fields = new Field[81];
            for (int i = 0; i < 81; i++)
            {
                fields[i] = new Field(i, boardBounds, boardIter);
            }
            reader.Read();
            bool pre = false;
            int num = 0;
            int position = int.Parse(reader.GetAttribute("position"));
            if (position>=0 && position<81)
            {
                pre = bool.Parse(reader.GetAttribute("preset"));
                num = reader.ReadElementContentAsInt();
                fields[position].SetNum(num,pre?fieldfontB:fieldfont,pre);
            }
            */
            txt = reader.ReadElementContentAsString();
        }

        public void WriteXml(XmlWriter writer)
        {
            //
        }
        #endregion
        
    }
}
