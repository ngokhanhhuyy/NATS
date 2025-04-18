using System.Text.RegularExpressions;

namespace NATS.Services;

public sealed partial class DataInitializer
{
    private DatabaseContext _context;
    private UserManager<User> _userManager;
    private RoleManager<Role> _roleManager;
    
    public void InitializeData(IApplicationBuilder builder)
    {
        using IServiceScope serviceScope = builder.ApplicationServices.CreateScope();

        _context = serviceScope.ServiceProvider.GetService<DatabaseContext>();
        _userManager = serviceScope.ServiceProvider.GetService<UserManager<User>>();
        _roleManager = serviceScope.ServiceProvider.GetService<RoleManager<Role>>();

        _context.Database.OpenConnection();
        _context.Database.EnsureCreated();

        using IDbContextTransaction transaction = _context.Database.BeginTransaction();

        try
        {
            InitializeRoles();
            InitializeUsers();
            IntializeAboutUsIntroduction();
            InitializeMembers();
            InitializeGeneralSettings();
            InitializeCertificates();
            InitializeSummaryItems();
            InitializeCourses();
            InitializeServices();
            InitializeSliderItems();
            InitializePosts();
            InitializeContacts();

            _context.SaveChanges();
            transaction.Commit();
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
        finally
        {
            _context.Database.CloseConnection();
        }
    }

    private void InitializeRoles()
    {
        if (!_roleManager.Roles.Any())
        {
            Console.WriteLine("Initializing Roles");
            List<Role> roles = new List<Role>
            {
                new Role
                {
                    Name = "Developer",
                    DisplayName = "Nhà phát triển",
                },
                new Role
                {
                    Name = "Admin",
                    DisplayName = "Quản trị viên",
                },
                new Role
                {
                    Name = "ContentCreator",
                    DisplayName = "Sáng tạo nội dung",
                },
            };

            foreach (Role role in roles)
            {
                IdentityResult result = _roleManager
                    .CreateAsync(role)
                    .GetAwaiter()
                    .GetResult();

                if (!result.Succeeded)
                {
                    string description = result.Errors.FirstOrDefault()?.Description;
                    throw new InvalidOperationException(description);
                }

                _context.SaveChanges();
            }
        }
    }

    private void InitializeUsers()
    {
        if (!_userManager.Users.Any())
        {
            Console.WriteLine("Initializing Users");
            Dictionary<User, (string Password, string RoleName)> users;
            users = new Dictionary<User, (string Password, string RoleName)>
            {
                {
                    new User
                    {
                        UserName = "ngokhanhhuyy",
                    },
                    ("Huyy47b1", "Developer")
                },
                {
                    new User
                    {
                        UserName = "thuytrangnguyen",
                    },
                    ("trang123", "Admin")
                },
                {
                    new User
                    {
                        UserName = "anhtaingo",
                    },
                    ("tai123", "ContentCreator")
                }
            };

            foreach (KeyValuePair<User, (string Password, string RoleName)> pair in users)
            {
                string description;
                IdentityResult result = _userManager
                    .CreateAsync(pair.Key, pair.Value.Password)
                    .GetAwaiter()
                    .GetResult();

                if (!result.Succeeded)
                {
                    description = result.Errors.FirstOrDefault()?.Description;
                    throw new InvalidOperationException(description);
                }

                result = _userManager
                    .AddToRoleAsync(pair.Key, pair.Value.RoleName)
                    .GetAwaiter()
                    .GetResult();

                if (!result.Succeeded)
                {
                    description = result.Errors.FirstOrDefault()?.Description;
                    throw new InvalidOperationException();
                }
            }
        }

        _context.SaveChanges();
    }

    private void InitializeGeneralSettings()
    {
        if (!_context.GeneralSettings.Any())
        {
            Console.WriteLine("Initializing AboutUsIntroduction");
            Console.WriteLine("Initializing GeneralSettings");
            GeneralSettings settings = new GeneralSettings
            {
                ApplicationName = "Trung tâm Khoa học Đào tạo và Thẩm mỹ Quốc Gia",
                ApplicationShortName = "NATS",
                FavIconUrl = "/images/favicon.ico"
            };

            _context.GeneralSettings.Add(settings);
            _context.SaveChanges();
        }
    }

    private void IntializeAboutUsIntroduction()
    {
        if (!_context.AboutUsIntroductions.Any())
        {
            AboutUsIntroduction aboutUsIntroduction = new AboutUsIntroduction
            {
                ThumbnailUrl = "/images/front-pages/about-us/1.jpg",
                MainQuoteContent = "Trong cuộc sống hiện đại, nhiều áp lực và lo lắng " +
                                    "khiến cho chúng ta càng ngày càng cảm thấy căng thảng, " +
                                    "mệt mỏi và có xu hướng tìm các giải pháp để cải thiện " +
                                    "sức khỏe và làm chậm quá trình lão hóa." +
                                    Environment.NewLine + Environment.NewLine +
                                    "Trung Tâm Khoa Học Đào Tạo và Thẩm Mỹ Quốc Gia (NATS) " +
                                    "hướng đến việc xây dựng văn hóa sức khỏe, thẩm mỹ " +
                                    "và lan tỏa giá trị tốt đẹp này đến với cộng đồng.",
                AboutUsContent = "Trung Tâm Khoa Học Đào Tạo và Thẩm Mỹ Quốc Gia (NATS) " +
                                "là đơn vị trực thuộc Viện Khoa học Giáo dục và Môi trường " +
                                "(IEES), ra đời với sứ mệnh giúp định hướng nghề nghiệp cho " +
                                "thế hệ trẻ. Đào tạo cho họ các kỹ thuật thẩm mỹ không xâm " +
                                "lấn theo phong thủy độc đáo, các kỹ thuật đả thông kinh " +
                                "lạc, các phương pháp thải độc và trẻ hóa tế bào. Chuyển " +
                                "giao quy trình mở spa dưỡng sinh trị liệu.",
                WhyChooseUsContent = "Trung Tâm Khoa Học Đào Tạo và Thẩm Mỹ Quốc Gia (NATS) " +
                                    "là đơn vị trực thuộc Viện Khoa học Giáo dục và Môi " +
                                    "trường (IEES), ra đời với sứ mệnh giúp định hướng nghề " +
                                    "nghiệp cho thế hệ trẻ. Đào tạo cho họ các kỹ thuật " +
                                    "thẩm mỹ không xâm lấn theo phong thủy độc đáo, các kỹ " +
                                    "thuật đả thông kinh lạc, các phương pháp thải độc và " +
                                    "trẻ hóa tế bào. Chuyển giao quy trình mở spa dưỡng " +
                                    "sinh trị liệu.",
                OurDifferenceContent = "Tại Trung Tâm NATS, bạn sẽ không chỉ được trang bị " +
                                        "các kỹ năng cần thiết để thành công trong ngành " +
                                        "chăm sóc sức khỏe và làm đẹp, mà còn được khuyến " +
                                        "khích phát triển bản thân, khám phá tài năng, sự " +
                                        "tự tin và vẻ đẹp bên trong của chính mình." +
                                        Environment.NewLine + Environment.NewLine +
                                        "Đội ngũ chuyên gia giàu kinh nghiệm trong ngành " +
                                        "Thẩm mỹ và Chăm sóc sức khỏe nhiều năm qua tại " +
                                        "Việt Nam đã sáng lập Trung Tâm NATS với mong muốn " +
                                        "tạo ra những học viên ưu tú, được tôn trọng và " +
                                        "công nhận trong ngành. NATS là một cộng đồng học " +
                                        "tập thân thiện và tương trợ lẫn nhau, nơi mỗi " +
                                        "người có thể khám phá tiềm năng, giá trị và hy " +
                                        "vọng của bản thân cho tương lai. Hãy đến với NATS " +
                                        "để khám phá sự khác biệt của chúng tôi và trở " +
                                        "thành một trong những chuyên gia Thẩm mỹ và Chăm " +
                                        "sóc sức khỏe tốt nhất trong ngành!",
                OurCultureContent = "Tuy ới những mục tiêu cao cả trong việc đào tạo những " +
                                    "chuyên gia thẩm mỹ không xâm lấn và chăm sóc sức khỏe " +
                                    "tốt nhất, Trung Tâm Khoa Học Đào Tạo và Thẩm Mỹ Quốc " +
                                    "Gia (NATS) cũng luôn chú trọng tới văn hoá trong môi " +
                                    "trường làm việc của ngành thẩm mỹ và chăm sóc sức khoẻ." +
                                    Environment.NewLine + Environment.NewLine +
                                    "Chúng tôi tin rằng văn hoá là một phần không thể thiếu " +
                                    "trong sự phát triển bền vững của một tổ chức. Tại " +
                                    "NATS, chúng tôi tạo ra một môi trường làm việc tích " +
                                    "cực và hỗ trợ cho đội ngũ nhân viên và học viên. Chúng " +
                                    "tôi khuyến khích sự sáng tạo và đóng góp ý kiến, xây " +
                                    "dựng một cộng đồng thân thiện và đoàn kết."
            };

            _context.AboutUsIntroductions.Add(aboutUsIntroduction);
            _context.SaveChanges();
        }
    }

    private void InitializeMembers()
    {
        if (!_context.Members.Any())
        {
            Console.WriteLine("Initializing Members");
            Faker faker = new Faker("vi");
            List<Member> members = new List<Member>
            {
                new Member
                {
                    FullName = "Đỗ Quang Huyền",
                    RoleName = "Giám đốc",
                    Description = faker.Lorem.Sentences(10),
                    ThumbnailUrl = "/images/front-pages/members/1.png"
                },
                new Member
                {
                    FullName = "Trang Nguyễn",
                    RoleName = "Phó giám đốc / Giảng viên",
                    Description = "Kinh nghiệm 10 năm trong ngành chăm sóc sức khoẻ cộng " +
                                "đồng. Nhiều năm kinh nghiệm đào tạo về mỹ phẩm và sản phẩm " +
                                "chăm sóc sức khoẻ. Giảng viên Thần Số Học.",
                    ThumbnailUrl = "/images/front-pages/members/2.png"
                },
                new Member
                {
                    FullName = "Lan Nguyễn",
                    RoleName = "Giám đốc chi nhánh Trà Vinh",
                    Description = faker.Lorem.Sentences(10),
                    ThumbnailUrl = "/images/front-pages/members/3.png"
                },
                new Member
                {
                    FullName = "Trần Kim Khoa",
                    RoleName = "Giám đốc chi nhánh Trà Vinh",
                    Description = faker.Lorem.Sentences(10),
                    ThumbnailUrl = "/images/front-pages/members/4.png"
                },
            };

            _context.Members.AddRange(members);
            _context.SaveChanges();
        }
    }

    private void InitializeCertificates()
    {
        if (!_context.Certificates.Any())
        {
            Console.WriteLine("Initializing Certificates");
            Certificate certificate = new Certificate
            {
                Name = "Quyết định Thành lập",
                ThumbnailUrl = "/images/front-pages/certificates/1.jpg"
            };

            _context.Certificates.Add(certificate);
            _context.SaveChanges();
        }
    }

    private void InitializeSummaryItems()
    {
        if (!_context.SummaryItems.Any())
        {
            Console.WriteLine("Initializing SummaryItems");
            List<SummaryItem> items = new List<SummaryItem>
            {
                new SummaryItem
                {
                    Name = "Trị liệu" + Environment.NewLine + "cột sống",
                    SummaryContent = string.Join(" ",
                        "Trị liệu bằng phương pháp nắn chỉnh tạo hình thẩm mỹ ",
                        "không xâm lấn cột sống, cơ xương khớp."),
                    DetailContent = string.Join(" ",
                        "Ngày nay, trị liệu  ằng phương pháp nắn chỉnh không xâm lấn cột ",
                        "sống cơ xương khớp đang trở nên thịnh hành, được nhiều người bệnh ",
                        "lựa chọn như một bí quyết chăm sóc sức khỏe, giúp thoát khỏi cơn ",
                        "đau khó chịu do bệnh lý cột sống, cơ xương khớp gây ra.",
                        Environment.NewLine,
                        "1. PHƯƠNG PHÁP NẮN CHỈNH TẠO HÌNH THẨM MỸ KHÔNG XÂM LẤN CỘT SỐNG LÀ ",
                        "GÌ?",
                        Environment.NewLine,
                        "Là phương pháp trị liệu nắn chỉnh bằng tay giúp điều chỉnh đốt sống ",
                        "bị lệch về đúng vị trí, cải thiện độ thẳng của cột sống và các cơ ",
                        "xương khớp, giảm chèn ép rễ thần kinh, giảm đau tự nhiên, tăng ",
                        "cường chức năng của hệ thống thần kinh và tuần hoàn máu trong cột ",
                        "sống và cơ xương khớp, cải thiện chức năng cơ bản của cơ thể, kích ",
                        "thích quá trình tự chữa lành của cơ thể mà không phải dùng thuốc ",
                        "tây hay phẫu thuật.",
                        Environment.NewLine,
                        "2. PHƯƠNG PHÁP TRỊ LIỆU NẮN CHỈNH KHÔNG XÂM LẤN CỘT SỐNG CẢI THIỆN ",
                        "NHỮNG BỆNH NÀO?",
                        Environment.NewLine,
                        "Cải thiện những bệnh liên quan đến cột sống, cơ xương khớp bao gồm: ",
                        "thoát vị đĩa đệm, thoái hóa cột sống, gai cột sống, vẹo cột sống, ",
                        "đau thần kinh tọa, đau thắt lưng, đau đầu, đau vai, tê tay, đau mắt ",
                        "cá, đau đầu gối, đau tê bàn chân, viêm thấp khớp, chấn thương thể ",
                        "thao, các bệnh mãn tính.",
                        Environment.NewLine,
                        "3. ƯU ĐIỂM NỔI BẬT CỦA TRỊ LIỆU BẰNG PHƯƠNG PHÁP NẮN CHỈNH KHÔNG ",
                        "XÂM LẤN CỘT SỐNG, CƠ XƯƠNG KHỚP: CẢI THIỆN BỆNH TẬN GỐC, NGĂN NGỪA ",
                        "BIẾN CHỨNG NGUY HIỂM. ",
                        Environment.NewLine,
                        "So với cách phẫu thuật, uống thuốc Tây hoặc châm cứu, bấm huyệt thì ",
                        "phương pháp đả thông liên hoàn, nắn chỉnh không xâm lấn cột sống, ",
                        "cơ xương khớp ưu việt hơn cả, nhờ cơ chế giải quyết chính xác nguồn ",
                        "gốc gây ra cơn đau. ",
                        Environment.NewLine,
                        "PHÙ HỢP VỚI TẤT CẢ CÁC ĐỐI TƯỢNG: Phương pháp đả thông liên hoàn, ",
                        "nắn chỉnh tạo hình thẩm mỹ không xâm lấn cột sống, cơ xương khớp ",
                        "hoàn toàn phù hợp với tất cả mọi người như trẻ em, người trẻ tuổi, ",
                        "người cao tuổi và thậm chí người đã phẫu thuật bị tái phát."),
                    ThumbnailUrl = "/images/front-pages/summary-items/5.jpg"
                },
                new SummaryItem
                {
                    Name = "Đả thông kinh lạc",
                    SummaryContent = string.Concat(
                        "Dưỡng sinh đả thông kinh lạc -",
                        "bí quyết giữ gìn sức khỏe và chống lão hóa."),
                    DetailContent = string.Join(" ",
                        "Dưỡng sinh đả thông kinh lạc - bí quyết giữ gìn sức khỏe và chống",
                        "lão hóa, đặc biệt hiệu quả với phụ nữ và những người trung niên trở",
                        "đi. Phương pháp này không chỉ cải thiện sức khỏe mà còn làm cho bạn",
                        "đẹp từ bên trong.",
                        Environment.NewLine,
                        "ĐẢ THÔNG KINH LẠC có tầm quan trọng to lớn trong",
                        "việc duy trì sự hoạt động bình thường của khí huyết. Chức năng",
                        "chính của hệ kinh lạc là thống nhất cơ thể, cung cấp dinh dưỡng cho",
                        "toàn bộ cơ thể và đáp ứng với các tác nhân kích thích từ bên trong",
                        "và bên ngoài. Kinh lạc được điều hành bởi khí huyết thông qua mạch",
                        "ẩn và lạc mạch trải rải bên ngoài cơ thể. Quá trình đả thông kinh",
                        "lạc có tác dụng giúp cải thiện sự lưu thông khí huyết, liên kết các",
                        "phần khác nhau của cơ thể và tạo sự kết nối giữa các mạch máu.",
                        Environment.NewLine,
                        "ĐẢ THÔNG KINH LẠC - BÁCH BỆNH KHÔNG SINH à một nguyên tắc cốt lõi",
                        "quan trọng trong y học cổ truyền. Nguyên tắc này ám chỉ rằng việc",
                        "duy trì sự thông kinh lạc, tức là giữ cho các đường lạc mạch và khí",
                        "huyết trong cơ thể luôn lưu thông mạnh mẽ, là một cách quan trọng để",
                        "duy trì sức khỏe và ngăn ngừa các bệnh tật. Trong y học cổ truyền,",
                        "sự cản trở hoặc lưu thông máu không tốt qua các kinh lạc được coi là",
                        "nguyên nhân gây ra nhiều loại bệnh, từ các triệu chứng nhẹ như mất",
                        "ngủ, căng thẳng đến các vấn đề sức khỏe nghiêm trọng hơn như đau",
                        "đầu, rối loạn tiền đình, vấn đề về hệ tiêu hóa, cơ xương khớp...",
                        Environment.NewLine,
                        "ĐẢ THÔNG KINH LẠC MANG LẠI NHIỀU LỢI ÍCH CHO SỨC KHỎE: Giúp cải",
                        "thiện lưu thông khí huyết và tuần hoàn máu trong cơ thể. Điều này có",
                        "tác dụng kích thích cơ thể hoạt động tốt hơn, cung cấp dưỡng chất và",
                        "oxi cho các tế bào và các cơ quan. Giảm căng thẳng và mệt mỏi, cải",
                        "thiện các vấn đề về mạch máu, cải thiện tình trạng đau nửa đầu, rối",
                        "loạn tiền đình, cải thiện giấc ngủ, cải thiện vấn đề đau nhức cơ",
                        "xương khớp, tê bì chân tay... Giúp da của bạn trở nên mịn màng,",
                        "nhuận hồng hơn nhờ tác động trực tiếp lên các huyệt đạo. Giúp điều",
                        "hòa kinh nguyệt cho phụ nữ. Ngoài ra, thông kinh lạc còn giúp phân",
                        "hủy các chất béo, giải độc tố, và tăng cường hoạt động của các cơ",
                        "quan bên trong, giúp cho cơ thể linh hoạt hơn."),
                    ThumbnailUrl = "/images/front-pages/summary-items/6.jpg"
                },
                new SummaryItem
                {
                    Name = "Thải độc tế bào",
                    SummaryContent = string.Join(" ",
                        "Thải độc cơ thể là quá trình loại bỏ các chất độc hại và cặn bã từ",
                        "cơ thể để duy trì sức khỏe và chức năng tối ưu của cơ thể."),
                    DetailContent = string.Join(" ",
                        "Độc tố trong cơ thể là những chất hóa học gây hại cho sức khỏe cơ",
                        "thể. Chúng tích tụ nhiều khiến cơ thể bị nhiễm độc, làm phát sinh",
                        "các bệnh tật: đặc biệt là các bệnh mãn tính như ung thư, gút, tiểu",
                        "đường, tim mạch,... Quá trình thải độc cơ thể là quá trình loại bỏ",
                        "các chất độc hại và cặn bã từ cơ thể để duy trì sức khỏe và chức",
                        "năng tối ưu của cơ thể. Một số phương pháp tự nhiên và lành mạnh để",
                        "thải độc cho cơ thể như: LIỆU PHÁP THẢI ĐỘC TẾ BÀO loại bỏ kim loại",
                        "nặng bằng máy ion lượng tử. LIỆU PHÁP THẢI ĐỘC TẾ BÀO bằng các loại",
                        "thảo dược. LIỆU PHÁP THẢI ĐỘC BẰNG CHẾ ĐỘ ĂN KIÊNG nhằm loại bỏ chất",
                        "độc từ cơ thể. UỐNG NƯỚC ĐỂ THẢI ĐỘC, Uống đủ lượng nước hàng ngày",
                        "giúp tăng cường chức năng thận và giảm lượng chất độc hại trong cơ",
                        "thể. Nước cũng giúp tạo ra nước mồ hôi và nước tiểu, từ đó loại bỏ",
                        "chất độc qua đường tiêu hóa và tiểu tiện.GIẢM STRESS: Stress có thể",
                        "ảnh hưởng tiêu cực đến sức khỏe tổng thể và chức năng của các cơ",
                        "quan trong cơ thể. Các kỹ thuật giảm stress như thiền, yoga, và hít",
                        "thở sâu có thể giúp cơ thể giảm stress và tăng cường quá trình thải",
                        "độc tự nhiên."),
                    ThumbnailUrl = "/images/front-pages/summary-items/7.jpg"
                },
                new SummaryItem
                {
                    Name = "Nhân số học & Tỉnh thức",
                    SummaryContent = string.Join(" ",
                        "Thông qua Nhân Số Học, bạn sẽ mở ra một cánh cửa tới việc giải mã",
                        "bản đồ cuộc đời của mình, thấu hiểu chính mình và những người xung",
                        "quanh một cách sâu sắc. Khám phá những tiềm năng của bạn để định",
                        "hướng nghề nghiệp và hoạch định tương lai thành công."),
                    DetailContent = string.Join(" ",
                        "Người xưa thường có câu: “Thân tâm an lạc”, có nghĩa là cuộc sống",
                        "chỉ trọn vẹn khi chúng ta sở hữu một tâm trí an lành bên trong một",
                        "thân thể khỏe mạnh.Một người có sức khỏe tinh thần tốt sẽ luôn nhìn",
                        "thấy những điều tích cực trong mọi vấn đề, luôn vui vẻ, mạnh mẽ, sẵn",
                        "sàng nghênh đón tất thảy mọi điều dù là may mắn hay chông gai phía",
                        "trước. Sở hữu một tinh thần khỏe mạnh sẽ giúp bạn ăn ngon, ngủ yên,",
                        "tràn trề năng lượng mỗi ngày để năng suất trong công việc và luôn",
                        "tự tin, mỉm cười với cuộc đời. Có thể nói, cảm xúc tích cực chính",
                        "là nền tảng cho phần lớn thành công của mỗi người trong cuộc sống.",
                        "Ngược lại, một tinh thần bất ổn có thể ảnh hưởng trực tiếp đến sức",
                        "khỏe thể chất, năng suất trong công việc và các mối quan hệ xung",
                        "quanh của con người. Những cảm xúc tiêu cực như buồn chán, căng",
                        "thẳng, lo âu diễn ra thường xuyên trong một khoảng thời gian dài có",
                        "thể khiến con người rơi vào tình trạng chán ăn, mất ngủ triền miên,",
                        "hay nghiêm trọng hơn là các căn bệnh liên quan đến đường tiêu hóa,",
                        "hệ thần kinh,... Thể chất và tinh thần của con người luôn có một sợi",
                        "dây liên kết vô cùng chặt chẽ. Bởi vậy, chỉ khi tinh thần khỏe mạnh",
                        "thì sức khỏe thể chất ổn định, cơ thể mới hoạt động “trơn tru” được.",
                        "Thông qua Nhân Số Học , bạn sẽ mở ra một cánh cửa tới việc giải mã",
                        "bản đồ cuộc đời của mình, thấu hiểu chính mình , thấu hiểu vợ chồng,",
                        "bạn bè và những người thân xung quanh một cách sâu sắc. Thực hành",
                        "sống Tỉnh Thức sẽ giúp bạn chữa lành những vết thương trong tâm hồn",
                        "và cải thiện các mối quan hệ của mình. Nhân số học giúp bạn định",
                        "hướng nuôi dạy con và giữ gìn hạnh phúc gia đình. Giúp bạn khám phá",
                        "số phận và mục đích sống của mình, giúp bạn trở thành một người tự",
                        "tin và hiểu biết về bản thân hơn. Khám phá những thế mạnh, thách",
                        "thức, bài học trong cuộc sống, và tiềm năng của bạn để định hướng",
                        "nghề nghiệp và hoạch định tương lai thành công.Ứng dụng trong việc",
                        "tuyển dụng và lựa chọn nhân sự phù hợp cho công việc, cũng như trong",
                        "đàm phán kinh doanh để đạt được thành công hơn.Giúp bạn trở thành",
                        "một người tự do và làm chủ cuộc đời của mình."),
                    ThumbnailUrl = "/images/front-pages/summary-items/8.jpg"
                }
            };

            
            _context.SummaryItems.AddRange(items);
            _context.SaveChanges();
        }
    }

    private void InitializeCourses()
    {
        if (!_context.CatalogItems.Any(ci => ci.Type == CatalogItemType.Course))
        {
            Console.WriteLine("Initializing CatalogItems - Courses");
            Faker faker = new Faker("vi");
            List<CatalogItem> courses = new List<CatalogItem>
            {
                new CatalogItem
                {
                    Name = "Khóa Học Nghệ Thuật Trang Điểm Chuyên Nghiệp",
                    Summary = "Khóa học này tập trung vào việc chăm sóc và điều trị da, bao " +
                            "gồm các phương pháp làm sạch da, massage, và các liệu pháp " +
                            "chăm sóc da mặt chuyên sâu.",
                },
                new CatalogItem
                {
                    Name = "Lớp Học Chăm Sóc Da Toàn Diện",
                    Summary = "Chương trình này cung cấp các kỹ năng cần thiết về trang " +
                            "điểm từ cơ bản đến nâng cao, giúp học viên trở thành chuyên " +
                            "gia trang điểm chuyên nghiệp.",
                },
                new CatalogItem
                {
                    Name = "Chương Trình Đào Tạo Nghệ Thuật Làm Tóc",
                    Summary = "Dành cho những ai muốn trở thành nhà tạo mẫu tóc chuyên " +
                            "nghiệp, chương trình này bao gồm cắt, nhuộm, tạo kiểu tóc và " +
                            "các kỹ thuật làm tóc khác.",
                },
                new CatalogItem
                {
                    Name = "Khóa Học Nail Nghệ Thuật và Thiết Kế",
                    Summary = "Cung cấp kiến thức và kỹ năng từ cơ bản đến nâng cao trong " +
                            "lĩnh vực làm nail, bao gồm vẽ nail, phủ gel, và thiết kế nail " +
                            "nghệ thuật.",
                },
            };

            foreach (CatalogItem course in courses)
            {
                course.Type = CatalogItemType.Course;
                course.Detail = faker.Lorem.Paragraph(5) + Environment.NewLine +
                                faker.Lorem.Paragraph(8) + Environment.NewLine +
                                faker.Lorem.Paragraph(10);

                foreach (CatalogItemPhoto photo in course.Photos ?? [])
                {
                    photo.Description = faker.Lorem.Sentences(3);
                }

                _context.Add(course);
            }

            _context.SaveChanges();
        }
    }

    private void InitializeServices()
    {
        if (!_context.CatalogItems.Any(ci => ci.Type == CatalogItemType.Service))
        {
            Console.WriteLine("Initializing CatalogItems - Services");
            Faker faker = new Faker("vi");
            List<CatalogItem> services = new List<CatalogItem>
            {
                new CatalogItem
                {
                    Name = "Dịch vụ massage toàn thân",
                    Summary = "Dùng các kỹ thuật massage truyền thống kết hợp với tinh dầu " +
                            "tự nhiên để thư giãn cơ bắp, giảm stress và cải thiện lưu " +
                            "thông máu.",
                    ThumbnailUrl = "/images/front-pages/services/1.jpg",
                    Photos = new List<CatalogItemPhoto>
                    {
                        new CatalogItemPhoto { Url = "/images/front-pages/services/1_1.jpg" },
                        new CatalogItemPhoto { Url = "/images/front-pages/services/1_2.jpg" },
                        new CatalogItemPhoto { Url = "/images/front-pages/services/1_3.jpg" },
                        new CatalogItemPhoto { Url = "/images/front-pages/services/1_4.jpg" },
                        new CatalogItemPhoto { Url = "/images/front-pages/services/1_5.jpg" },
                        new CatalogItemPhoto { Url = "/images/front-pages/services/1_6.jpg" },
                    } 
                },
                new CatalogItem
                {
                    Name = "Liệu pháp da mặt chống lão hóa",
                    Summary = "Sử dụng các sản phẩm chăm sóc da cao cấp và công nghệ tiên " +
                            "tiến để giảm thiểu các dấu hiệu lão hóa, làm mờ nếp nhăn, và " +
                            "tái tạo làn da.",
                    ThumbnailUrl = "/images/front-pages/services/2.jpg",
                    Photos = new List<CatalogItemPhoto>
                    {
                        new CatalogItemPhoto { Url = "/images/front-pages/services/2_1.jpg" },
                        new CatalogItemPhoto { Url = "/images/front-pages/services/2_2.jpg" },
                        new CatalogItemPhoto { Url = "/images/front-pages/services/2_3.jpg" }
                    } 
                },
                new CatalogItem
                {
                    Name = "Dịch vụ tắm trắng toàn thân",
                    Summary = "Kết hợp giữa tắm hơi và sử dụng hỗn hợp tinh chất tự nhiên " +
                            "giúp làm sáng da, mờ vết thâm và cung cấp dưỡng chất.",
                    ThumbnailUrl = "/images/front-pages/services/3.jpg",
                    Photos = new List<CatalogItemPhoto>
                    {
                        new CatalogItemPhoto { Url = "/images/front-pages/services/3_1.jpg" },
                    } 
                },
                new CatalogItem
                {
                    Name = "Dịch vụ chăm sóc móng tay/móng chân",
                    Summary = "Cung cấp dịch vụ làm sạch, tạo hình, và sơn móng chuyên " +
                            "nghiệp, kèm theo liệu pháp dưỡng ẩm cho da tay/da chân và " +
                            "massage nhẹ nhàng.",
                    ThumbnailUrl = "/images/front-pages/services/4.jpg",
                },
            };

            foreach (CatalogItem service in services)
            {
                service.Type = CatalogItemType.Service;
                service.Detail = faker.Lorem.Paragraph(5) + Environment.NewLine +
                                faker.Lorem.Paragraph(8) + Environment.NewLine +
                                faker.Lorem.Paragraph(10);

                foreach (CatalogItemPhoto photo in service.Photos ?? [])
                {
                    photo.Description = faker.Lorem.Sentences(3);
                }

                _context.CatalogItems.Add(service);
            }

            _context.SaveChanges();
        }
    }

    private void InitializeSliderItems()
    {
        if (!_context.SliderItems.Any())
        {
            Console.WriteLine("Initializing SliderItems");
            string[] photoUrls = new string[]
            {
                "/images/front-pages/slider-items/1.jpg",
                "/images/front-pages/slider-items/3.jpg"
            };

            for (int i = 0; i < photoUrls.Length; i++)
            {
                SliderItem item = new SliderItem
                {
                    ThumbnailUrl = photoUrls[i],
                    Index = i
                };

                _context.SliderItems.Add(item);
            }

            _context.SaveChanges();
        }
    }
    
    private void InitializePosts()
    {
        // Initialize posts.
        if (!_context.Posts.Any())
        {
            Console.WriteLine("Initializing Posts");
            Faker faker = new Faker("vi");
            Random random = new Random();
            for (int i = 0; i < 30; i++)
            {
                string title = faker.Lorem.Sentence(20);
                Post post = new Post
                {
                    Title = title,
                    NormalizedTitle = NormalizedTitleProhibitedCharactersRegex()
                        .Replace(
                            title.ToNonDiacritics()
                                .ToLower()
                                .Replace(" ", "-")
                                .Replace("đ", "d"),
                            ""),
                    Content = faker.Lorem.Paragraphs(random.Next(15, 20), Environment.NewLine),
                    UserId = _context.Users
                        .Where(u => u.UserName == "ngokhanhhuyy")
                        .Select(u => u.Id)
                        .Single(),
                };
                _context.Posts.Add(post);
                _context.SaveChanges();
            }
        }
        else
        {
            List<Post> posts = _context.Posts.ToList();
            foreach (Post post in posts)
            {
                post.NormalizedTitle = post.NormalizedTitle
                    .Replace(".", "")
                    .Replace(",", "")
                    .ToLower();
            }

            _context.SaveChanges();
        }
    }
    
    private void InitializeContacts()
    {
        if (!_context.Contacts.Any())
        {
            Console.WriteLine("Initializing Contacts");
            List<Contact> contacts = new List<Contact>
            {
                new Contact
                {
                    Type = ContactType.PhoneNumber,
                    Content = "0914 64 0979",
                },
                new Contact
                {
                    Type = ContactType.ZaloNumber,
                    Content = "0914 64 0979",
                },
                new Contact
                {
                    Type = ContactType.Email,
                    Content = "thammyquocgia@gmail.com",
                },
                new Contact
                {
                    Type = ContactType.Address,
                    Content = "21 Phan Đăng Lưu, phường Tân An, " +
                            "thành phố Buôn Ma Thuột, tỉnh Đắk Lắk",
                },
            };

            _context.Contacts.AddRange(contacts);
        }

        _context.SaveChanges();
    }
    
    [GeneratedRegex(@"[.,\?\-:;/><\(\)]")]
    private static partial Regex NormalizedTitleProhibitedCharactersRegex();
}