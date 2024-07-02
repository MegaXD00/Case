using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Case.Models
{
    /// <summary>
    ///     Class that represents the XML structure.
    ///     XML file: "userlist.xml" 
    /// </summary>
    [XmlRoot("userlist")]
    public class UserList
    {
        [XmlElement("user")]
        public List<User> Users { get; set; }
    }

    /// <summary>
    ///     Class that describes all the parameters of a user.
    /// </summary>
    public class User
    {
        [XmlElement("id")]
        public int Id { get; set; }

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("cpf")]
        public string CPF { get; set; }

        [XmlElement("role")]
        public string Role { get; set; }

        [XmlElement("email")]
        public string Email { get; set; }
    }
}
