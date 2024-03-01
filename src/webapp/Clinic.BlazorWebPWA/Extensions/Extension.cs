namespace Clinic.BlazorWebPWA.Extensions;

public static class Extension
{
    public static string ConvertSpecEnum(this int value)
    {
        string chuyenKhoa = value switch
        {
            0 => "None",
            1 => "Khoa Nội Tim Mạch",
            2 => "Khoa Nội Tổng Hợp",
            3 => "Khoa Chẩn Đoán Hình Ảnh Và Thăm Dò Chức Năng",
            4 => "Khoa Nội Tổng Quát",
            5 => "Khoa Khám Bệnh",
            6 => "Khoa Nhi",
            7 => "Khoa Ung Bướu",
            8 => "Khoa Nội Thần Kinh",
            9 => "Khoa Y Học Cổ Truyền",
            10 => "Khoa Nội Thần Kinh Đột Quỵ",
            11 => " Khoa Răng Hàm mặt"
        };
        return chuyenKhoa;
    }
}