﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace AnZwDev.ALTools.ALSymbolReferences.Serialization
{

    [XmlRoot("Package", Namespace = "http://schemas.microsoft.com/navx/2015/manifest")]
    public class NavxPackage
    {

        [XmlElement("App")]
        public NavxApp App { get; set; }

        [XmlArray("Dependencies")]
        [XmlArrayItem("Dependency")]
        public NavxDependency[] Dependencies { get; set; }

    }

}
