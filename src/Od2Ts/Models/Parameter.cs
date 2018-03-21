﻿using System.Linq;

namespace Od2Ts.Models
{
    public class Parameter
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string TypescriptType
        {
            get
            {
                switch (Type)
                {
                    case "Edm.String":
                    case "Edm.Duration":
                    case "Edm.Guid":
                    case "Edm.Binary":
                        return "string";
                    case "Edm.Int16":
                    case "Edm.Int32":
                    case "Edm.Int64":
                    case "Edm.Double":
                    case "Edm.Decimal":
                    case "Edm.Single":
                        return "number";
                    case "Edm.Boolean":
                        return "boolean";
                    case "Edm.DateTimeOffset":
                        return "Date";
                    default:
                    {
                        return Type.Contains(".") ? Type.Split('.').Last(a => !string.IsNullOrWhiteSpace(a)) : "any";
                    }
                }
            }
        }
        public bool IsRequired { get; set; }
        
        public string AsDefinition() {
            return $"{this.Name}: {this.TypescriptType}" + (!this.IsRequired ? $" = null" : "");
        }
    }
}
