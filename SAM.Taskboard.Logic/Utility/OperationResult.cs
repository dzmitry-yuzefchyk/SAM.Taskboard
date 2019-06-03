namespace SAM.Taskboard.Logic.Utility
{
    public class OperationResult<TViewModel> where TViewModel : class
    {
        public TViewModel Model { get; set; }
        public GenericServiceResult Message { get; set; }
    }
}
