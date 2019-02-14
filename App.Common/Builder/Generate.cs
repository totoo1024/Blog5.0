using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using SqlSugar;
using App.Common.Utils;

namespace App.Common.Builder
{
    /// <summary>
    /// 代码生成
    /// </summary>
    public class CodeGenerate
    {
        #region 常量
        /// <summary>
        /// 作者
        /// </summary>
        const string Author = "涂文杰";
        /// <summary>
        /// 版本
        /// </summary>
        const string Verison = "1.0";
        /// <summary>
        /// 实体名称空间
        /// </summary>
        const string EntityNameSpace = "App.Entities";
        /// <summary>
        /// IRepository名称空间
        /// </summary>
        const string IRepositoryNameSpace = "App.IRepository";
        /// <summary>
        /// Repository名称空间
        /// </summary>
        const string RepositoryNameSpace = "App.Repository";
        /// <summary>
        /// IService名称空间
        /// </summary>
        const string IServiceNameSpace = "App.IServices";
        /// <summary>
        /// Service名称空间
        /// </summary>
        const string ServiceNameSpace = "App.Services";
        #endregion

        #region 字段

        /// <summary>
        /// 根目录
        /// </summary>
        static string RootPath = Directory.GetCurrentDirectory();

        /// <summary>
        /// 实体所在目录路径
        /// </summary>
        static string EntityPath = Path.Combine(Directory.GetParent(RootPath.Substring(0, RootPath.IndexOf(@"\bin"))).FullName, EntityNameSpace);

        /// <summary>
        /// IRepository所在目录路径
        /// </summary>
        static string IRepositoryPath = Path.Combine(Directory.GetParent(RootPath.Substring(0, RootPath.IndexOf(@"\bin"))).FullName, IRepositoryNameSpace);

        /// <summary>
        /// Repository所在目录路径
        /// </summary>
        static string RepositoryPath = Path.Combine(Directory.GetParent(RootPath.Substring(0, RootPath.IndexOf(@"\bin"))).FullName, RepositoryNameSpace);

        /// <summary>
        /// IService所在目录路径
        /// </summary>
        static string IServicePath = Path.Combine(Directory.GetParent(RootPath.Substring(0, RootPath.IndexOf(@"\bin"))).FullName, IServiceNameSpace);

        /// <summary>
        /// Service所在目录路径
        /// </summary>
        static string ServicePath = Path.Combine(Directory.GetParent(RootPath.Substring(0, RootPath.IndexOf(@"\bin"))).FullName, ServiceNameSpace);
        #endregion

        /// <summary>
        /// 数据库操作对象
        /// </summary>
        private static SqlSugarClient Db { get => InitDB(); }

