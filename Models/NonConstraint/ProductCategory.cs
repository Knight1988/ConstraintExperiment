using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ConstraintExperiment.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ConstraintExperiment.Models.NonConstraint;

public class ProductCategory : IBaseModel
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }
    [Unicode(false)]
    public string Name { get; set; }
}