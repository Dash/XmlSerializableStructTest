This project demos a problem with the .NET XmlSerialiser and structs.

When a struct implements IXmlSerializable AND has a parameterless constructor, the XmlSerializer class will blow up on Deserialize with an InvalidProgramException.