namespace SAM.Taskboard.DataProvider.Models
{
    public class Attachment
    {
        public int Id { get; set; }

        public byte[] Document { get; set; }

        public int TaskId { get; set; }

        public Task Task { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Extension { get; set; }
    }
}
