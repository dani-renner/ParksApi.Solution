using System.ComponentModel.DataAnnotations;

namespace ParksApi.Models
{
  public class Park
  {
    public int ParkId { get; set; }
    [Required]
    public string Name { get; set; }
    public int SqMiles { get; set; }
    [Required]
    public string Location { get; set; }
  }
}