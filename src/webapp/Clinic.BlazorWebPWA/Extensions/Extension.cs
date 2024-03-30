using Clinic.DTO.Models.Dto;

namespace Clinic.BlazorWebPWA.Extensions;

public static class Extension
{
    public static string ConvertSpecEnum(this int value)
    {
        string chuyenKhoa = value switch
        {
            0 => "None",
            1 => "Cardiology", //Khoa tim mạch
            2 => "Ophthalmology", //Khoa mắt
            3 => "Otorhinolaryngology", //Khoa tai mũi họng
            4 => "Dermatology", //Khoa da liễu
            5 => "Gastroenterology", //Khoa tiêu hóa
            6 => "Pediatrician", //Khoa nhi
            7 => "Obstetrician", //Khoa sản
            8 => "Neurology", //Khoa thần kinh
            9 => "Traditional Medicine", //Khoa y học cổ truyền
            10 => "Hematology", //Khoa huyết học
            11 => "Oral maxillofacial", //Khoa răng hàm mặt
            12 => "Oncology", //Khoa ung thư
            13 => "Orthopedics", //Khoa ngoại chỉnh hình
            14 => "Traumatology", //Khoa chấn thương
            15 => "Pathology" //Khoa bệnh lý học
        };
        return chuyenKhoa;
    }
    public static List<TimeSpan> GetTimeSlots()
    {
        TimeSpan startTime = new TimeSpan(8, 30, 0);
        TimeSpan endTime = new TimeSpan(17, 0, 0);
        TimeSpan interval = new TimeSpan(0, 30, 0);
        List<TimeSpan> timeSlots = new List<TimeSpan>();

        while (startTime <= endTime)
        {
            timeSlots.Add(startTime);
            startTime = startTime.Add(interval);
        }

        return timeSlots;
    }
    public static string GetDateTime(this long ticks)
    {
        DateTime result = new DateTime(ticks);

        return result.ToString();
    }
    public static HttpMethod ConvertMethod(this ApiType type)
    {
        return type switch
        {
            ApiType.POST => HttpMethod.Post,
            ApiType.PUT => HttpMethod.Put,
            ApiType.DELETE => HttpMethod.Delete,
            _ => HttpMethod.Get
        };
    }
    public static bool ContainsCaseInsensitive(this string source, string substring)
    {
        return source?.IndexOf(substring, StringComparison.OrdinalIgnoreCase) > -1;
    }
}