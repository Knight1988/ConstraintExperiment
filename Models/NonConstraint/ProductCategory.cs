using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConstraintExperiment.Models.NonConstraint;

public class ProductCategory
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
}