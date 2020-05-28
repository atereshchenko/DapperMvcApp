namespace DapperMvcApp.Models.Entities
{
    public class AccessType
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool Read { get; set; }
        public bool Write { get; set; }
        public bool Delete { get; set; }        
    }
}
