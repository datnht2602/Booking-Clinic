using System.ComponentModel.DataAnnotations.Schema;

namespace Clinic.Identity.Models;

public class ScheduleTime
{
    public int Id { get; set; }
    [ForeignKey("ApplicationUser")]
    public string UserId { get; set; }
    public virtual ApplicationUser ApplicationUser { get; set; }
    public long Time { get; set; }
   
}