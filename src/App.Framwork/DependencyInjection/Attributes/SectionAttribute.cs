using System;

namespace App.Framwork.DependencyInjection.Attributes
{
    /// <summary>
    /// appsetting配置特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class SectionAttribute : Attribute
    {
        /// <summary>
        /// appsettings节点
        /// </summary>
        public string Section { get; }

        public Lifetime Lifetime { get; }
        public SectionAttribute(string section, Lifetime lifetime = Lifetime.Scoped)
        {
            Section = section;
            Lifetime = Lifetime;
        }
    }

    /// <summary>
    /// 生命周期
    /// </summary>
    public enum Lifetime
    {
        /// <summary>
        /// 瞬时
        /// </summary>
        Transient,
        /// <summary>
        /// 范围
        /// </summary>
        Scoped,
        /// <summary>
        /// 单列
        /// </summary>
        Singleton
    }
}