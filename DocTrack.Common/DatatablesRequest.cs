namespace DocTrack.Common
{
    public class DatatablesRequest
    {
        public string Search { get; set; }

        public int Start { get; set; }

        public int Length { get; set; }

        public string SortColumnName { get; set; }

        public string SortDir { get; set; }
    }
}
