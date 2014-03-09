using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using LuaInterface;
using Realm.Library.Common;
using Realm.Library.Lua.Properties;

namespace Realm.Library.Lua
{
    /// <summary>
    /// Helper static class for common Lua functions
    /// </summary>
    public static class LuaHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="functionRepository"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static LuaFunctionRepository RegisterFunctionTypes(LuaFunctionRepository functionRepository, Type type)
        {
            LuaFunctionRepository repository = functionRepository ?? new LuaFunctionRepository();

            List<MethodInfo> methods = type.GetMethods().ToList();
            foreach (MethodInfo info in methods)
            {
                foreach (Attribute attr in Attribute.GetCustomAttributes(info).ToList())
                {
                    // and if they happen to be one of our LuaFunctionAttribute attributes
                    if (attr.GetType() != typeof(LuaFunctionAttribute))
                        continue;

                    var functionAttr = attr as LuaFunctionAttribute;
                    var paramTable = new Dictionary<string, string>();

                    // Get the desired function name and doc string, along with parameter info
                    if (functionAttr == null)
                        continue;

                    var strFName = functionAttr.Name;
                    var strFDoc = functionAttr.Description;
                    var paramDocs = functionAttr.Parameters;

                    // Now get the expected parameters from the MethodInfo object
                    var paramInfo = info.GetParameters();

                    // If they don't match, someone forgot to add some documentation to the
                    // attribute, complain and go to the next method
                    if (paramDocs.IsNotNull() && (paramInfo.Length != paramDocs.Count))
                        throw new ArgumentException(String.Format(Resources.ERR_FUNC_ARG_MISMATCH, info.Name, strFName,
                                                                  paramDocs.Count, paramInfo.Length));

                    // Build a parameter <-> parameter doc dictionary
                    if (paramDocs.IsNotNull())
                        Enumerable.Range(0, paramInfo.Length)
                                  .ToList()
                                  .ForEach(i => paramTable.Add(paramInfo[i].Name, paramDocs.ElementAt(i)));

                    repository.Add(strFName, new LuaFunctionDescriptor(strFName, strFDoc, paramTable, info));
                }
            }

            return repository;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [LuaFunction("ExploreObject", "Lists the Members and Properties of an Object", "Object to Explore")]
        public static string LuaExploreObject(Object obj)
        {
            var result = new StringBuilder();
            var proxy = obj.CastAs<ProxyType>();

            var type = proxy.IsNotNull() ? proxy.UnderlyingSystemType : obj.GetType();

            result.AppendLine("Type: " + type);

            result.AppendLine("Properties:");
            type.GetProperties().ToList().ForEach(propertyInfo => result.AppendLine("   " + propertyInfo.Name));

            result.AppendLine("Methods:");
            type.GetMethods().ToList().ForEach(methodInfo => result.AppendLine("   " + methodInfo.Name));

            return result.ToString();
        }

        // TODO: LuaLog(ILog, LogLevel, Message, Table of Params)

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [LuaFunction("GetProperty", "Gets the indicated property on the object", "The Object to get the Property from", "The name of the property")]
        public static object LuaGetProperty(IPropertyContext context, string name)
        {
            return context.GetProperty<object>(name);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        [LuaFunction("SetProperty", "Sets the indicated property on the object", "The object to set the property on", "The name of the property", "The value of the property")]
        public static bool LuaSetProperty(IPropertyContext context, string name, object value, PropertyTypeOptions flags)
        {
            context.SetProperty(name, value, flags);
            return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <param name="toUpper"></param>
        /// <returns></returns>
        [LuaFunction("EnumToString", "Returns the enum value as a string", "Enum to return", "Flag to upper-case the enum string")]
        public static string LuaEnumToString(Enum value, bool toUpper)
        {
            return toUpper ? value.GetName().ToUpper() : value.GetName();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="str"></param>
        /// <param name="padChar"></param>
        /// <param name="totalLength"></param>
        /// <param name="toFront"></param>
        /// <returns></returns>
        [LuaFunction("PadString", "Pads a string with a given number of the chosen characters.", "String to pad", "Character to pad with", "Number of characters to pad to", "pad to front")]
        public static string LuaPadString(string str, string padChar, int totalLength, bool toFront)
        {
            return toFront ? str.PadStringToFront(padChar, totalLength) : str.PadString(padChar, totalLength);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="str"></param>
        /// <param name="wordNumber"></param>
        /// <param name="delim"></param>
        /// <returns></returns>
        [LuaFunction("ParseWord", "Parses the word number from the string using the delimiter", "String to parse", "Word number ot seek", "Delimiter type")]
        public static string LuaParseWord(string str, int wordNumber, DelimiterType delim)
        {
            return str.ParseWord(wordNumber, delim.ValueOf());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="str"></param>
        /// <param name="wordNumber"></param>
        /// <returns></returns>
        [LuaFunction("RemoveWord", "Removes the indicated word number from the given string", "String to parse", "Word number to remove")]
        public static string LuaRemoveWord(string str, int wordNumber)
        {
            return str.RemoveWord(wordNumber);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        [LuaFunction("ToLower", "Lower cases a string", "String")]
        public static string LuaToLower(string str)
        {
            return str.ToLower();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        [LuaFunction("ToUpper", "Upper cases a string", "String")]
        public static string LuaToUpper(string str)
        {
            return str.ToUpper();
        }
    }
}