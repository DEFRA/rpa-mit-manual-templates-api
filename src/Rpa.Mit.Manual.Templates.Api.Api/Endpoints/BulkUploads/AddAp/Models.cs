namespace BulkUploads.AddAp
{
    internal sealed class Request
    {
        public IFormFile File { get; set; }

        internal sealed class Validator : Validator<Request>
        {
            public Validator()
            {

            }
        }
    }

    internal sealed class Response
    {
        public string Message { get; set; } = string.Empty;
    }
}
