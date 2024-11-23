namespace NATS.Services.Localization;

public static class DisplayNames
{
    public const string User = "Người dùng";
    public const string Role = "Vị trí";
    public const string Photo = "Hình ảnh";
    public const string Home = "Trang chủ";
    public const string Dashboard = "Bảng điều khiển";
    public const string GeneralSettings = "Cài đặt chung";
    public const string Education = "Đào tạo";
    public const string Product = "Sản phẩm";
    public const string BusinessService = "Dịch vụ";
    public const string Features = "Tác dụng";
    public const string Enquiry = "Câu hỏi";
    public const string Id = "Mã số";
    public const string Name = "Tên";
    public const string Account = "Tài khoản";
    public const string Profile = "Hồ sơ";
    public const string UserName = "Tên đăng nhập";
    public const string Password = "Mật khẩu";
    public const string CurrentPassword = "Mật khẩu hiện tại";
    public const string NewPassword = "Mật khẩu mới";
    public const string ConfirmationPassword = "Xác nhận mật khẩu";
    public const string FullName = "Tên đầy đủ";
    public const string RoleName = "Chức vụ";
    public const string Description = "Mô tả";
    public const string Quatity = "Số lượng";
    public const string StartingAt = "Bắt đầu vào lúc";
    public const string EndingAt = "Kết thúc vào lúc";
    public const string Category = "Phân loại";
    public const string NormalizedTitle = "Tiêu đề không dấu";
    public const string Title = "Tiêu đề";
    public const string Status = "Tình trạng";
    public const string GroupName = "Tên nhóm";
    public const string OrderByField = "Trường sắp xếp";
    public const string OrderByAscending = "Thứ tự sắp xếp";
    public const string SearchByField = "Trường tìm kiếm";
    public const string SearchByContent = "Nội dung tìm kiếm";
    public const string Page = "Trang";
    public const string ResultsPerPage = "Kết quả mỗi trang";
    public const string PageCount = "Số trang";
    public const string ResultsCount = "Số kết quả";
    public const string Results = "Kết quả";
    public const string Thumbnail = "Ảnh xem trước";
    public const string ThumbnailFile = "File ảnh xem trước";
    public const string Operation = "Thao tác";
    public const string MainPhoto = "Ảnh chính";
    public const string MessageFromUs = "Thông điệp";
    public const string AboutUs = "Về chúng tôi";
    public const string WhyChooseUs = "Vì sao chọn chúng tôi";
    public const string OurDifference = "Sự khác biệt của chúng tôi";
    public const string OurCulture = "Văn hoá của chúng tôi";
    public const string TeamMembers = "Đội ngũ";
    public const string BusinessCertificates = "Chứng chỉ";
    public const string ApplicationName = "Tên trang web";
    public const string ApplicationShortName = "Tên trang web (viết tắt)";
    public const string FavIcon = "Biểu tượng trang web";
    public const string UnderMaintainance = "Tình trạng bảo trì";
    public const string Content = "Nội dung";
    public const string PhotoFile = "File ảnh";
    public const string Course = "Khoá học";
    public const string CourseSection = "Trọng tâm khoá học";
    public const string Summary = "Tóm tắt";
    public const string Introduction = "Giới thiệu";
    public const string IntroductionItem = "Giới thiệu";
    public const string Detail = "Chi tiết";
    public const string Index = "Thứ tự";
    public const string HomePageSliderItem = "Trình chiếu ảnh";
    public const string Post = "Bài viết";
    public const string PostCategory = "Chuyên mục bài viết";
    public const string CreatedDateTime = "Tạo vào lúc";
    public const string UpdatedDateTime = "Sửa vào lúc";
    public const string Views = "Lượt xem";
    public const string IsPinned = "Được ghim";
    public const string IsPublished = "Được xuất bản";
    public const string Statistics = "Thống kê";
    public const string TotalCategories = "Chuyên mục";
    public const string TotalPosts = "Bài viết";
    public const string TotalViews = "Lượt xem";
    public const string UnpublishedPosts = "Chưa xuất bản";
    public const string PhoneNumber = "Số điện thoại";
    public const string Zalo = "Zalo";
    public const string Email = "Địa chỉ email";
    public const string Address = "Địa chỉ";
    public const string ReceivedDateTime = "Ngày nhận";
    public const string IsCompleted = "Đã hoàn thành";
    public const string ContactInfo = "Thông tin liên hệ";
    public const string WorkingHours = "Giờ làm việc";
    public const string RecordedAt = "Ghi nhận lúc";
    public const string AccessCount = "Lượt truy cập";
    public const string GuessCount = "Khách truy cập";

    private static readonly Dictionary<string, string> names;

    static DisplayNames()
    {
        names = new Dictionary<string, string>();
        FieldInfo[] fields = typeof(DisplayNames).GetFields(BindingFlags.Public | BindingFlags.Static);
        foreach (var field in fields)
        {
            names.Add(field.Name, (string)field.GetValue(null));
        }
    }

    public static string Get(string objectName)
    {
        if (objectName == null)
        {
            throw new ArgumentNullException(nameof(objectName));
        }
        return names
            .Where(pair => pair.Key == objectName.ToWordsFirstLetterCapitalized())
            .Select(pair => pair.Value)
            .Single();
    }

    public static string Get(object[] objectName)
    {
        if (objectName == null || !objectName.Any())
        {
            throw new ArgumentException($"{nameof(objectName)} must be non-null and contain at least 1 element.");
        }
        return Get(objectName
            .Reverse()
            .Where(name => name != null)
            .Select(name => name.ToString().ToWordsFirstLetterCapitalized())
            .First());
    }
}