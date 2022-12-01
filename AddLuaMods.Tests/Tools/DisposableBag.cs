using System;

namespace AddLuaMods.Tests.Tools
{
    /// <summary>
    /// Класс для обёртки освобождения ресурса.
    /// </summary>
    /// <typeparam name="T">Тип хранимого значения.</typeparam>
    public class DisposableBag<T> : IDisposableBag<T>
    {
        private DisposeAction? _onDisposing;
        private bool _disposed;
        private T _value;

        /// <summary>
        /// Акция уничтожения хранимого объекта.
        /// </summary>
        /// <param name="value"></param>
        public delegate void DisposeAction(ref T value);

        /// <summary>
        /// Конструктор экземпляра <see cref="DisposableBag{T}"/>.
        /// </summary>
        /// <param name="value">Хранимое значение.</param>
        /// <param name="onDisposing">Метод выполняемы при освобождении ресурса.</param>
        /// <param name="isReadOnly">Флаг запрета изменения <see cref="Value"/>.</param>
        public DisposableBag(T value, Action<T> onDisposing, bool isReadOnly = true)
            : this(value, (ref T value1) => onDisposing(value1), isReadOnly)
        {
        }

        /// <summary>
        /// Конструктор экземпляра <see cref="DisposableBag{T}"/>.
        /// </summary>
        /// <param name="value">Хранимое значение.</param>
        /// <param name="onDisposing">Метод выполняемы при освобождении ресурса.</param>
        /// <param name="isReadOnly">Флаг запрета изменения <see cref="Value"/>.</param>
        public DisposableBag(T value, DisposeAction? onDisposing = null, bool isReadOnly = true)
        {
            _value = value;
            _onDisposing = onDisposing;
            IsReadOnly = isReadOnly;
        }

        /// <summary>
        /// Конструктор экземпляра <see cref="DisposableBag{T}"/>.
        /// </summary>
        /// <param name="value">Хранимое значение.</param>
        /// <param name="onDisposing">Метод выполняемы при освобождении ресурса.</param>
        /// <param name="isReadOnly">Флаг запрета изменения <see cref="Value"/>.</param>
        public DisposableBag(ref T value, DisposeAction? onDisposing = null, bool isReadOnly = true)
        {
            _value = value;
            _onDisposing = onDisposing;
            IsReadOnly = isReadOnly;
        }

        /// <inheritdoc />
        public T Value
        {
            get => _value;
        }

        /// <inheritdoc />
        public bool IsReadOnly { get; }

        /// <inheritdoc />
        public void ReplaceValue(T newValue)
        {
            if (IsReadOnly)
            {
                throw new InvalidOperationException($"Операция запрещена: {nameof(IsReadOnly)} = {IsReadOnly}.");
            }

            _value = newValue;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            _onDisposing?.Invoke(ref _value);
            _onDisposing = null;
        }

        /// <summary>
        /// Деструктор.
        /// </summary>
        ~DisposableBag()
        {
            Dispose();
        }
    }
}