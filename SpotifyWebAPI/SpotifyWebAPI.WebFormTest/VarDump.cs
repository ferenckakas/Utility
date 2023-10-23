using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace SpotifyWebAPI.Test
{
    public class VarDump
    {
        public static string Dump(object obj, int recursion)
        {
            if (obj == null)
                return string.Empty;

            StringBuilder result = new StringBuilder();

            // Protect the method against endless recursion
            if (recursion < 5)
            {
                // Determine object type
                Type t = obj.GetType();
                if (t == typeof(DateTime))
                    return result.ToString();
                if (t == typeof(string))
                    return result.ToString();

                // Get array with properties for this object
                PropertyInfo[] properties = t.GetProperties();

                foreach (PropertyInfo property in properties)
                {
                    try
                    {
                        // Get the property value
                        object value = property.GetValue(obj, null);

                        // Create indenting string to put in front of properties of a deeper level 
                        // We'll need this when we display the property name and value
                        string indent = String.Empty;
                        string spaces = "|   ";
                        string trail = "|...";
                        
                        if (recursion > 0)
                        {
                            indent = new StringBuilder(trail).Insert(0, spaces, recursion - 1).ToString();
                        }

                        if (value != null)
                        {
                            // If the value is a string, add quotation marks
                            string displayValue = value.ToString();
                            if (value is string) displayValue = String.Concat('"', displayValue, '"');

                            // Add property name and value to return string
                            result.AppendFormat("{0}<b>{1}</b> = {2}<br />", indent, property.Name, displayValue);

                            try
                            {
                                if (!(value is ICollection))
                                {

                                    // Call var_dump() again to list child properties
                                    // This throws an exception if the current property value 
                                    // is of an unsupported type (eg. it has not properties)
                                    result.Append(Dump(value, recursion + 1));
                                }
                                else
                                {
                                    // 2009-07-29: added support for collections
                                    // The value is a collection (eg. it's an arraylist or generic list)
                                    // so loop through its elements and dump their properties
                                    int elementCount = 0;

                                    if (value.GetType() == typeof(ICollection) || value.GetType().GetGenericTypeDefinition() == typeof(List<>))
                                    {
                                        foreach (object element in ((ICollection)value))
                                        {
                                            string elementName = String.Format("{0}[{1}]", property.Name, elementCount);
                                            indent = new StringBuilder(trail).Insert(0, spaces, recursion).ToString();

                                            // Display the collection element name and type
                                            result.AppendFormat("{0}<b>{1}</b> = {2}<br />", indent, elementName, element.ToString());

                                            // Display the child properties
                                            result.Append(Dump(element, recursion + 2));
                                            elementCount++;
                                        }
                                    }

                                    result.Append(Dump(value, recursion + 1));
                                }
                            }
                            catch { }
                        }
                        else
                        {
                            // Add empty (null) property to return string
                            result.AppendFormat("{0}<b>{1}</b> = {2}<br />", indent, property.Name, "null");
                        }
                    }
                    catch
                    {
                        // Some properties will throw an exception on property.GetValue() 
                        // I don't know exactly why this happens, so for now i will ignore them...
                    }
                }
            }

            return result.ToString();
        }
    }
}