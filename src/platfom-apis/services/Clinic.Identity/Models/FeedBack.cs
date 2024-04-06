using System.ComponentModel.DataAnnotations.Schema;

namespace Clinic.Identity.Models;

public class FeedBack
{
    public int Id { get; set; }
    [ForeignKey("ApplicationUser")]
    public string DoctorId { get; set; }
    public virtual ApplicationUser ApplicationUser { get; set; }
    public string UserName { get; set; }
    public string BookingId { get; set; }
    public int Rate { get; set; }
    public string Comment { get; set; }

}