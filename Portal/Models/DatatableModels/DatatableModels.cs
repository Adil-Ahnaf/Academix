using Newtonsoft.Json;

namespace Portal.Models.DatatableModels
{
    /// <summary>
    /// A full result, as understood by jQuery DataTables.
    /// </summary>
    /// <typeparam name="T">The data type of each row.</typeparam>
    public class DtResult<T>
    {
        [JsonProperty("draw")]
        public int Draw { get; set; }

        [JsonProperty("recordsTotal")]
        public int RecordsTotal { get; set; }

        [JsonProperty("recordsFiltered")]
        public int RecordsFiltered { get; set; }

        [JsonProperty("data")]
        public IEnumerable<T> Data { get; set; } = Enumerable.Empty<T>();

        [JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)]
        public string? Error { get; set; }

        public string? PartialView { get; set; }
    }


    /// <summary>
    /// Parameters sent by jQuery DataTables.
    /// </summary>
    public class DtParameters
    {
        public int Draw { get; set; }

        public DtColumn[]? Columns { get; set; }

        public DtOrder[]? Order { get; set; }

        public int Start { get; set; }

        public int Length { get; set; }

        public DtSearch? Search { get; set; }

        public IEnumerable<string>? AdditionalValues { get; set; }

        public string? SortOrder
        {
            get
            {
                if (Columns == null ||
                    Order == null ||
                    Order.Length == 0)
                {
                    return null;
                }

                int columnIndex = Order[0].Column;

                if (columnIndex < 0 ||
                    columnIndex >= Columns.Length)
                {
                    return null;
                }

                string? columnName = Columns[columnIndex].Data;

                if (string.IsNullOrWhiteSpace(columnName))
                {
                    return null;
                }

                string direction =
                    string.Equals(
                        Order[0].Dir,
                        "desc",
                        StringComparison.OrdinalIgnoreCase)
                        ? "desc"
                        : "asc";

                return $"{columnName} {direction}";
            }
        }
    }


    /// <summary>
    /// A DataTables column.
    /// </summary>
    public class DtColumn
    {
        public string? Data { get; set; }

        public string? Name { get; set; }

        public bool Searchable { get; set; }

        public bool Orderable { get; set; }

        public DtSearch? Search { get; set; }
    }


    /// <summary>
    /// Ordering information sent by DataTables.
    /// </summary>
    public class DtOrder
    {
        public int Column { get; set; }

        // DataTables sends "asc" or "desc".
        public string? Dir { get; set; }
    }


    /// <summary>
    /// Search information sent by DataTables.
    /// </summary>
    public class DtSearch
    {
        public string? Value { get; set; }

        public bool Regex { get; set; }
    }


    /// <summary>
    /// Internal enum used by the dynamic ordering extension.
    /// </summary>
    public enum DtOrderDir
    {
        Asc,
        Desc
    }
}