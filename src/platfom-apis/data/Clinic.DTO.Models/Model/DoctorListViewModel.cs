namespace Clinic.DTO.Models
{
    public class DoctorListViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Email { get; set; }
        public int ExperienceYear { get; set; }
        public long DateOfBirth { get; set; }
        public string Title { get; set; }
        public int Specialization { get; set; }
        public string ClinicNum { get; set; }
        public double AverageRating { get; set; }
        public string Introduction { get; set; }
        public List<long> OrderedTimes { get; set; }
    }

    public enum Specialization
    {
        None,
        Cardiology, //Khoa tim mạch
        Ophthalmology, //Khoa mắt
        Otorhinolaryngology, //Khoa tai mũi họng
        Dermatology, //Khoa da liễu
        Gastroenterology, //Khoa tiêu hóa
        Pediatrician, //Khoa nhi
        Obstetrician, //Khoa sản
        Neurology, //Khoa thần kinh
        TraditionalMedicine, //Khoa y học cổ truyền
        Hematology, //Khoa huyết học
        Oralmaxillofacial, //Khoa răng hàm mặt
        Oncology, //Khoa ung thư
        Orthopedics, //Khoa ngoại chỉnh hình
        Traumatology, //Khoa chấn thương
        Pathology //Khoa bệnh lý học
    }
}