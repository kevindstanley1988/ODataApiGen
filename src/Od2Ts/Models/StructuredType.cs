﻿using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Od2Ts.Models
{
    public abstract class StructuredType 
    {
        public StructuredType(XElement sourceElement)
        {
            NameSpace = sourceElement.Parent?.Attribute("Namespace")?.Value;
            Name = sourceElement.Attribute("Name")?.Value;
            BaseType = sourceElement.Attribute("BaseType")?.Value;

            KeyNames =
                sourceElement.Descendants()
                    .Where(a => a.Name.LocalName == "Key")
                    .Descendants()
                    .Select(ch => ch.Attribute("Name")?.Value)
                    .ToList();

            Properties = sourceElement.Descendants().Where(a => a.Name.LocalName == "Property")
                .Select(propElement => new Property()
                {
                    IsCollection = propElement.Attribute("Type")?.Value.StartsWith("Collection(") ?? false,
                    Name = propElement.Attribute("Name")?.Value,
                    IsRequired = propElement.Attribute("Nullable")?.Value == "false",
                    Type = propElement.Attribute("Type")?.Value.TrimStart("Collection(".ToCharArray()).TrimEnd(')'),
                }).ToList();

            NavigationProperties = sourceElement.Descendants().Where(a => a.Name.LocalName == "NavigationProperty")
                .Select(prop => new NavigationProperty()
                {
                    Name = prop.Attribute("Name")?.Value.Split(".").Last(),
                    FullName = prop.Attribute("Name")?.Value,
                    IsRequired = false,
                    IsCollection = prop.Attribute("Type")?.Value.StartsWith("Collection(") ?? false,
                    Type = prop.Attribute("Type")?.Value.TrimStart("Collection(".ToCharArray()).TrimEnd(')'),
                    ReferentialConstraint = prop.Descendants().SingleOrDefault(a => a.Name.LocalName == "ReferentialConstraint")?.Attribute("Property")?.Value,
                    ReferencedProperty = prop.Descendants().SingleOrDefault(a => a.Name.LocalName == "ReferentialConstraint")?.Attribute("ReferencedProperty")?.Value
                }).ToList();
        }

        public string NameSpace { get; private set; }
        public string Name { get; private set; }
        public string BaseType { get; private set; }
        public string Type { get { return $"{this.NameSpace}.{this.Name}"; } }
        public List<string> KeyNames { get; set; }
        public string KeyName { get { return this.KeyNames.FirstOrDefault(); } }
        public bool CompositeKey { get { return this.KeyNames.Count() > 1; } }
        public List<Property> Properties { get; private set; }
        public List<NavigationProperty> NavigationProperties { get; set; }
    }
}