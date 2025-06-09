namespace EMSAPI.Models
{
    public class Movement
    {
        public int MoveID { get; set; }
        public string? Message { get; set; }
        public int ButtonSignal { get; set; } = default!;
        public int SensorSignal { get; set; } = default!;
        public string DataFrom { get; set; } = default!;
        
    }
}