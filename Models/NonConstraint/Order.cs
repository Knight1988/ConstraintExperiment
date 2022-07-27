using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ConstraintExperiment.Interfaces;

namespace ConstraintExperiment.Models.NonConstraint;

public class Order : IBaseModel
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public DateTime Date { get; set; }
}