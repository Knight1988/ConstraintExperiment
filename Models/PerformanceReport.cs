namespace ConstraintExperiment.Models;

public class PerformanceReport
{
    public string Content { get; set; }
    public List<long> ConstraintTimes { get; set; }
    public List<long> NonConstraintTimes { get; set; }
}