using System;

namespace AddLuaMods.Tests.Tools
{
    /// <summary>
    /// Интерфейс для обёртывание освобождение ресурса.
    /// </summary>
    /// <typeparam name="T">Тип хранимого значения.</typeparam>
    public interface IDisposableBag<T> : IDisposable
    {
        /// <summary>
        /// Хранимый ресурс.
        /// </summary>
        T Value { get; }

        /// <summary>
        /// Флаг запрета изменения <see cref="Value"/>.
        /// </summary>
        bool IsReadOnly { get; }

        /// <summary>
        /// Заменить хранимое значение.
        /// </summary>
        /// <param name="newValue">Новое значение.</param>
        void ReplaceValue(T newValue);
    }
}
