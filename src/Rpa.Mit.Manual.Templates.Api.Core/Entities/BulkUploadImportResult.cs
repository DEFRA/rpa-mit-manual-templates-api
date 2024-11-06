using System.Diagnostics.CodeAnalysis;

namespace Rpa.Mit.Manual.Templates.Api.Core.Entities
{
    [ExcludeFromCodeCoverage]
    public readonly struct BulkUploadImportResult<T, E>
    {
        private readonly bool _success;
        public readonly T BulkUploadImport;
        public readonly E Error;

        private BulkUploadImportResult(T v, E e, bool success)
        {
            BulkUploadImport = v;
            Error = e;
            _success = success;
        }

        public bool IsOk => _success;

        public static BulkUploadImportResult<T, E> Ok(T v)
        {
            return new(v, default!, true);
        }

        public static BulkUploadImportResult<T, E> Err(E e)
        {
            return new(default(T)!, e, false);
        }

        public static implicit operator BulkUploadImportResult<T, E>(T v) => new(v, default!, true);
        public static implicit operator BulkUploadImportResult<T, E>(E e) => new(default(T)!, e, false);

        public R Match<R>(
                Func<T, R> success,
                Func<E, R> failure) =>
            _success ? success(BulkUploadImport) : failure(Error);
    }
}
