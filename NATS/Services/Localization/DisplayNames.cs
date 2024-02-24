namespace NATS.Services.Localization;

public static class DisplayNames
{
    public const string User = "Người dùng";
    public const string Role = "Vị trí";
    public const string Table = "Bàn";
    public const string CustomerCount = "Số khách";
    public const string Payment = "Thanh toán";
    public const string Supply = "Đơn nhập hàng";
    public const string Order = "Đơn đặt hàng";
    public const string Expense = "Chi phí";
    public const string Photo = "Hình ảnh";
    public const string Announcement = "Thông báo";
    public const string Id = "Mã số";
    public const string Name = "Tên";
    public const string Account = "Tài khoản";
    public const string Profile = "Hồ sơ";
    public const string UserName = "Tên đăng nhập";
    public const string Password = "Mật khẩu";
    public const string CurrentPassword = "Mật khẩu hiện tại";
    public const string NewPassword = "Mật khẩu mới";
    public const string ConfirmationPassword = "Xác nhận mật khẩu";
    public const string CreatedAt = "Tạo vào lúc";
    public const string UpdatedAt = "Sửa vào lúc";
    public const string Note = "Ghi chú";
    public const string Description = "Mô tả";
    public const string Unit = "Đơn vị";
    public const string Price = "Giá tiền";
    public const string SuppliedAt = "Ngày nhập hàng";
    public const string SuppliedQuatity = "Số lượng nhập hàng";
    public const string StockingQuatity = "Số lượng trong kho";
    public const string Amount = "Số tiền";
    public const string PaymentAmount = "Số tiền";
    public const string Quatity = "Số lượng";
    public const string PaidAmount = "Số tiền đã thanh toán";
    public const string PaidAt = "Thanh toán vào lúc";
    public const string StartingAt = "Bắt đầu vào lúc";
    public const string EndingAt = "Kết thúc vào lúc";
    public const string Category = "Phân loại";
    public const string Title = "Tiêu đề";
    public const string Content = "Nội dung";
    public const string IsPinned = "Được đánh dấu";
    public const string OrderedAt = "Đặt vào lúc";
    public const string DeliveredAt = "Giao vào lúc";
    public const string ReservedAt = "Đặt bàn vào lúc";
    public const string CheckedInAt = "Nhập bàn vào lúc";
    public const string CheckedOutAt = "Xuất bàn vào lúc";
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
    public const string OrderItem = "Món đặt";
    public const string OrderedOrderItem = "Món đã đặt";
    public const string ProcessingOrderItemCount = "Món đang chờ";
    public const string Operation = "Thao tác";

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