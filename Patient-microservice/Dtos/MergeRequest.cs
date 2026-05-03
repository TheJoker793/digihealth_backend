namespace Patient_microservice.Dtos
{
    public class MergeRequest
    {
        public Guid SourceId { get; set; }
        public Guid TargetId { get; set; }
    }
}
