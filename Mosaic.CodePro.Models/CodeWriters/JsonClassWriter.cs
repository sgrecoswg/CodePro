using Mosaic.Core.Extensions;
using Mosaic.Core.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaic.CodePro.Models
{
    public class JsonClassWriter : Notifier
    {
       
        public List<string> WriteClass(string text, string className = "YourClass", List<string> lists = null)
        {
            if (lists == null)
            {
                lists = new List<string>();
            }

            try
            {

                var sub = text.Substring(0, 3);
                
                if (sub.ToLower() == "var")
                {
                    var txtsplit = text.TrimStart(new char[] { 'v', 'a', 'r', ' ' }).Split('=');
                    className = txtsplit[0];
                    text = txtsplit[1];
                }

                var json = JObject.Parse(text);
                string objType = "string";
                StringBuilder sb = new StringBuilder();
                Notify($"Writing class {className.ToPascalCasing()}");
                sb.AppendLine($"///<summary>");
                sb.AppendLine($"///");
                sb.AppendLine($"///</summary>");
                sb.AppendLine($"public class {className.ToPascalCasing()}");
                sb.AppendLine("{");
                foreach (KeyValuePair<string, JToken> token in json)
                {
                    if (token.Value.HasValues)//should it be it's own class?
                    {
                        objType = token.Key;//yes
                    }
                    else
                    {
                        //no, its a primitive type... which one?
                        if (token.Value.ToString().Contains("function(")) objType = "function";
                        else if (string.IsNullOrEmpty(token.Value.ToString())) objType = "object?";
                        else if (int.TryParse(token.Value.ToString(), out int i)) objType = "int";
                        else if (decimal.TryParse(token.Value.ToString(), out decimal d)) objType = "decimal";
                        else if (DateTime.TryParse(token.Value.ToString(), out DateTime date)) objType = "DateTime";
                        else if (bool.TryParse(token.Value.ToString(), out bool b)) objType = "bool";
                        
                    }

                    if (objType != "function")
                    {
                        sb.AppendLine($"\t///<summary>");
                        sb.AppendLine($"\t///");
                        sb.AppendLine($"\t///</summary>");                        

                        if (objType == token.Key)
                        {
                            sb.AppendLine($"\tpublic {objType.ToPascalCasing()} {token.Key.ToPascalCasing()}" + " { get; set; }" + Environment.NewLine);
                            WriteClass(token.Value.ToString(), token.Key, lists).Last();
                        }
                        else
                        {
                            sb.AppendLine($"\tpublic {objType} {token.Key.ToPascalCasing()}" + " { get; set; }" + Environment.NewLine);
                        }
                    }
                    else
                    {
                        sb.AppendLine($"\t///<summary>");
                        sb.AppendLine($"\t///");
                        sb.AppendLine($"\t///</summary>");
                        sb.AppendLine($"\tpublic void {token.Key.ToPascalCasing()}()" + " { throw new NotImplemetedException(); }" + Environment.NewLine);
                    }
                   
                }
                sb.AppendLine("}" + Environment.NewLine);

                lists.Add(sb.ToString());
                return lists;
            }
            catch (Exception e)
            {
                PassError(e);
                return lists;
            }

        }
    }
}
