using System.Text;

namespace Rpa.Mit.Manual.Templates.Api.Api.Endpoints.Reports
{
    internal static class ConvertToCsv
    {
        public static string AsCsv<T>(this IEnumerable<T> items) where T : class
        {
            var csvBuilder = new StringBuilder();
            var properties = typeof(T).GetProperties();
            csvBuilder.AppendLine(String.Join(",", properties.Select(p => p.Name.ToCsvValue()).ToArray()));
            foreach (T item in items)
            {
                string line = String.Join(",", properties.Select(p => p.GetValue(item, null).ToCsvValue()).ToArray());
                csvBuilder.AppendLine(line);
            }
            return csvBuilder.ToString();
        }

        private static string ToCsvValue<T>(this T item) where T : class
        {
            if (item == null)
            {
                return "";
            }

            if (item is string)
            {
                return String.Format("\"{0}\"", item.ToString().Replace("\"", "\""));
            }

            double dummy;
            if (double.TryParse(item.ToString(), out dummy))
            {
                return String.Format("{0}", item);
            }
            return String.Format("\"{0}\"", item);
        }
    }
}
