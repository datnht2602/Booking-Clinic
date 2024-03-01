using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.DTO.Models
{
    public class DoctorListViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public int ExperienceYear { get; set; }
        public string Title { get; set; }
        public int Specialization { get; set; }
        public string ClinicNum { get; set; }
        public double AverageRating { get; set; }
        public string Introduction { get; set; }
    }

    public enum Specialization
    {
        None,
        KhoaNoiTimMach,
        KhoaNoiTongHop,
        KhoaChanDoanHinhAnhVaThamDoChucNang,
        KhoaNoiTongQuat,
        KhoaKhambenh,
        KhoaNhi,
        KhoaUngBuou,
        KhoaNoiThanKinh,
        KhoaYHocCoTruyen,
        KhoaNoiThanKinhDotQuy,
        KhoaRangHamMat,
    }
}