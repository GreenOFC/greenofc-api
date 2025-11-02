using _24hplusdotnetcore.Common.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace _24hplusdotnetcore.Extensions
{
    public static class ObjectHelpers
    {
        /// <summary>
        /// Using reflection to apply optional parameters to the request.  
        /// 
        /// If the optonal parameters are null then we will just return the request as is.
        /// </summary>
        /// <param name="request">The request. </param>
        /// <param name="optional">The optional parameters. </param>
        /// <returns></returns>
        public static object ApplyOptionalParms(object request, object optional)
        {
            if (optional == null)
                return request;

            System.Reflection.PropertyInfo[] optionalProperties = (optional.GetType()).GetProperties();

            foreach (System.Reflection.PropertyInfo property in optionalProperties)
            {
                // Copy value from optional parms to the request.  They should have the same names and datatypes.
                System.Reflection.PropertyInfo piShared = (request.GetType()).GetProperty(property.Name);
                if (property.GetValue(optional, null) != null) // TODO Test that we do not add values for items that are null
                    piShared.SetValue(request, property.GetValue(optional, null), null);
            }

            return request;
        }

        public static object GetDeepPropertyValue(this object instance, string path)
        {
            var pp = path.Split('.');
            Type type = instance.GetType();
            foreach (var prop in pp)
            {
                if (int.TryParse(prop, out int n))
                {
                    type = type.GetEnumerableType();
                    if (!(instance is IList list) || list.Count < n + 1)
                    {
                        return null;
                    }
                    instance = list[n];
                }
                else
                {
                    PropertyInfo propInfo = type.GetProperty(prop);

                    if (propInfo == null)
                    {
                        throw new ArgumentException("Properties path is not correct");
                    }

                    if (instance == null)
                    {
                        return null;
                    }

                    instance = propInfo.GetValue(instance, null);
                    type = propInfo.PropertyType;
                }
            }
            return instance;
        }

        public static Type GetEnumerableType(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                return type.GetGenericArguments()[0];

            var iface = (from i in type.GetInterfaces()
                         where i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>)
                         select i).FirstOrDefault();

            if (iface == null)
                throw new ArgumentException("Does not represent an enumerable type.", "type");

            return GetEnumerableType(iface);
        }

        public static bool IsSimpleType(Type type)
        {
            return
                type.IsPrimitive ||
                new Type[] {
                    typeof(string),
                    typeof(decimal),
                    typeof(DateTime),
                    typeof(DateTimeOffset),
                    typeof(TimeSpan),
                    typeof(Guid)
                }.Contains(type) ||
                type.IsEnum ||
                Convert.GetTypeCode(type) != TypeCode.Object ||
                (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) && IsSimpleType(type.GetGenericArguments()[0]));
        }

        private static bool IsNull(this object obj)
        {
            return obj == null || 
                (obj is IEnumerable<object> oldValueEn && !oldValueEn.Any()) || 
                (obj is string oldValueStr && string.IsNullOrEmpty(oldValueStr));
        }

        public static CompareResult Compare(object oldValue, object newValue)
        {
            if (oldValue.IsNull() && newValue.IsNull())
            {
                return null;
            }

            if (oldValue == null || newValue == null)
            {
                return new CompareResult
                {
                    OldValue = oldValue,
                    NewValue = newValue
                };
            }

            Type type = oldValue.GetType();
            if (IsSimpleType(type))
            {
                return oldValue.Equals(newValue) ? null : new CompareResult
                {
                    OldValue = oldValue,
                    NewValue = newValue
                };
            }

            if (type.IsGenericType)
            {

                if (oldValue is IEnumerable<object> oldValueEn && newValue is IEnumerable<object> newValueEn)
                {
                    var itemType = type.GetGenericArguments()[0];
                    var props = itemType.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
                    var keyProp = props.FirstOrDefault(x => x.GetCustomAttributes(true).Any(y => y is KeyAuditingAttribute));

                    var oldValueList = oldValueEn.ToList();
                    var newValueList = newValueEn.ToList();

                    var listOld = new List<object>();
                    var listNew = new List<object>();
                    if (keyProp == null)
                    {
                        listOld = oldValueList.Skip(newValueList.Count).ToList();
                        listNew = newValueList.Skip(oldValueList.Count).ToList();
                        for (int i = 0; i < oldValueList.Count && i < newValueList.Count; i++)
                        {
                            var reslt = Compare(oldValueList[i], newValueList[i]);
                            if (reslt != null)
                            {
                                listOld.Add(reslt.OldValue);
                                listNew.Add(reslt.NewValue);
                            }
                        }
                    }
                    else
                    {
                        listOld = oldValueList.Where(x => !newValueList.Any(y => Equals(keyProp.GetValue(x), keyProp.GetValue(y)))).ToList();
                        listNew = newValueList.Where(x => !oldValueList.Any(y => Equals(keyProp.GetValue(x), keyProp.GetValue(y)))).ToList();
                        foreach (var oldItem in oldValueList)
                        {
                            var newItem = newValueList.FirstOrDefault(x => Equals(keyProp.GetValue(x), keyProp.GetValue(oldItem)));
                            if (newItem != null)
                            {
                                var result = Compare(oldItem, newItem);
                                if (result != null)
                                {
                                    listOld.Add(result.OldValue);
                                    listNew.Add(result.NewValue);
                                }
                            }
                        }
                    }

                    if (listOld.Any() || listNew.Any())
                    {
                        return new CompareResult
                        {
                            OldValue = listOld,
                            NewValue = listNew
                        };
                    }
                }

                return null;
            }
            else
            {
                var dictOld = new Dictionary<string, object>();
                var dictNew = new Dictionary<string, object>();
                bool hasChange = false;
                var props = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
                    .Where(x => !x.GetCustomAttributes(true).Any(y => y is DisableAuditingAttribute));
                foreach (PropertyInfo pi in props)
                {
                    var val = pi.GetValue(oldValue);
                    var tval = pi.GetValue(newValue);

                    var result = Compare(val, tval);
                    if (result != null)
                    {
                        hasChange = true;
                        dictOld.Add(pi.Name, result.OldValue);
                        dictNew.Add(pi.Name, result.NewValue);
                    }

                }

                if (hasChange)
                {
                    var keyProp = props.FirstOrDefault(x => x.GetCustomAttributes(true).Any(y => y is KeyAuditingAttribute));
                    if (keyProp != null)
                    {
                        dictOld.Add(keyProp.Name, keyProp.GetValue(oldValue));
                        dictNew.Add(keyProp.Name, keyProp.GetValue(newValue));
                    }
                }

                return hasChange ? new CompareResult
                {
                    OldValue = dictOld,
                    NewValue = dictNew
                } : null;
            }
        }
    }

    public class CompareResult
    {
        public object OldValue { get; set; }
        public object NewValue { get; set; }
    }
}
