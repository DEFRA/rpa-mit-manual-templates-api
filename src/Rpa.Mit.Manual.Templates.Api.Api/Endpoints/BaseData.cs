namespace Rpa.Mit.Manual.Templates.Api.Api.Endpoints
{
    public abstract class BaseData
    {
        private readonly ConnectionStrings _options;

        protected BaseData(ConnectionStrings options) 
        {
            _options = options;
        }

        public virtual string DbConn => _options.PostGres;
    }
}
