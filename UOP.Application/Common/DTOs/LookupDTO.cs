namespace UOP.Application.Common.DTOs
{
    public class LookupDTO<T> where T : struct
    {
        public T Id { get; set; }
        public string? NameEn { get; set; }
        public string? NameAr { get; set; }
        public string? Order { get; set; }
    }
}
