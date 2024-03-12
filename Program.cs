using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace XmlSerializableStructTest
{
	internal class Program
	{
		static void Main(string[] args)
		{

			var a = FromXmlStream<NakedWrapper>(ToXmlStream(new NakedWrapper()));
			Console.WriteLine(a.Stuff.Data);

			var b = FromXmlStream<SerializableNakedWrapper>(ToXmlStream(new SerializableNakedWrapper()));
			Console.WriteLine(b.Stuff.Data);

			var c = FromXmlStream<InitWrapper>(ToXmlStream(new InitWrapper()));
			Console.WriteLine(c.Stuff.Data);

			var d = FromXmlStream<SerializableInitWrapper>(ToXmlStream(new SerializableInitWrapper()));
			Console.WriteLine(d.Stuff.Data);

		}

		#region Serialiser
		static MemoryStream ToXmlStream(object obj)
		{
			MemoryStream ms = new MemoryStream();
			XmlSerializer xs = new XmlSerializer(obj.GetType());
			xs.Serialize(ms, obj);
			ms.Position = 0;
			StreamReader sr = new StreamReader(ms);
			Console.WriteLine(sr.ReadToEnd());
			ms.Position = 0;
			return ms;
		}

		static T FromXmlStream<T>(Stream stream)
			where T : class
		{
			XmlSerializer xs = new XmlSerializer(typeof(T));
			return xs.Deserialize(stream) as T;
		}
		#endregion
	}

	#region Classes to wrap the structs up

	public class NakedWrapper
	{
		public NakedStruct Stuff { get; set; } = new NakedStruct() { Data = "Foobar" };
	}

	public class SerializableNakedWrapper
	{
		public SerializableNakedStruct Stuff { get; set; } = new SerializableNakedStruct() { Data = "Foobar" };
	}

	public class InitWrapper
	{
		public InitStruct Stuff { get; set; } = new InitStruct() { Data = "Foobar" };
	}

	public class SerializableInitWrapper
	{
		public SerializableInitStruct Stuff { get; set; } = new SerializableInitStruct() { Data = "Foobar" };
	}
	#endregion

	/// <summary>
	/// WORKS: No explicit constructor, no initialisers, no IXmlSerializable
	/// </summary>
	public struct NakedStruct
	{
		public string Data { get; set; }
	}

	/// <summary>
	/// WORKS: No explicit constructor, no initialisers, but with IXmlSerializable implemented
	/// </summary>
	public struct SerializableNakedStruct : IXmlSerializable
	{
		public string Data { get; set; }

		public XmlSchema? GetSchema()
		{
			return null;
		}

		public void ReadXml(XmlReader reader)
		{
			this.Data = reader.GetAttribute("Data");
		}

		public void WriteXml(XmlWriter writer)
		{
			writer.WriteAttributeString("Data", this.Data);
		}
	}

	/// <summary>
	/// WORKS: Parameterless constructor defined with a default value, no IXmlSerializable
	/// </summary>
	public struct InitStruct
	{
		public InitStruct()
		{
			this.Data = "Nope";
		}
		public string Data { get; set; }
	}

	/// <summary>
	/// FAILS: Parameterless constructor defined with a default value, IXmlSerializable implemented
	/// </summary>
	public struct SerializableInitStruct : IXmlSerializable
	{
		public SerializableInitStruct() 
		{ 
			this.Data = "Nope";
		}
		public string Data { get; set; }

		public XmlSchema? GetSchema()
		{
			return null;
		}

		public void ReadXml(XmlReader reader)
		{
			this.Data = reader.GetAttribute("Data");
		}

		public void WriteXml(XmlWriter writer)
		{
			writer.WriteAttributeString("Data", this.Data);
		}
	}
}
