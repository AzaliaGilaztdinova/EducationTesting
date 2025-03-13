using System;
using Microsoft.Extensions.DependencyInjection;

namespace EducationTesting.Client.Helpers
{
    /// <summary>
    /// Ленивая загрузка
    /// </summary>
    /// <typeparam name="T">Тип объекта</typeparam>
    internal class Lazier<T> : Lazy<T> where T : class
    {
        /// <summary>
        /// Конструктор ленивой загрузки
        /// </summary>
        /// <param name="provider"></param>
        public Lazier(IServiceProvider provider)
            : base(provider.GetRequiredService<T>)
        {
        }
    }
}