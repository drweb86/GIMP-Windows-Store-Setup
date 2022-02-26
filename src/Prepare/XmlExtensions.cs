using System.Xml;

namespace DownloadInstaller
{
    public static class XmlExtensions
    {
        public static XmlNode Go(this XmlNode root, string childName)
        {
            foreach (XmlNode node in root)
            {
                if (node.Name == childName)
                {
                    return node;
                }
            }
            throw new Exception($"Cannot find node {childName}");
        }

        public static XmlNode SetAttr(this XmlNode root, string attribute, string value)
        {
            root.Attributes[attribute].Value = value;
            return root;
        }
    }
}
