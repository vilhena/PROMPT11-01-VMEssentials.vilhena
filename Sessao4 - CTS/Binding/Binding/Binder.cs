using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Binding
{
    

    public class Binder<T>
    {
        public T Bind(IEnumerable<KeyValuePair<string, string>> pairs)
        {
            T ret = Activator.CreateInstance<T>();
            Type retType = typeof(T);

            foreach (var keyValuePair in pairs)
            {
                if(retType.GetProperty(keyValuePair.Key) == null && retType.GetField(keyValuePair.Key) == null)
                    throw new InexistentMemberBinderException(keyValuePair.Key);
            }

            foreach (var prop in retType.GetProperties())
            {
                foreach (var item in pairs)
                {
                    if (item.Key == prop.Name)
                    {
                        if (prop.PropertyType.IsPrimitive || prop.PropertyType == typeof(String))
                        {
                            object newType = Convert.ChangeType(item.Value, prop.PropertyType);
                            prop.SetValue(ret, newType, new object[] { });
                        }
                        else
                        {
                            throw new NotPrimitiveMemberBinderException(prop);
                        }

                    }
                }
            }

            foreach (var field in retType.GetFields())
            {
                foreach (var item in pairs)
                {
                    if (item.Key == field.Name)
                    {
                        if (field.FieldType.IsPrimitive || field.FieldType == typeof(String))
                        {
                            object newType = Convert.ChangeType(item.Value, field.FieldType);
                            field.SetValue(ret, newType);
                        }
                        else
                        {
                            throw new NotPrimitiveMemberBinderException(field);
                        }
                    }
                }
            }
    
            return ret;
        }

        public T BindTo(IEnumerable<KeyValuePair<string,string>> pairs)
        {
            T ret = Activator.CreateInstance<T>();
            Type retType = typeof(T);

            foreach (var prop in retType.GetProperties())
            {
                foreach (BindingAttribute attr in prop.GetCustomAttributes(typeof(BindingAttribute),false))
                {
                    foreach (var item in pairs)
                    {
                        if (item.Key == attr.Name)
                        {
                            if (prop.PropertyType.IsPrimitive || prop.PropertyType == typeof(String))
                            {
                                object newType = Convert.ChangeType(item.Value, prop.PropertyType);
                                prop.SetValue(ret, newType, new object[] { });
                            }
                            else
                            {
                                throw new NotPrimitiveMemberBinderException(prop);
                            }

                        }
                    } 
                }
            }

            foreach (var field in retType.GetFields())
            {
                foreach (BindingAttribute attr in field.GetCustomAttributes(typeof(BindingAttribute), false))
                {
                    foreach (var item in pairs)
                    {
                        if (item.Key == attr.Name)
                        {
                            if (field.FieldType.IsPrimitive || field.FieldType == typeof(String))
                            {
                                object newType = Convert.ChangeType(item.Value, field.FieldType);
                                field.SetValue(ret, newType);
                            }
                            else
                            {
                                throw new NotPrimitiveMemberBinderException(field);
                            }
                        }
                    }
                }
            }
            
            return ret;
        }
    }
}
