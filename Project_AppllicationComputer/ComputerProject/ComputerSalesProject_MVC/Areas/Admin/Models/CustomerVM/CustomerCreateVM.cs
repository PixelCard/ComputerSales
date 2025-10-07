using System.ComponentModel.DataAnnotations;

public sealed class CustomerCreateVM
{
    public string? IMG { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập tên khách hàng")]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Display(Name = "Địa chỉ")]
    [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
    public string address { get; set; } = string.Empty; // giữ đúng casing theo DTO

    [Display(Name = "Số điện thoại")]
    public string? sdt { get; set; } // giữ đúng tên trường như DTO

    [Display(Name = "Ngày tạo/Ngày đăng ký")]
    [DataType(DataType.Date)]
    public DateTime Date { get; set; } = DateTime.Today;

    [Display(Name = "ID tài khoản gắn với khách hàng")]
    [Range(1, int.MaxValue, ErrorMessage = "IDAccount phải > 0")]
    public int IDAccount { get; set; }
}