using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Case.Models
{
    /// <summary>
    ///     Class that represents the XML structure.
    ///     XML file: "productlist.xml" 
    /// </summary>
    [XmlRoot("productlist")]
    public class ProductList
    {
        [XmlElement("product")]
        public List<Product> Products { get; set; }
    }

    /// <summary>
    ///     Class that describes all the parameters of a product.
    /// </summary>
    public class Product
    {
        [XmlElement("id")]
        public int Id { get; set; }

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("type")]
        public string Type { get; set; }

        [XmlElement("minquantity")]
        public long MinQuantity { get; set; }

        [XmlElement("maxquantity")]
        public long MaxQuantity { get; set; }

        [XmlElement("unitprice")]
        public long UnitPrice { get; set; }

        [XmlElement("grace")]
        public string GracePeriod { get; set; }

        [XmlElement("maturity")]
        public string MaturityDate { get; set; }

        [XmlElement("ratetype")]
        public string RateType { get; set; }

        [XmlElement("rate")]
        public string Rate { get; set; }

        [XmlElement("interest")]
        public string Interest { get; set; }

        [XmlElement("amortization")]
        public string Amortization { get; set; }

        [XmlElement("agency")]
        public string Agency { get; set; }

        [XmlElement("rating")]
        public string Rating { get; set; }

        [XmlElement("liquidity")]
        public string Liquidity { get; set; }
    }
}
