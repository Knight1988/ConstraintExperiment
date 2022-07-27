using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ConstraintExperiment.Interfaces;

namespace ConstraintExperiment.Models.NonConstraint;

public class Customer : IBaseModel
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }
    public string Name { get; set; }
}