using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;

namespace NodeTreeCms
{
    public static class ModelHelper    {
        public static dynamic UpdateModelFirst(object model, dynamic requestForm, List<string> allowedFields = null, List<string> disAllowedFields = null, List<string> checkboxes = null, bool convertCamelCaseToPascalCase = true)
        {
            var collectedErrorMessages = "";
            //var keyValue = new NameValueCollection();
            //foreach (string k in requestForm.Keys)
            //{
            //    var key = k;
            //    if (convertCamelCaseToPascalCase) key = k[0].ToString().ToUpper() + k.Remove(0, 1);
            //    keyValue.Add(key, requestForm[k].ToString());
            //}
            //if (checkboxes != null)
            //{
            //    foreach (var s in checkboxes)
            //    {
            //        if (!keyValue.AllKeys.Contains(s))
            //            keyValue.Add(s, "");
            //    }
            //}

            foreach (var pi in model.GetType().GetProperties())
                if ((disAllowedFields == null || !disAllowedFields.Contains(pi.Name)) || (allowedFields == null || allowedFields.Contains(pi.Name)))
                {
                    // ((Nancy.DynamicDictionary)requestForm).GetDynamicMemberNames().Contains(pi.Name)
                    if (requestForm[pi.Name].HasValue)
                    {
                        try
                        {
                            if (pi.PropertyType == typeof(DateTime))
                            {
                                pi.SetValue(model, Convert.ToDateTime(requestForm[pi.Name].Value), null);
                            }
                            else if (pi.PropertyType == typeof(Int32))
                            {
                                pi.SetValue(model, Convert.ToInt32(requestForm[pi.Name].Value), null);
                            }
                            else if (pi.PropertyType == typeof(bool))
                            {
                                pi.SetValue(model, (!String.IsNullOrEmpty(requestForm[pi.Name].Value)), null);
                            }
                            else
                            {
                                pi.SetValue(model, requestForm[pi.Name].Value, null);
                            }

                        }
                        catch (Exception ex)
                        {
                            collectedErrorMessages += pi.Name + ":" + ex.Message + ". ";
                        }
                    }
                }

            if (collectedErrorMessages != "")
            {
                throw new FormatException(collectedErrorMessages);
            }

            return model;
        }
        public static dynamic UpdateModel(object model, dynamic requestForm, List<string> allowedFields = null, List<string> disAllowedFields = null, List<string> checkboxes = null, bool convertCamelCaseToPascalCase = true)
        {
            var collectedErrorMessages = "";
            //NameValueCollection
            var keyValue = new NameValueCollection();
            foreach (string k in requestForm.Keys)
            {
                var key = k;
                if (convertCamelCaseToPascalCase) key = k[0].ToString().ToUpper() + k.Remove(0, 1);
                keyValue.Add(key, requestForm[k].ToString());
            }
            if (checkboxes != null)
            {
                foreach (var s in checkboxes)
                {
                    if (!keyValue.AllKeys.Contains(s))
                        keyValue.Add(s, "");
                }
            }

            foreach (var key in keyValue.AllKeys)
                if ((disAllowedFields == null || !disAllowedFields.Contains(key)) || (allowedFields == null || allowedFields.Contains(key)))
                {
                    var pi = model.GetType().GetProperty(key);
                    if (pi != null)
                    {
                        try
                        {
                            if (pi.PropertyType == typeof(DateTime))
                            {
                                pi.SetValue(model, Convert.ToDateTime(requestForm[key]), null);
                            }
                            else if (pi.PropertyType == typeof(Int32))
                            {
                                pi.SetValue(model, Convert.ToInt32(requestForm[key]), null);
                            }
                            else if (pi.PropertyType == typeof(bool))
                            {
                                pi.SetValue(model, (!String.IsNullOrEmpty(requestForm[key])), null);
                            }
                            else
                            {
                                pi.SetValue(model, requestForm[key], null);
                            }

                        }
                        catch (Exception ex)
                        {
                            collectedErrorMessages += key + ":" + ex.Message + ". ";
                        }
                    }
                }

            if (collectedErrorMessages != "")
            {
                throw new FormatException(collectedErrorMessages);
            }

            return model;
        }
    }
}