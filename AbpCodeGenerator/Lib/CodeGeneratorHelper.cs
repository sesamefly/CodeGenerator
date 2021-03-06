﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AbpCodeGenerator.Lib
{
    public class CodeGeneratorHelper
    {
        /// <summary>
        /// 根目录 读取模板的路径
        /// </summary>
        private const string rootDirectory = @"D:\Test\AbpCodeGenerator\AbpCodeGenerator\FileTemplates";

        /// <summary>
        /// 程序集名称
        /// </summary>
        private const string Namespace_Here = "ZhouDaFu.XinYunFen";

        /// <summary>
        /// Application文件目录
        /// </summary>
        private const string Application_Directory = @"D:\Project\保费零息贷\ZhouDaFu.XinYunFen\src\ZhouDaFu.XinYunFen.Application\";
        #region client

        #endregion


        #region Server
        /// <summary>
        /// 生成接口信息
        /// </summary>
        /// <param name="className"></param>
        /// <param name="Primary_Key_Inside_Tag_Here">主键类型</param>
        public static void SetAppServiceIntercafeClass(string className, string Primary_Key_Inside_Tag_Here)
        {
            string appServiceIntercafeClassDirectory = rootDirectory + @"\Server\AppServiceIntercafeClass\MainTemplate.txt";
            var templateContent = Read(appServiceIntercafeClassDirectory);

            templateContent = templateContent.Replace("{{Namespace_Here}}", Namespace_Here)
                                             .Replace("{{Namespace_Relative_Full_Here}}", className)
                                             .Replace("{{Entity_Name_Plural_Here}}", className)
                                             .Replace("{{Entity_Name_Here}}", className)
                                             .Replace("{{Primary_Key_Inside_Tag_Here}}", Primary_Key_Inside_Tag_Here)
                                             ;
            Write(Application_Directory + className + "\\", "I" + className + "AppService.cs", templateContent);
        }

        /// <summary>
        /// 生成接口实现类信息
        /// </summary>
        /// <param name="className"></param>
        /// <param name="Primary_Key_Inside_Tag_Here">主键类型</param>
        public static void SetAppServiceClass(string className, string Primary_Key_Inside_Tag_Here)
        {
            string appServiceIntercafeClassDirectory = rootDirectory + @"\Server\AppServiceClass\MainTemplate.txt";
            var templateContent = Read(appServiceIntercafeClassDirectory);
            var Primary_Key_With_Comma_Here = Primary_Key_Inside_Tag_Here;
            if (Primary_Key_Inside_Tag_Here != "int")
            {
                Primary_Key_With_Comma_Here = "," + Primary_Key_Inside_Tag_Here;
            }
            templateContent = templateContent.Replace("{{Namespace_Here}}", Namespace_Here)
                                             .Replace("{{Namespace_Relative_Full_Here}}", className)
                                             .Replace("{{Entity_Name_Plural_Here}}", className)
                                             .Replace("{{Entity_Name_Here}}", className)
                                             .Replace("{{Primary_Key_Inside_Tag_Here}}", Primary_Key_Inside_Tag_Here)
                                             .Replace("{{entity_Name_Here}}", GetFirstToLowerStr(className))
                                             .Replace("{{Permission_Name_Here}}", $"Pages_Administration_{className}s")
                                             .Replace("{{Project_Name_Here}}", $"XinYunFen")//这里需要改成自己项目的父类
                                             .Replace("{{Primary_Key_With_Comma_Here}}", Primary_Key_With_Comma_Here)
                                             ;
            Write(Application_Directory + className + "\\", className + "AppService.cs", templateContent);
        }
        #endregion



        #region Dtos

        /// <summary>
        /// 生成接口实现类信息
        /// </summary>
        /// <param name="className"></param>
        /// <param name="Primary_Key_Inside_Tag_Here">主键类型</param>
        public static void SetCreateOrEditInputClass(string className, List<MetaTableInfo> metaTableInfoList)
        {
            string appServiceIntercafeClassDirectory = rootDirectory + @"\Server\Dtos\CreateOrEditInputClass\MainTemplate.txt";
            var templateContent = Read(appServiceIntercafeClassDirectory);
            StringBuilder sb = new StringBuilder();

            foreach (var item in metaTableInfoList)
            {
                sb.AppendLine("/// <summary>");
                sb.AppendLine("/// " + item.Annotation);
                sb.AppendLine("/// </summary>");
                sb.AppendLine("public " + item.PropertyType + " " + item.Name + " { get; set; }");
            }
            var property_Looped_Template_Here = sb.ToString();
            templateContent = templateContent.Replace("{{Namespace_Here}}", Namespace_Here)
                                             .Replace("{{Namespace_Relative_Full_Here}}", className)
                                             .Replace("{{Entity_Name_Here}}", className)
                                             .Replace("{{Property_Looped_Template_Here}}", property_Looped_Template_Here)
                                             ;
            Write(Application_Directory + className + "\\Dtos\\", "CreateOrEdit" + className + "Dto.cs", templateContent);
        }

        #endregion

        #region 文件读取
        public static string Read(string path)
        {
            using (StreamReader sr = new StreamReader(path, Encoding.Default))
            {
                StringBuilder sb = new StringBuilder();

                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    sb.AppendLine(line.ToString());
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath">文件保存路径</param>
        /// <param name="fileName">文件名</param>
        /// <param name="templateContent">模板内容</param>
        public static void Write(string filePath, string fileName, string templateContent)
        {
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            using (FileStream fs = new FileStream(filePath + fileName, FileMode.Create))
            {
                //获得字节数组
                byte[] data = Encoding.Default.GetBytes(templateContent);
                //开始写入
                fs.Write(data, 0, data.Length);
            }

        }
        #endregion

        #region 首字母小写
        /// <summary>
        /// 首字母小写
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string GetFirstToLowerStr(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                if (str.Length > 1)
                {
                    return char.ToLower(str[0]) + str.Substring(1);
                }
                return char.ToLower(str[0]).ToString();
            }
            return null;
        }
        #endregion
    }
}
