using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Pit11_26_toPit11_27_Converter
{
    public static class Converter
    {
        private static string _pitVersion = "27";
        private static string _pitCode = "PIT-11 (27)";

        private static (int begin, int end) _range = new(64, 88);
        private static (int begin, int end) _secondRange = new(89, 91);
        private static int _detailsOffset = 6;
        private static int _secondDetailsOffset = 7;

        private static XNamespace _newNamespace = "http://crd.gov.pl/wzor/2021/03/04/10477/";
        private static XNamespace _etd = "http://crd.gov.pl/xml/schematy/dziedzinowe/mf/2020/07/06/eD/DefinicjeTypy/";

        private static XElement GetNode(this XDocument document, string name)
        {
            return document.Descendants().SingleOrDefault(p => p.Name.LocalName == name);
        }
        public static void ReplaceDefaultNamespace(this XDocument source, XNamespace xNamespace)
        {
            source.Root.SetAttributeValue("xmlns", _newNamespace);
            foreach (XElement xElement in source.Descendants())
            {
                if(xElement.Name.Namespace != _etd)
                    xElement.Name = xNamespace + xElement.Name.LocalName;
            }
        }


        public static void ConvertDocument(XDocument document)
        {
            var node = document.GetNode(NodeNames.FormVariant);
            if (node != null) 
                node.Value = _pitVersion;

            node = document.GetNode(NodeNames.FormCode);
            node.SetAttributeValue(AttributeNames.SystemCode, _pitCode);

            for (int i = _range.end; i >= _range.begin; --i)
            {
                node = document.GetNode(NodeNames.DetailsPrefix + i);
                if (node != null)
                    node.Name = NodeNames.DetailsPrefix + (i + _detailsOffset);
            }

            for (int i = _secondRange.end; i >= _secondRange.begin; --i)
            {
                node = document.GetNode(NodeNames.DetailsPrefix + i);
                if (node != null)
                    node.Name = NodeNames.DetailsPrefix + (i + _secondDetailsOffset);
            }

            document.ReplaceDefaultNamespace(_newNamespace);
        }
    }
}
