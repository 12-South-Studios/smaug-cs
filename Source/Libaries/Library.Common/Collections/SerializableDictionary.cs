using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace Library.Common.Collections;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TVal"></typeparam>
[Serializable]
public sealed class SerializableDictionary<TKey, TVal> : Dictionary<TKey, TVal>, IXmlSerializable, ISerializable
{
  private const string ITEM_NODE_NAME = "Item";
  private const string KEY_NODE_NAME = "Key";
  private const string VALUE_NODE_NAME = "Value";

  /// <summary>
  /// 
  /// </summary>
  public SerializableDictionary()
  {
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="dictionary"></param>
  public SerializableDictionary(IDictionary<TKey, TVal> dictionary)
    : base(dictionary)
  {
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="comparer"></param>
  public SerializableDictionary(IEqualityComparer<TKey> comparer)
    : base(comparer)
  {
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="capacity"></param>
  public SerializableDictionary(int capacity)
    : base(capacity)
  {
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="dictionary"></param>
  /// <param name="comparer"></param>
  public SerializableDictionary(IDictionary<TKey, TVal> dictionary, IEqualityComparer<TKey> comparer)
    : base(dictionary, comparer)
  {
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="capacity"></param>
  /// <param name="comparer"></param>
  public SerializableDictionary(int capacity, IEqualityComparer<TKey> comparer)
    : base(capacity, comparer)
  {
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="info"></param>
  /// <param name="context"></param>
  private SerializableDictionary(SerializationInfo info, StreamingContext context) : base(info, context)
  {
    int itemCount = info.GetInt32("ItemCount");
    for (int i = 0; i < itemCount; i++)
    {
      KeyValuePair<TKey, TVal> kvp =
        (KeyValuePair<TKey, TVal>)info.GetValue($"Item{i}", typeof(KeyValuePair<TKey, TVal>));
      Add(kvp.Key, kvp.Value);
    }
  }

  void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
  {
    info.AddValue("ItemCount", Count);
    int itemIdx = 0;
    foreach (KeyValuePair<TKey, TVal> kvp in this)
    {
      info.AddValue($"Item{itemIdx}", kvp, typeof(KeyValuePair<TKey, TVal>));
      itemIdx++;
    }
  }

  void IXmlSerializable.WriteXml(XmlWriter writer)
  {
    foreach (KeyValuePair<TKey, TVal> kvp in this)
    {
      writer.WriteStartElement(ITEM_NODE_NAME);
      writer.WriteStartElement(KEY_NODE_NAME);
      KeySerializer.Serialize(writer, kvp.Key);
      writer.WriteEndElement();
      writer.WriteStartElement(VALUE_NODE_NAME);
      ValueSerializer.Serialize(writer, kvp.Value);
      writer.WriteEndElement();
      writer.WriteEndElement();
    }
  }

  void IXmlSerializable.ReadXml(XmlReader reader)
  {
    if (reader.IsEmptyElement)
      return;

    // Move past container
    if (!reader.Read())
      throw new XmlException("Error in Deserialization of Dictionary");

    while (reader.NodeType != XmlNodeType.EndElement)
    {
      reader.ReadStartElement(ITEM_NODE_NAME);
      reader.ReadStartElement(KEY_NODE_NAME);
      TKey key = (TKey)KeySerializer.Deserialize(reader);
      reader.ReadEndElement();
      reader.ReadStartElement(VALUE_NODE_NAME);
      TVal value = (TVal)ValueSerializer.Deserialize(reader);
      reader.ReadEndElement();
      reader.ReadEndElement();
      Add(key, value);
      reader.MoveToContent();
    }

    reader.ReadEndElement(); // Read End Element to close Read of containing node
  }

  System.Xml.Schema.XmlSchema IXmlSerializable.GetSchema()
  {
    return null;
  }

  /// <summary>
  /// 
  /// </summary>
  private XmlSerializer ValueSerializer => _valueSerializer ??= new XmlSerializer(typeof(TVal));

  private XmlSerializer KeySerializer => _keySerializer ??= new XmlSerializer(typeof(TKey));

  private XmlSerializer _keySerializer;
  private XmlSerializer _valueSerializer;
}