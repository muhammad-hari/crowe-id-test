namespace Events.Domain.DataTable
{
    public class ColumnParameters
    {
        public string Data { get; set; }
        public string Name { get; set; }
        public bool Searchable { get; set; }
        public bool Orderable { get; set; }
        public SearchParameters Search { get; set; }
    }
}
