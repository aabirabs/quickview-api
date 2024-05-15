public class Alert
{
    public int Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public string Message { get; set; } = String.Empty;
    public string Probe {get;set;}= String.Empty;
    public string Group {get;set;}= String.Empty;
    public string Device{get;set;}=String.Empty;
    public  String DisplayLastValue{get;set;} = String.Empty;
}