        /// <summary>
        /// 初始化数据库连接
        /// </summary>
        /// <returns></returns>
        private static SqlSugarClient InitDB()
        {
            var db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = ConfigurationUtil.DBConnectionString,
                DbType = DbType.MySql,
                InitKeyType = InitKeyType.Attribute,//使用特性识别主键
                IsAutoCloseConnection = true
            });
            db.Ado.CommandTimeOut = 30;
            return db;
        }

        /// <summary>
        /// 生成所有代码
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="entityName">实体类名</param>
        /// <param name="isSqlSugarAttr">是否使用sqlsguar特性</param>
        public static void Builder(string tableName, string entityName = null, bool isSqlSugarAttr = true)
        {
            GenerateEntity(tableName, entityName, EntityNameSpace, isSqlSugarAttr);
            entityName = entityName ?? tableName;
            GenerateIRepository(entityName);
            GenerateRepository(entityName);
            GenerateIService(entityName);
            GenerateService(entityName);
        }

        /// <summary>
        /// 生成实体代码预览
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="entityName">实体类名</param>
        /// <param name="entityNameSpace">命名空间</param>
        /// <param name="isSqlSugarAttr">是否使用sqlsguar特性</param>
        /// <returns></returns>
        public static string GenerateEntityView(string tableName, string entityName = null, string entityNameSpace = EntityNameSpace, bool isSqlSugarAttr = true)
        {
            DbTableInfo dbTableInfo = Db.DbMaintenance.GetTableInfoList().Where(tt => tt.Name.Equals(tableName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            if (dbTableInfo == null)
            {
                throw new ArgumentNullException(tableName, "表明不存在");
            }
            string template = string.Empty;
            var currentAssembly = Assembly.GetExecutingAssembly();
            using (var stream = currentAssembly.GetManifestResourceStream($"{currentAssembly.GetName().Name}.Builder.Template.EntityTemplate.txt"))
            {
                if (stream != null)
                {
                    using (var reader = new StreamReader(stream))
                    {
                        template = reader.ReadToEnd();
                    }
                }
            }
            entityName = entityName ?? dbTableInfo.Name;
            template = template.Replace("@Comment", dbTableInfo.Description)
               .Replace("@Author", Author)
               .Replace("@Verison", Verison)
               .Replace("@GeneratorTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
               .Replace("@Namespace", entityNameSpace)
               .Replace("@ModelName", entityName);
            if (entityName.ToLower() != tableName.ToLower())
            {
                template = template.Replace("@TableAttribute", $"[SugarTable(\"{dbTableInfo.Name}\")]");
                isSqlSugarAttr = true;
            }
            else
            {
                template = template.Replace("@TableAttribute", "");
            }
            if (!isSqlSugarAttr)
            {
                template = template.Replace("using SqlSugar;", "");
            }
            StringBuilder sb = new StringBuilder();
            List<DbColumnInfo> c = Db.DbMaintenance.GetColumnInfosByTableName(tableName);
            foreach (var item in c)
            {
                string type = Db.Ado.DbBind.GetPropertyTypeName(item.DataType);
                if (type != "string" && type != "byte[]" && type != "object" && item.IsNullable)
                {
                    type = type + "?";
                }
                sb.AppendLine("\t\t/// <summary>");
                sb.AppendLine("\t\t/// " + item.ColumnDescription);
                sb.AppendLine("\t\t/// </summary>");
                if (item.IsPrimarykey)
                {
                    sb.AppendLine($"\t\t[SugarColumn(IsPrimaryKey = true{(item.IsIdentity ? ", IsIdentity = true" : "")})]");
                }
                sb.AppendLine($"\t\tpublic {type} {item.DbColumnName}" + " { get; set; }");
            }
            template = template.Replace("@Properties", sb.ToString());
            return template;
        }

        /// <summary>
        /// 生成实体代码cs文件并包含在项目内
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="entityName">实体类名</param>
        /// <param name="entityNameSpace">命名空间</param>
        /// <param name="isSqlSugarAttr">是否使用sqlsguar特性</param>
        public static void GenerateEntity(string tableName, string entityName = null, string entityNameSpace = "App.Entities", bool isSqlSugarAttr = true)
        {
            string code = GenerateEntityView(tableName, entityName, entityNameSpace, isSqlSugarAttr);
            entityName = entityName ?? tableName;
            CreateFile(code, EntityPath, entityName);
        }

        /// <summary>
        /// 生成IRepository代码预览
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="NameSpace"></param>
        /// <returns></returns>
        public static string GenerateIRepositoryView(string entityName, string NameSpace = IRepositoryNameSpace)
        {
            string template = string.Empty;
            var currentAssembly = Assembly.GetExecutingAssembly();
            using (var stream = currentAssembly.GetManifestResourceStream($"{currentAssembly.GetName().Name}.Builder.Template.IRepositoryTemplate.txt"))
            {
                if (stream != null)
                {
                    using (var reader = new StreamReader(stream))
                    {
                        template = reader.ReadToEnd();
                    }
                }
            }
            template = template.Replace("@Comment", "")
              .Replace("@Author", Author)
              .Replace("@Verison", Verison)
              .Replace("@GeneratorTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
              .Replace("@Namespace", NameSpace)
              .Replace("@IRepositoryName", $"I{entityName}Repository")
              .Replace("@ModelName", entityName);
            return template;
        }

        /// <summary>
        /// 生成IRepository接口文件
        /// </summary>
        /// <param name="entityName">实体类名</param>
        /// <param name="NameSpace">名称空间</param>
        public static void GenerateIRepository(string entityName, string NameSpace = IRepositoryNameSpace)
        {
            string code = GenerateIRepositoryView(entityName, NameSpace);
            string fileName = $"I{entityName}Repository";
            CreateFile(code, IRepositoryPath, fileName);
        }

        /// <summary>
        /// 生成Repository代码预览
        /// </summary>
        /// <param name="entityName">实体类名称</param>
        /// <param name="NameSpace">名称空间</param>
        /// <returns></returns>
        public static string GenerateRepositoryView(string entityName, string NameSpace = RepositoryNameSpace)
        {
            string template = string.Empty;
            var currentAssembly = Assembly.GetExecutingAssembly();
            using (var stream = currentAssembly.GetManifestResourceStream($"{currentAssembly.GetName().Name}.Builder.Template.RepositoryTemplate.txt"))
            {
                if (stream != null)
                {
                    using (var reader = new StreamReader(stream))
                    {
                        template = reader.ReadToEnd();
                    }
                }
            }
            template = template.Replace("@Comment", "")
              .Replace("@Author", Author)
              .Replace("@Verison", Verison)
              .Replace("@GeneratorTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
              .Replace("@Namespace", NameSpace)
              .Replace("@RepositoryName", $"{entityName}Repository")
              .Replace("@ModelName", entityName)
              .Replace("@IRepository", $"I{entityName}Repository");
            return template;
        }

        /// <summary>
        /// 生成Repository代码文件
        /// </summary>
        /// <param name="entityName">实体类名称</param>
        /// <param name="NameSpace">名称空间</param>
        public static void GenerateRepository(string entityName, string NameSpace = RepositoryNameSpace)
        {
            string code = GenerateRepositoryView(entityName, NameSpace);
            string fileName = $"{entityName}Repository";
            CreateFile(code, RepositoryPath, fileName);
        }

        /// <summary>
        /// 生成IService代码预览
        /// </summary>
        /// <param name="entityName">实体类名称</param>
        /// <param name="NameSpace">名称空间</param>
        /// <returns></returns>
        public static string GenerateIServiceView(string entityName, string NameSpace = IServiceNameSpace)
        {
            string template = string.Empty;
            var currentAssembly = Assembly.GetExecutingAssembly();
            using (var stream = currentAssembly.GetManifestResourceStream($"{currentAssembly.GetName().Name}.Builder.Template.IServiceTemplate.txt"))
            {
                if (stream != null)
                {
                    using (var reader = new StreamReader(stream))
                    {
                        template = reader.ReadToEnd();
                    }
                }
            }
            template = template.Replace("@Comment", "")
              .Replace("@Author", Author)
              .Replace("@Verison", Verison)
              .Replace("@GeneratorTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
              .Replace("@Namespace", NameSpace)
              .Replace("@IServiceName", $"I{entityName}Logic")
              .Replace("@ModelName", entityName);
            return template;
        }

        /// <summary>
        /// 生成IService代码文件
        /// </summary>
        /// <param name="entityName">实体类名称</param>
        /// <param name="NameSpace">名称空间</param>
        public static void GenerateIService(string entityName, string NameSpace = IServiceNameSpace)
        {
            string code = GenerateIServiceView(entityName, NameSpace);
            CreateFile(code, IServicePath, $"I{entityName}Logic");
        }

        /// <summary>
        /// 生成Service代码预览
        /// </summary>
        /// <param name="entityName">实体类名称</param>
        /// <param name="NameSpace">名称空间</param>
        /// <returns></returns>
        public static string GenerateServiceView(string entityName, string NameSpace = ServiceNameSpace)
        {
            string template = string.Empty;
            var currentAssembly = Assembly.GetExecutingAssembly();
            using (var stream = currentAssembly.GetManifestResourceStream($"{currentAssembly.GetName().Name}.Builder.Template.ServiceTemplate.txt"))
            {
                if (stream != null)
                {
                    using (var reader = new StreamReader(stream))
                    {
                        template = reader.ReadToEnd();
                    }
                }
            }
            template = template.Replace("@Comment", "")
              .Replace("@Author", Author)
              .Replace("@Verison", Verison)
              .Replace("@GeneratorTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
              .Replace("@Namespace", NameSpace)
              .Replace("@ServiceName", $"{entityName}Logic")
              .Replace("@IServiceName", $"I{entityName}Logic")
              .Replace("@ModelName", entityName)
              .Replace("@IRepositoryType", $"I{entityName}Repository");
            return template;
        }

        /// <summary>
        /// 生成IService代码文件
        /// </summary>
        /// <param name="entityName">实体类名称</param>
        /// <param name="NameSpace">名称空间</param>
        public static void GenerateService(string entityName, string NameSpace = ServiceNameSpace)
        {
            string code = GenerateServiceView(entityName, NameSpace);
            CreateFile(code, ServicePath, $"{entityName}Logic");
        }

        /// <summary>
        /// 创建cs文件
        /// </summary>
        /// <param name="code">代码</param>
        /// <param name="filePath">文件目录</param>
        /// <param name="fileName">文件名</param>
        static void CreateFile(string code, string filePath, string fileName)
        {
            if (!Directory.Exists(filePath))
            {
                throw new Exception($"所在路径：{EntityPath}不存在");
            }
            string codePath = Path.Combine(filePath, fileName + ".cs");
            if (!File.Exists(codePath))
            {
                FileInfo file = new FileInfo(codePath);
                using (FileStream stream = file.Create())
                {
                    using (StreamWriter writer = new StreamWriter(stream, System.Text.Encoding.UTF8))
                    {
                        writer.Write(code);
                        writer.Flush();
                    }
                }
            }
        }
    }
}
