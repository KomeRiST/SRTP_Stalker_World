using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml;

namespace SRTP_Stalker_World.GameTextObjects
{
    public class GameArticle : MainClass
    {
        public GameArticle(XmlNode xNode, string FileName) : base(xNode, FileName)
        {
            // дальнейшая обработка XmlNode
        }

        public override void Delete()
        {
            throw new System.NotImplementedException();
        }

        public override void Reload()
        {
            throw new System.NotImplementedException();
        }

        public override void Save()
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}