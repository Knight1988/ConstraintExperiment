namespace ConstraintExperiment.Models;

public class PerformanceReport
{
    public string Content { get; set; }
    public List<long> ConstaintTimes { get; set; }
    public List<long> NonConstaintTimes { get; set; }
}